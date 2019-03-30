using Microsoft.Xna.Framework;
using ServerSideCharacter2.Core;
using ServerSideCharacter2.Crypto;
using ServerSideCharacter2.Utils;
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
				string encrypted = reader.ReadString();
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
                    //以下为QQ绑定验证代码
                    //bool isAuthSuccess 为验证状态
                    //string QQ 为用户绑定的QQ号（如果已绑定）
                    bool isAuthSuccess = ServerSideCharacter2.DEBUGMODE;
					if (!ServerSideCharacter2.DEBUGMODE)
					{
						string ClientMD5Key = "This is the CLIENT key.";
						string ServerMD5Key = "This is the SERVER key.";
						string username = serverPlayer.Name;
						char[] constant =
						{
						'0','1','2','3','4','5','6','7','8','9',
						'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
						'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
						};
						System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
						System.Random rd = new System.Random();
						//salt暂定长度为10
						for (int i = 0; i < 10; i++)
						{
							newRandom.Append(constant[rd.Next(62)]);
						}
						string salt = newRandom.ToString();
						try
						{
							System.Security.Cryptography.MD5CryptoServiceProvider md5CryptoServiceProvider = new System.Security.Cryptography.MD5CryptoServiceProvider();
							byte[] bytes = Encoding.UTF8.GetBytes(username + salt + ClientMD5Key);
							bytes = md5CryptoServiceProvider.ComputeHash(bytes);
							md5CryptoServiceProvider.Clear();
							string s_return = "";
							for (int i2 = 0; i2 < bytes.Length; i2++)
							{
								s_return += System.Convert.ToString(bytes[i2], 16).PadLeft(2, '0');
							}
							string check = s_return.PadLeft(32, '0');
							string pageData = Encoding.UTF8.GetString(new System.Net.WebClient
							{
								Credentials = System.Net.CredentialCache.DefaultCredentials
							}.DownloadData(string.Concat(new string[]
							{
							"http://peserver.terrariaserver.cn/SteamCityAuth.aspx?username=",
							username,
							"&salt=",
							salt,
							"&check=",
							check
							})));
							string _state = pageData.Substring(pageData.IndexOf("<span id=\"statelabel\">") + 22, pageData.IndexOf("</span>", pageData.IndexOf("<span id=\"statelabel\">")) - pageData.IndexOf("<span id=\"statelabel\">") - 22);
							string _check = pageData.Substring(pageData.IndexOf("<span id=\"checklabel\">") + 22, pageData.IndexOf("</span>", pageData.IndexOf("<span id=\"checklabel\">")) - pageData.IndexOf("<span id=\"checklabel\">") - 22);
							//QQ号不参与验证，可用于扩展功能
							string QQ = pageData.Substring(pageData.IndexOf("<span id=\"qqnumlabel\">") + 22, pageData.IndexOf("</span>", pageData.IndexOf("<span id=\"qqnumlabel\">")) - pageData.IndexOf("<span id=\"qqnumlabel\">") - 22);
							if (_state == "true")
							{
								System.Security.Cryptography.MD5CryptoServiceProvider md5CryptoServiceProvider2 = new System.Security.Cryptography.MD5CryptoServiceProvider();
								byte[] _bytes = Encoding.UTF8.GetBytes(username + salt + ServerMD5Key);
								_bytes = md5CryptoServiceProvider2.ComputeHash(_bytes);
								md5CryptoServiceProvider2.Clear();
								string _s_return = "";
								for (int ii = 0; ii < _bytes.Length; ii++)
								{
									_s_return += System.Convert.ToString(_bytes[ii], 16).PadLeft(2, '0');
								}
								if (_check == _s_return.PadLeft(32, '0'))
								{
									//验证成功
									isAuthSuccess = true;
								}
								else
								{
									//验证失败 MD5校验失败
									MessageSender.SendLoginFailed(playerNumber, "MD5校验失败！");
									isAuthSuccess = false;
								}
							}
							else if (_state == "false")
							{
								//验证失败 用户未绑定QQ
								MessageSender.SendLoginFailed(playerNumber, "请先绑定QQ！");
								isAuthSuccess = false;
							}
							else
							{
								//验证失败 网络错误或其他原因
								MessageSender.SendLoginFailed(playerNumber, "网络错误！");
								isAuthSuccess = false;
							}
						}
						catch
						{
							//验证失败 程序出错
							MessageSender.SendLoginFailed(playerNumber, "程序错误！");
							isAuthSuccess = false;
						}
						//QQ绑定验证结束
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
					int result = CheckName(Main.player[playerNumber].name);
					if (result == 0)
					{
						serverPlayer.SetPassword(info);
						// SuccessLogin(serverPlayer);
						MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "注册成功");
						// 告诉客户端解除封印
						// MessageSender.SendLoginIn(serverPlayer.PrototypePlayer.whoAmI);
						CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册成功.");
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
