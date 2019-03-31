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
	public class SummonHandler : SSCCommandHandler
	{
		public override string PermissionName => "sm";

		private bool TilePlacementValid(int tileX, int tileY)
		{
			return tileX >= 0 && tileX < Main.maxTilesX && tileY >= 0 && tileY < Main.maxTilesY;
		}

		private bool TileSolid(int tileX, int tileY)
		{
			return TilePlacementValid(tileX, tileY) && Main.tile[tileX, tileY] != null &&
				Main.tile[tileX, tileY].active() && Main.tileSolid[Main.tile[tileX, tileY].type] &&
				!Main.tile[tileX, tileY].inActive() && !Main.tile[tileX, tileY].halfBrick() &&
				Main.tile[tileX, tileY].slope() == 0 && Main.tile[tileX, tileY].type != TileID.Bubble;
		}

		private void GetRandomClearTileWithInRange(int startTileX, int startTileY, int tileXRange, int tileYRange,
				out int tileX, out int tileY)
		{
			var j = 0;
			do
			{
				// 尝试100次以后停下
				if (j == 100)
				{
					tileX = startTileX;
					tileY = startTileY;
					break;
				}
				tileX = startTileX + Main.rand.Next(tileXRange * -1, tileXRange);
				tileY = startTileY + Main.rand.Next(tileYRange * -1, tileYRange);
				j++;
			} while (TilePlacementValid(tileX, tileY) && TileSolid(tileX, tileY));
		}

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			try
			{
				// 服务器端
				if (Main.netMode == 2)
				{
					var type = reader.ReadInt32();
					var number = reader.ReadInt32();
					var p = Main.player[playerNumber];
					var player = p.GetServerPlayer();
					if (number > 200) number = 200;
					if (type >= 1 && type < Main.npcTexture.Length && type != 113)
					{
						for (var i = 0; i < number; i++)
						{
							int spawnTileX;
							int spawnTileY;
							GetRandomClearTileWithInRange((int)(p.Center.X) / 16, (int)(p.Center.Y) / 16, 50, 50, out spawnTileX,
																		 out spawnTileY);
							var npcid = NPC.NewNPC(spawnTileX * 16, spawnTileY * 16, type, 0);
							// This is for special slimes
							Main.npc[npcid].SetDefaults(type);
						}
						var s = string.Format("{0} 召唤了 {1} 个 {2}",
								player.Name, number, Lang.GetNPCNameValue(type));
						ServerPlayer.SendInfoMessageToAll(s);
					}
					else
					{
						player.SendErrorInfo("不存在该NPC");
					}
				}
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}
	}

}
