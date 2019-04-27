using Microsoft.Xna.Framework;
using MySql.Data.MySqlClient;
using ServerSideCharacter2.Core;
using ServerSideCharacter2.Utils;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace ServerSideCharacter2.Services.Login
{
	public delegate void AuthorizationHandler(ServerPlayer player);
	public class Authorization : ISSCNetHandler
	{

		private void SuccessLogin(ServerPlayer player)
		{
			player.IsLogin = true;
			player.ClearAllBuffs();
			NetMessage.SendData(MessageID.PlayerBuffs, -1, -1, NetworkText.Empty, player.PrototypePlayer.whoAmI, 0f, 0f, 0f, 0, 0, 0);
		}

		public static event EventHandler OnPlayerRegistered;
		public static event AuthorizationHandler OnPlayerLogin;

		private static string bannedchars = "$%^&*!@#:?|<>";

		/// <summary>
		/// 检测名字是否符合要求，如果名字长度小于2或者大于10就不合法
		/// </summary>
		/// <param name="name"></param>
		/// <returns>如果过短返回-1，过长返回1，否则返回0</returns>
		public static int CheckName(string name)
		{
			if (name.Length < 2)
			{
				return -1;
			}
			else if (name.Length > 10)
			{
				return 1;
			}
			foreach(var c in name)
			{
				if (bannedchars.Contains(c))
					return 2;
			}
			return 0;
		}

		private void RecordVisit(int id, ServerPlayer player)
		{
			var ipaddr = Netplay.Clients[id].Socket.GetRemoteAddress().GetIdentifier();
			QQAuth.MySqlManager sqlmanager = new QQAuth.MySqlManager();
			sqlmanager.Connect();
			MySqlCommand cmd = sqlmanager.command;
			cmd.CommandText = $"update users set lastIP = '{ipaddr}' where QQ = '{player.qqAuth.QQ}'";
			cmd.ExecuteNonQuery();
			cmd.Cancel();
		}

		public void Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 2)
			{
                var encrypted = reader.ReadString();
				// 解密RSA加密的信息
				var info = CryptedUserInfo.GetDecrypted(encrypted);
				var serverPlayer = Main.player[playerNumber].GetServerPlayer();
                serverPlayer.qqAuth.CharacterName = Main.player[playerNumber].name;
                serverPlayer.qqAuth.MachineCode = info.MachineCode;
                if (serverPlayer.IsLogin)
				{
					MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "你已经登录，请不要重复登录");
					return;
				}
                if (serverPlayer.HasPassword)
				{
                    bool isLoginSuccess = false;
                    QQAuth.States.LoginState loginState = serverPlayer.qqAuth.Login(info);
                    switch(loginState)
                    {
                        case QQAuth.States.LoginState.Debug:
                            CommandBoardcast.ConsoleMessage("Debug模式已启用，跳过登录验证.");
                            isLoginSuccess = true;
                            break;
                        case QQAuth.States.LoginState.Unbound:
                            CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 认证失败：未绑定QQ.");
                            MessageSender.SendLoginFailed(playerNumber, "请先绑定QQ！");
                            isLoginSuccess = false;
                            break;
                        case QQAuth.States.LoginState.Banned:
							
                            CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 认证失败：玩家已被封禁.");
                            MessageSender.SendLoginFailed(playerNumber, $"您已被封禁！原因：{serverPlayer.qqAuth.GetBanReason(serverPlayer)}");
                            isLoginSuccess = false;
                            break;
                        case QQAuth.States.LoginState.LoginSuccess:
                            CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} ,QQ {serverPlayer.qqAuth.QQ} 认证成功.");
                            isLoginSuccess = true;
                            break;
                        case QQAuth.States.LoginState.ChangePasswordRequired:
                            serverPlayer.HasPassword = false;
                            CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} ,QQ {serverPlayer.qqAuth.QQ} 申请改密.");
                            MessageSender.SendLoginFailed(playerNumber, "申请改密已受理！请输入QQ和新密码完成改密。");
                            isLoginSuccess = false;
                            break;
                        case QQAuth.States.LoginState.GetMCFailed:
                            CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 认证失败：机器码获取失败.");
                            MessageSender.SendLoginFailed(playerNumber, "机器码获取失败！请确认机器已经注册。");
                            isLoginSuccess = false;
                            break;
                        case QQAuth.States.LoginState.MCCheckFailed:
                            CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 认证失败：机器码不正确.");
                            MessageSender.SendLoginFailed(playerNumber, "你不能使用非当前角色绑定的机器！");
                            isLoginSuccess = false;
                            break;
                        case QQAuth.States.LoginState.Error:
                            CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 登录错误，信息：" + serverPlayer.qqAuth.ErrorLog);
                            MessageSender.SendLoginFailed(playerNumber, "数据库操作出错！");
                            isLoginSuccess = false;
                            break;
                        default:
                            isLoginSuccess = false;
                            break;
                    }
                    if (isLoginSuccess)
                    {
                        if (serverPlayer.CheckPassword(info))
                        {
                            SuccessLogin(serverPlayer);
                            MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "认证成功");
							if (!ServerSideCharacter2.DEBUGMODE)
							{
								OnPlayerLogin?.Invoke(serverPlayer);
							}
							RecordVisit(playerNumber, serverPlayer);
							// 告诉客户端解除封印
							MessageSender.SendLoginIn(serverPlayer.PrototypePlayer.whoAmI);
							NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(serverPlayer.Name + " 登入了游戏"), new Color(255, 255, 240, 20), -1);
							CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 认证成功.");
                        }
                        else
                        {
							// 如果忘记密码就要找管理员重置密码
							MessageSender.SendLoginFailed(playerNumber, "密码错误！");
                            CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 认证失败：密码错误.");
                        }
                    }
                }
                else
				{
					var result = CheckName(Main.player[playerNumber].name);
                    if (result == 0)
                    {
                        bool isRegisterLegal = false;
                        QQAuth.States.RegisterState registerState = serverPlayer.qqAuth.Register(info);
                        switch (registerState)
                        {
                            case QQAuth.States.RegisterState.Debug:
                                CommandBoardcast.ConsoleMessage("Debug模式已启用，跳过注册验证.");
                                isRegisterLegal = true;
                                break;
                            case QQAuth.States.RegisterState.NullQQ:
                                MessageSender.SendLoginFailed(playerNumber, "注册时QQ不能为空！");
                                isRegisterLegal = false;
                                break;
                            case QQAuth.States.RegisterState.RegisterSuccess:
                                CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册请求合法（常规注册）.");
                                isRegisterLegal = true;
								if (!ServerSideCharacter2.DEBUGMODE)
								{
									OnPlayerRegistered?.Invoke(serverPlayer, new EventArgs());
								}
								break;
                            case QQAuth.States.RegisterState.RegisterRep:
                                CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册请求合法（角色可能丢失）.");
                                isRegisterLegal = true;
                                break;
                            case QQAuth.States.RegisterState.QQBound:
                                MessageSender.SendLoginFailed(playerNumber, "该QQ已被其他角色绑定！");
                                CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册请求被拒（QQ已被绑定）.");
                                isRegisterLegal = false;
                                break;
                            case QQAuth.States.RegisterState.GetMCFailed:
                                MessageSender.SendLoginFailed(playerNumber, "机器码获取失败！请确认机器已经注册。");
                                CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册请求被拒（获取机器码失败）.");
                                isRegisterLegal = false;
                                break;
                            case QQAuth.States.RegisterState.MCBound:
                                MessageSender.SendLoginFailed(playerNumber, "该机器已被其他角色绑定！");
                                CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册请求被拒（机器已被绑定）.");
                                isRegisterLegal = false;
                                break;
                            case QQAuth.States.RegisterState.Error:
                                MessageSender.SendLoginFailed(playerNumber, "数据库操作出错！");
                                CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册出现错误，信息：" + serverPlayer.qqAuth.ErrorLog);
                                isRegisterLegal = false;
                                break;
                            default:
                                isRegisterLegal = false;
                                break;
                        }
                        if (isRegisterLegal)
                        {
                            serverPlayer.SetPassword(info);
                            // SuccessLogin(serverPlayer);
                            MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "注册成功，输入密码即可登录");
                            // 告诉客户端解除封印
                            // MessageSender.SendLoginIn(serverPlayer.PrototypePlayer.whoAmI);
                            CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册成功.");
						}
                    }
                    else
					{
						if (result == 1 || result == -1)
						{
							serverPlayer.SendMessageBox("无法注册玩家：用户名" +
								(result == 1 ? "过长" : "过短") + "\n" + "用户名应为2-10个字符", 120, Color.Red);
						}
						else
						{
							serverPlayer.SendMessageBox("无法注册玩家：用户名不能含有下列特殊字符：$%^&*!@#:?|<>", 120, Color.Red);
						}
					}
				}
			}
		}
	}
}
