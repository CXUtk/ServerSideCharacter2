using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Terraria;

namespace ServerSideCharacter2.JsonData
{
	[JsonObject]
	public class PlayerInfo
	{
		public string Name { get; set; }

		public int ID { get; set; }

		public string Password { get; set; }

		public bool HasPassword { get; set; }

		public bool TPProtect { get; set; }

		public int LifeMax { get; set; }

		public int StatLife { get; set; }

		public int ManaMax { get; set; }

		public int StatMana { get; set; }

		public HashSet<string> Friends = new HashSet<string>();

		public string Group { get; set; }

		public ItemInfo[] inventory = new ItemInfo[Main.maxInventory + 1];

		public ItemInfo[] armor = new ItemInfo[20];

		public ItemInfo[] dye = new ItemInfo[10];

		public ItemInfo[] miscEquips = new ItemInfo[5];

		public ItemInfo[] miscDye = new ItemInfo[5];

		public ItemInfo[] bank = new ItemInfo[Chest.maxItems];

		//public ItemInfo[] bank2 = new ItemInfo[Chest.maxItems];

		//public ItemInfo[] bank3 = new ItemInfo[Chest.maxItems];

		public PlayerInfo()
		{
			for (int i = 0; i < inventory.Length; i++)
			{
				inventory[i] = new ItemInfo();
			}
			for (int i = 0; i < armor.Length; i++)
			{
				armor[i] = new ItemInfo();
			}
			for (int i = 0; i < dye.Length; i++)
			{
				dye[i] = new ItemInfo();
			}
			for (int i = 0; i < miscEquips.Length; i++)
			{
				miscEquips[i] = new ItemInfo();
			}
			for (int i = 0; i < miscDye.Length; i++)
			{
				miscDye[i] = new ItemInfo();
			}
			for (int i = 0; i < bank.Length; i++)
			{
				bank[i] = new ItemInfo();
			}
			//for (int i = 0; i < bank2.Length; i++)
			//{
			//	bank2[i] = new ItemInfo();
			//}
			//for (int i = 0; i < bank3.Length; i++)
			//{
			//	bank3[i] = new ItemInfo();
			//}

		}
	}
}
