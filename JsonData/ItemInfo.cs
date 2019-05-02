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
	public struct ItemInfo
	{
		public bool IsMod;
		public int ID;
		public Dictionary<string, object> modData;
		public int Stack;
		public byte Prefix;
		public bool Favorite;

		public static ItemInfo Create()
		{
			ItemInfo info = new ItemInfo();
			info.modData = new Dictionary<string, object>();
			return info;
		}

		public ItemInfo(bool ismod)
		{
			IsMod = ismod;
			ID = 0;
			modData = new Dictionary<string, object>();
			Stack = 0;
			Prefix = 0;
			Favorite = false;
		}


		private void ToDict(TagCompound tag)
		{
			modData.Clear();
			foreach (var pair in tag)
			{
				if (pair.Key == "stack")
				{
					continue;
				}
				modData.Add(pair.Key, pair.Value);
			}
		}

		public void FromItem(Item item)
		{
			this.modData = new Dictionary<string, object>();
			if (item.type > Main.maxItemTypes || item.modItem != null)
			{
				this.IsMod = true;
				this.ID = 0;
				ToDict(ItemIO.Save(item));
			}
			else
			{
				this.IsMod = false;
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
			ItemInfo info = Create();
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
