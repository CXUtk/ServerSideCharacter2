using Microsoft.Xna.Framework;
using MySql.Data.MySqlClient;
using ServerSideCharacter2.Core;
using ServerSideCharacter2.Crypto;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace ServerSideCharacter2.Services.Login
{
	public class Authorization : ISSCNetHandler
	{

		private void SuccessLogin(ServerPlayer player)
		{
			player.IsLogin = true;
			player.ClearAllBuffs();
			NetMessage.SendData(MessageID.PlayerBuffs, -1, -1, NetworkText.Empty, player.PrototypePlayer.whoAmI, 0f, 0f, 0f, 0, 0, 0);
		}

		private static string bannedchars = "$%^&*!@#:?|";

		/// <summary>
		/// 检测名字是否符合要求，如果名字长度小于2或者大于10就不合法
		/// </summary>
		/// <param name="name"></param>
		/// <returns>如果过短返回-1，过长返回1，否则返回0</returns>
		private int CheckName(string name)
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

		public void Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 2)
			{
                var _constr = "server=localhost;User Id=mysqlserver;Password=258741369;Database=steamcityqqauth";
                var encrypted = reader.ReadString();
				// 解密RSA加密的信息
				var info = CryptedUserInfo.GetDecrypted(encrypted);
				// info.UserName 目前是 "用户名即为玩家名字"
				var serverPlayer = Main.player[playerNumber].GetServerPlayer();
				if (serverPlayer.IsLogin)
				{
					MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "你已经登录，请不要重复登录");
					return;
				}
				if (serverPlayer.HasPassword)
				{
                    // QQ验证模块 登录绑定验证
                    var isAuthSuccess = false;
                    string QQ = "";
                    string OpenID = "";
                    try
                    {
                        MySqlConnection mycon = new MySqlConnection(_constr);
                        mycon.Open();
                        MySqlCommand cmd = new MySqlCommand("set names utf8", mycon);
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "select qq,openid from users where username = @UserName";
                        cmd.Parameters.AddWithValue("@UserName", serverPlayer.Name);
                        MySqlDataReader mdr = cmd.ExecuteReader();
                        if (mdr.Read())
                        {
                            QQ = mdr["qq"].ToString();
                            OpenID = mdr["openid"].ToString();
                        }
                        mdr.Close();
                        cmd.Cancel();
                        mycon.Close();
                        if (QQ == "" || OpenID == "")
                        {
                            // 用户未绑定QQ
                            CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 认证失败：未绑定QQ.");
                            MessageSender.SendLoginFailed(playerNumber, "请先绑定QQ！");
                            isAuthSuccess = false;
                        }
                        else
                        {
                            // 用户已绑定QQ
                            isAuthSuccess = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // 程序出错
                        MessageSender.SendLoginFailed(playerNumber, "数据库操作出错！");
                        CommandBoardcast.ConsoleMessage("QQ验证模块 登录验证 出现错误，信息：" + ex.Message);
                        isAuthSuccess = false;
                    }
                    if (isAuthSuccess)
                    {
                        if (serverPlayer.CheckPassword(info))
                        {
                            SuccessLogin(serverPlayer);
                            MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "认证成功");
                            // 告诉客户端解除封印
                            MessageSender.SendLoginIn(serverPlayer.PrototypePlayer.whoAmI);
                            CommandBoardcast.ConsoleMessage("玩家 " + serverPlayer.Name + " 认证成功");
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
                        // QQ验证模块 新用户注册验证
                        bool isAuthSuccess = false;
                        string QQ = info.UserName;
                        string UserName = "";
                        try
                        {
                            MySqlConnection mycon = new MySqlConnection(_constr);
                            mycon.Open();
                            MySqlCommand cmd = new MySqlCommand("set names utf8", mycon);
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = "select username from users where qq = @QQ";
                            cmd.Parameters.AddWithValue("@QQ", QQ);
                            MySqlDataReader mdr = cmd.ExecuteReader();
                            if (mdr.Read())
                            {
                                UserName = mdr["username"].ToString();
                            }
                            mdr.Close();
                            cmd.Cancel();
                            mycon.Close();
                            if (UserName == "")
                            {
                                // QQ未绑定到角色，允许注册
                                try
                                {
                                    MySqlConnection _mycon = new MySqlConnection(_constr);
                                    _mycon.Open();
                                    MySqlCommand _cmd = new MySqlCommand("set names utf8", _mycon);
                                    _cmd.CommandType = System.Data.CommandType.Text;
                                    _cmd.CommandText = "insert into users set qq = @QQ , username = @UserName";
                                    _cmd.Parameters.AddWithValue("@QQ", QQ);
                                    _cmd.Parameters.AddWithValue("@UserName", serverPlayer.Name);
                                    _cmd.ExecuteNonQuery();
                                    _cmd.Cancel();
                                    _mycon.Close();
                                    isAuthSuccess = true;
                                    CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册请求合法.");
                                }
                                catch (Exception ex)
                                {
                                    // 程序出错
                                    MessageSender.SendLoginFailed(playerNumber, "数据库操作出错！");
                                    CommandBoardcast.ConsoleMessage("QQ验证模块 用户注册 出现错误，信息：" + ex.Message);
                                    isAuthSuccess = false;
                                }
                            }
                            else if(UserName == serverPlayer.Name)
                            {
                                // QQ已被自己绑定，允许注册
                                isAuthSuccess = true;
                            }
                            else
                            {
                                // QQ已被其他角色绑定，禁止注册
                                MessageSender.SendLoginFailed(playerNumber, "该QQ已被其他角色绑定！");
                                CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册请求被拒.");
                                isAuthSuccess = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            // 程序出错
                            MessageSender.SendLoginFailed(playerNumber, "数据库操作出错！");
                            CommandBoardcast.ConsoleMessage("QQ验证模块 注册验证 出现错误，信息：" + ex.Message);
                            isAuthSuccess = false;
                        }
                        if (isAuthSuccess)
                        {
                            serverPlayer.SetPassword(info);
                            // SuccessLogin(serverPlayer);
                            MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "注册成功");
                            // 告诉客户端解除封印
                            // MessageSender.SendLoginIn(serverPlayer.PrototypePlayer.whoAmI);
                            CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册成功.");
                        }
					}
					else
					{
						if (result == 1 || result == -1)
						{
							MessageSender.SendLoginFailed(serverPlayer.PrototypePlayer.whoAmI, "无法注册玩家：用户名" +
								(result == 1 ? "过长" : "过短") + "\n" + "用户名应为2-10个字符");
						}
						else
						{
							MessageSender.SendLoginFailed(serverPlayer.PrototypePlayer.whoAmI, "无法注册玩家：用户名不能含有下列特殊字符：$%^&*!@#:?|");
						}
					}
				}
			}
		}
	}
}
