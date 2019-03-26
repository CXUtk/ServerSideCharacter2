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
	public class ItemHandler : SSCCommandHandler
	{
		public override string PermissionName => "item";


		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			try
			{
				// 服务器端
				if (Main.netMode == 2)
				{
					int type = reader.ReadInt32();
					Player p = Main.player[playerNumber];
					var player = p.GetServerPlayer();
					Item item = new Item();
					item.netDefaults(type);
					var id = Item.NewItem(p.position, Vector2.Zero, type, item.maxStack, true, 0, true);
					string s = string.Format("你得到了 {0} 个 {1}", item.maxStack, Lang.GetItemNameValue(type));
					player.SendInfoMessage(s);
					CommandBoardcast.ConsoleMessage(s);
				}
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}
	}

}
