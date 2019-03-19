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
	public class Authorization : ISSCNetService
	{

		private void successLogin(ServerPlayer player)
		{
			player.IsLogin = true;
			player.ClearAllBuffs();
			NetMessage.SendData(MessageID.PlayerBuffs, -1, -1, NetworkText.Empty, player.PrototypePlayer.whoAmI, 0f, 0f, 0f, 0, 0, 0);
		}

		public bool Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 2)
			{
				string encrypted = reader.ReadString();
				var info = CryptedUserInfo.GetDecrypted(encrypted);
				var serverPlayer = Main.player[playerNumber].GetServerPlayer();
				if (serverPlayer.HasPassword)
				{
					if (serverPlayer.CheckPassword(info))
					{
						successLogin(serverPlayer);
						MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "认证成功");
						CommandBoardcast.ConsoleMessage("玩家 " + serverPlayer.Name + " 认证成功");
					}
					else
					{
						MessageSender.SendLoginFailed(playerNumber, "密码错误！");
					}
				}
				else
				{
					serverPlayer.SetPassword(info);
					successLogin(serverPlayer);
					MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "注册成功");
				}
			}
			return false;
		}
	}
}
