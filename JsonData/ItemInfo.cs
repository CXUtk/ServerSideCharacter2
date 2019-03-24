using ServerSideCharacter2.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.JsonData
{
	public class ItemInfo
	{
		public bool IsMod { get; set; }
		public int ID { get; set; }
		public string FullName { get; set; }
		public int Stack { get; set; }
		public byte Prefix { get; set; }
		public bool Favorite { get; set; }

		public ItemInfo()
		{
			IsMod = false;
			ID = 0;
		}

		public static ItemInfo CreateInfo(Item item)
		{
			ItemInfo info = new ItemInfo();
			if(item.type > Main.maxItemTypes || item.modItem != null)
			{
				info.IsMod = true;
				info.FullName = item.modItem.GetType().FullName;
			}
			else
			{
				info.ID = item.type;
			}

			info.Prefix = item.prefix;
			info.Stack = item.stack;
			info.Favorite = item.favorited;
			return info;
		}

		public static ItemInfo CreateInfo(int id)
		{
			Item item = new Item();
			item.SetDefaults(id);
			return CreateInfo(item);
		}

		public Item ToItem()
		{
			Item item = new Item();
			if (IsMod)
			{
				string modName = FullName.Substring(0, FullName.IndexOf('.'));
				string itemName = FullName.Substring(FullName.LastIndexOf('.') + 1);
				if (ModLoader.GetLoadedMods().Contains(modName))
				{
					int type = ModLoader.LoadedMods.First(m => m.Name == modName).ItemType(itemName);
					item.netDefaults(type);
				}
				else
				{
					item.netDefaults(ServerSideCharacter2.Instance.ItemType<FailedItem>());
					((FailedItem)item.modItem).SetUp(FullName);
					// MOD物品数据会丢失
				}
			}
			else
			{
				item.netDefaults(ID);
			}

			item.prefix = Prefix;
			item.stack = Stack;
			item.favorited = Favorite;
			return item;
		}
	}
}
