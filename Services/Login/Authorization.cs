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

		public bool Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 2)
			{
				string encrypted = reader.ReadString();
				var info = CryptedUserInfo.GetDecrypted(encrypted);
				var serverPlayer = Main.player[playerNumber].GetServerPlayer();
				if (serverPlayer.IsLogin)
				{
					MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "你已经登录，请不要重复登录");
					return false;
				}
				if (serverPlayer.HasPassword)
				{
					if (serverPlayer.CheckPassword(info))
					{
						SuccessLogin(serverPlayer);
						MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "认证成功");
						MessageSender.SendLoginIn(serverPlayer.PrototypePlayer.whoAmI);
						CommandBoardcast.ConsoleMessage("玩家 " + serverPlayer.Name + " 认证成功");
					}
					else
					{
						// 如果忘记密码就要找管理员重置密码
						MessageSender.SendLoginFailed(playerNumber, "密码错误！");
					}
				}
				else
				{
					int result = CheckName(Main.player[playerNumber].name);
					if (result == 0)
					{
						serverPlayer.SetPassword(info);
						SuccessLogin(serverPlayer);
						MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "注册成功");
						MessageSender.SendLoginIn(serverPlayer.PrototypePlayer.whoAmI);
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
			return false;
		}
	}
}
