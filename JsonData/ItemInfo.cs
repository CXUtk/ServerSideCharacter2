using ServerSideCharacter2.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.IO;

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
		public TagCompound data { get; set; }

		public ItemInfo()
		{
			IsMod = false;
			ID = 0;
		}

		public void FromItem(Item item)
		{
			if(item.type > Main.maxItemTypes || item.modItem != null)
			{
				this.IsMod = true;
				this.FullName = item.modItem.GetType().FullName;
			}
			else
			{
				this.ID = item.type;
			}

			this.Prefix = item.prefix;
			this.Stack = item.stack;
			this.Favorite = item.favorited;

		}

		public static ItemInfo CreateInfo(int id)
		{
			var item = new Item();
			item.SetDefaults(id);
			ItemInfo info = new ItemInfo();
			info.FromItem(item);
			return info;
		}

		public Item ToItem()
		{
			var item = new Item();
			if (IsMod)
			{
				var modName = FullName.Substring(0, FullName.IndexOf('.'));
				var itemName = FullName.Substring(FullName.LastIndexOf('.') + 1);
				if (ModLoader.GetLoadedMods().Contains(modName))
				{
					var type = ModLoader.LoadedMods.First(m => m.Name == modName).ItemType(itemName);
					item.netDefaults(type);
				}
				else
				{
					var tag = new TagCompound()
					{
						{"mod", modName},
						{"name", itemName }
					};
					item.netDefaults(ModLoader.GetMod("ModLoader").ItemType("MysteryItem"));
					var msitem = (MysteryItem)item.modItem;
					var setup = typeof(MysteryItem).GetMethod("Setup", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
					setup.Invoke(msitem, new TagCompound[] { tag });
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
