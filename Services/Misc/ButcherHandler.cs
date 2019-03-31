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
	public class ButcherHandler : SSCCommandHandler
	{
		public override string PermissionName => "butcher";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var p = Main.player[playerNumber];
				var player = p.GetServerPlayer();
				var kills = 0;
				for (var i = 0; i < Main.npc.Length; i++)
				{
					if (Main.npc[i].active && ((!Main.npc[i].townNPC && Main.npc[i].netID != NPCID.TargetDummy)))
					{
						var dmg = (int)(Main.npc[i].life + (Main.npc[i].defense * 0.6));
						Main.npc[i].StrikeNPC(dmg, 0, 0);
						NetMessage.SendData(MessageID.StrikeNPC, -1, -1, NetworkText.Empty, i, dmg, 0, 0);
						kills++;
					}
				}
				ServerPlayer.SendInfoMessageToAll($"{player.Name} 杀死了 {kills} 个怪物.");
				CommandBoardcast.ConsoleMessage($"{player.Name} 杀死了 {kills} 个怪物.");
			}
		}

	}
}
