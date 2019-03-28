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
using Terraria.ID;
using Terraria.Localization;

namespace ServerSideCharacter2.Services.Misc
{
	public class ForcePVPHandler : SSCCommandHandler
	{
		public override string PermissionName => "forcepvp";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				string s = $"玩家 {Main.player[playerNumber].name} {(ServerSideCharacter2.Config.PvpMode != PVPMode.Always ? "开启" : "关闭")}了强制pvp模式";
				if (ServerSideCharacter2.Config.PvpMode != PVPMode.Always)
				{
					ServerSideCharacter2.Config.PvpMode = PVPMode.Always;
					foreach(var player in Main.player)
					{
						if (player.active)
						{
							player.hostile = true;
						}
						NetMessage.SendData(MessageID.PlayerPvP, -1, -1, NetworkText.FromLiteral(""), player.whoAmI);
					}
				}
				else
				{
					ServerSideCharacter2.Config.PvpMode = PVPMode.Normal;
				}
				ServerPlayer.SendInfoMessageToAll(s);
				CommandBoardcast.ConsoleMessage(s);
			}
		}
	}
}
