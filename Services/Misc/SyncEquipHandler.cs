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
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace ServerSideCharacter2.Services.Misc
{
	public class SyncEquipHandler : SSCCommandHandler
	{
		public override string PermissionName => "see-inventory";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{

			if (Main.netMode == 2)
			{
				int plr = reader.ReadByte();
				var id = reader.ReadByte();
				var nettype = reader.ReadInt16();
				var prefix = reader.ReadByte();
				var stack = reader.ReadInt16();
				var player = Main.player[plr];
				Item item = new Item();
				item.SetDefaults(nettype);
				item.Prefix(prefix);
				item.stack = stack;
				ItemIO.ReceiveModData(item, reader);
				if (!player.active) return;
				var splayer = player.GetServerPlayer();
				lock (player)
				{
					if (id > 58) return;
					if (stack < 0) stack = 0;
					player.inventory[id] = item;
					NetMessage.SendData(5, -1, -1, null, plr, id, (float)prefix, 0f, 0, 0, 0);
				}
			}
		}
	}

	public class SyncEquipHandler2 : SSCCommandHandler
	{
		public override string PermissionName => "see-inventory";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{

			if (Main.netMode == 2)
			{
				string name = reader.ReadString();
				var id = reader.ReadByte();
				var nettype = reader.ReadInt16();
				var prefix = reader.ReadByte();
				var stack = reader.ReadInt16();
				var player = ServerSideCharacter2.PlayerCollection.Get(name);
				var sender = Main.player[playerNumber].GetServerPlayer();

				Item item = new Item();
				item.SetDefaults(nettype);
				item.Prefix(prefix);
				item.stack = stack;
				ItemIO.ReceiveModData(item, reader);
				if (player == null)
				{
					sender.SendInfoMessage("玩家不存在", Color.Red);
					return;
				}
				lock (player)
				{
					if (stack < 0) stack = 0;
					player.SetInventory(id, item);
				}
			}
		}
	}
}
