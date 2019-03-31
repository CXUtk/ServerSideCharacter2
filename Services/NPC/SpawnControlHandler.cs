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
	public class SpawnControlHandler1 : SSCCommandHandler
	{
		public override string PermissionName => "sm";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 2)
			{
				var val = reader.ReadInt32();
				var player = Main.player[playerNumber];
				var splayer = player.GetServerPlayer();
				var spawnrate = typeof(NPC).GetField("defaultSpawnRate",
						System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
				if (val < 0)
				{
					splayer.SendInfoMessage($"当前刷怪率为：{(int)spawnrate.GetValue(null)}");
				}
				else
				{
					spawnrate.SetValue(null, val);
					var s = $"玩家 {player.name} 设置刷怪间隔为 {val}";
					ServerPlayer.SendInfoMessageToAll(s);
					CommandBoardcast.ConsoleMessage(s);
				}
			}
		}
	}

	public class SpawnControlHandler2 : SSCCommandHandler
	{
		public override string PermissionName => "sm";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 2)
			{
				var val = reader.ReadInt32();
				var player = Main.player[playerNumber];
				var splayer = player.GetServerPlayer();
				var maxspawns = typeof(NPC).GetField("defaultMaxSpawns",
						System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
				if (val < 0)
				{
					splayer.SendInfoMessage($"当前最大刷怪次数为：{(int)maxspawns.GetValue(null)}");
				}
				else
				{
					maxspawns.SetValue(null, val);
					var s = $"玩家 {player.name} 设置最大刷怪次数为 {val}";
					ServerPlayer.SendInfoMessageToAll(s);
					CommandBoardcast.ConsoleMessage(s);
				}
			}
		}
	}

}
