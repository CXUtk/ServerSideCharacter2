using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Localization;

namespace ServerSideCharacter2.Services.Misc
{
	public class TPHandler : SSCCommandHandler
	{
		public override string PermissionName => "tp";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				int target = reader.ReadByte();
				var player = Main.player[playerNumber].GetServerPlayer();
				var targetPlayer = Main.player[target].GetServerPlayer();
				if (targetPlayer.PrototypePlayer != null && targetPlayer.PrototypePlayer.active)
				{
					//if (targetPlayer.PrototypePlayer.hostile || player.PrototypePlayer.hostile)
					//{
					//	player.SendMessageBox("PVP状态不允许传送", Color.Red);
					//}
					//else
					//{
					Main.player[playerNumber].Teleport(Main.player[target].position);
					if (Main.netMode == 2)
					{
						NetMessage.SendData(65, -1, -1, null, 0, (float)playerNumber, Main.player[target].position.X, Main.player[target].position.Y, 0, 0, 0);
					}
					player.SendInfoMessage("你传送到了 " + targetPlayer.Name + " 身边");
					targetPlayer.SendInfoMessage(player.Name + " 传送到了你身边");
					CommandBoardcast.ConsoleMessage($"玩家 {player.Name} 传送到了 {targetPlayer.Name} 身边");
					//}
				}
				else
				{
					player.SendErrorInfo("找不到这个玩家");
				}
			}
		}
	}

	public class TPHereHandler : SSCCommandHandler
	{
		public override string PermissionName => "tphere";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				int target = reader.ReadByte();
				var player = Main.player[playerNumber].GetServerPlayer();
				var targetPlayer = Main.player[target].GetServerPlayer();
				if (targetPlayer.PrototypePlayer != null && targetPlayer.PrototypePlayer.active)
				{
					Main.player[target].Teleport(Main.player[playerNumber].position);
					if (Main.netMode == 2)
					{
						NetMessage.SendData(65, -1, -1, null, 0, (float)target, Main.player[playerNumber].position.X, Main.player[playerNumber].position.Y, 0, 0, 0);
					}
					player.SendInfoMessage("成功让 " + targetPlayer.Name + " 强行传送至你身边");
					targetPlayer.SendInfoMessage($"你被强制传送到了 {player.Name} 身边");
					CommandBoardcast.ConsoleMessage($"玩家 {targetPlayer.Name} 被 {player.Name} 强行抓走了");
				}
				else
				{
					player.SendErrorInfo("找不到这个玩家");
				}
			}
		}
	}

}
