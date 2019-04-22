using ServerSideCharacter2.Items;
using ServerSideCharacter2.Utils;
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
		public Dictionary<string, object> modData { get; set; }
		public int Stack { get; set; }
		public byte Prefix { get; set; }
		public bool Favorite { get; set; }

		public ItemInfo()
		{
			IsMod = false;
			ID = 0;
			Stack = 0;
			modData = new Dictionary<string, object>();
		}

		private void ToDict(TagCompound tag)
		{
			modData.Clear();
			foreach (var pair in tag)
			{
				if(pair.Key == "stack")
				{
					continue;
				}
				modData.Add(pair.Key, pair.Value);
			}
		}

		public void FromItem(Item item)
		{
			
			if(item.type > Main.maxItemTypes || item.modItem != null)
			{
				this.IsMod = true;
				this.ID = 0;
				ToDict(ItemIO.Save(item));
			}
			else
			{
				this.IsMod = false;
				this.modData = new Dictionary<string, object>();
				this.ID = item.type;
			}
			this.Prefix = item.prefix;
			this.Stack = item.stack;
			this.Favorite = item.favorited;
			if (item.stack > item.maxStack)
			{
				item.stack = item.maxStack;
			}
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
			Item item = new Item();
			if (IsMod)
			{
				TagCompound tag = new TagCompound();
				foreach (var pair in modData)
				{
					tag.Add(pair.Key, pair.Value);
				}
				item = ItemIO.Load(tag);
			}
			else
			{
				item.netDefaults(ID);
			}
			item.Prefix(Prefix);
			item.stack = Stack;
			item.favorited = Favorite;
			if (item.stack < 0) item.stack = 0;
			else if (item.stack > item.maxStack)
			{
				item.stack = item.maxStack;
			}
			return item;
		}
	}
}
