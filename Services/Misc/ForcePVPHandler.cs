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
				PVPMode mode = (PVPMode)reader.ReadByte();
				string s = $"玩家 {Main.player[playerNumber].name} 将PVP模式设置为 {mode.ToString()}";

				ServerSideCharacter2.Config.PvpMode = mode;

				foreach (var player in Main.player)
				{
					if (player.active)
					{
						player.hostile = (mode == PVPMode.Always ? true : false);
					}
					NetMessage.SendData(MessageID.PlayerPvP, -1, -1, NetworkText.FromLiteral(""), player.whoAmI);
				}

				ServerPlayer.SendInfoMessageToAll(s);
				CommandBoardcast.ConsoleMessage(s);
			}
		}
	}
}
