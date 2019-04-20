using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.GUI.UI;
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
	public class PlayerInventoryHandler : SSCCommandHandler
	{
		public override string PermissionName => "see-inventory";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{

			if (Main.netMode == 2)
			{
				CommandBoardcast.ConsoleMessage("受到玩家请求离线玩家背包");
				int plr = reader.ReadInt32();
				var splayer = ServerSideCharacter2.PlayerCollection.Get(plr);
				var sender = Main.player[playerNumber].GetServerPlayer();
				if(splayer == null)
				{
					sender.SendInfoMessage("找不到这个玩家", Color.Red);
					return;
				}
				if (splayer.IsLogin && splayer.PrototypePlayer != null && splayer.PrototypePlayer.active)
				{
					sender.SendInfoMessage("该玩家在线，请使用资料页面查看背包");
					return;
				}
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.GetEquipsOffline);
				var list = splayer.GetInventory();
				p.Write(splayer.Name);
				p.Write((byte)list.Count);
				for (int i = 0; i < list.Count; i++)
				{
					p.Write((short)list[i].netID);
					p.Write((short)list[i].stack);
					p.Write((byte)list[i].prefix);
					ItemIO.SendModData(list[i], p);
				}
				p.Send();
			}
			else
			{
				var name = reader.ReadString();
				int num = reader.ReadByte();
				List<Item> list = new List<Item>();
				for(int i = 0; i < num; i++)
				{
					int type = reader.ReadInt16();
					int stack = reader.ReadInt16();
					int prefix = reader.ReadByte();
					Item item = new Item();
					item.netDefaults(type);
					item.stack = stack;
					item.Prefix(prefix);
					ItemIO.ReceiveModData(item, reader);
					list.Add(item);
				}
				lock (PlayerInventoryState2.Instance)
				{
					ServerSideCharacter2.GuiManager.OpenInventory2();
					PlayerInventoryState2.Instance.GetInventory(name, list);
				}
			}
		}
	}

}
