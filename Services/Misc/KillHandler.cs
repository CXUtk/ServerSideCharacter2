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
using Terraria.ModLoader;

namespace ServerSideCharacter2.Services.Misc
{
	public class KillHandler : SSCCommandHandler
	{
		public override string PermissionName => "kill";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				int target = reader.ReadByte();
				var player = Main.player[playerNumber];
				var target0 = Main.player[target];
				var target1 = target0.GetServerPlayer();

				var str = $"玩家 {player.name} 击杀了玩家 {target0.name} ";
				target1.Kill($"{target1.Name} 遭到了天谴……");
				if (player.GetServerPlayer().ContainsValueName("罪恶值"))
				{
					player.GetServerPlayer().ModifyExtraValue("罪恶值", (int)player.GetServerPlayer().GetExtraValue("罪恶值") + 1);
				}
				CommandBoardcast.ConsoleMessage(str);
			}
		}
	}


}
