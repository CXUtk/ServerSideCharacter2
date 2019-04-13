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
	public class ClearHandler : SSCCommandHandler
	{
		public override string PermissionName => "clear";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			try
			{
				// 服务器端
				if (Main.netMode == 2)
				{
					var type = reader.ReadByte();
					var p = Main.player[playerNumber];
					var player = p.GetServerPlayer();
					if (type == 0)
					{
						int cleared = 0;
						for (int i = 0; i < Main.npc.Length; i++)
						{
							if (Main.npc[i].active && !Main.npc[i].townNPC)
							{
								Main.npc[i].active = false;
								Main.npc[i].type = 0;
								NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i);
								cleared++;
							}
						}
						player.SendInfoMessage($"成功清除了 {cleared} 个NPC", Color.Green);
					}
					else if(type == 1)
					{
						int cleared = 0;
						for (int i = 0; i < Main.item.Length; i++)
						{
							if (Main.item[i].active)
							{
								Main.item[i].active = false;
								NetMessage.SendData(MessageID.SyncItem, -1, -1, null, i);
								cleared++;
							}
						}
						player.SendInfoMessage($"成功清除了 {cleared} 个物品", Color.Green);
					}
					else if(type == 2)
					{
						player.SendErrorInfo("这个功能没有实现");
						return;
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
