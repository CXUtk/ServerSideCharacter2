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
		public string Name;

		public int ID;

		public string Password;

		public bool HasPassword;

		public int LifeMax;

		public int StatLife;

		public int ManaMax;

		public int StatMana;

		public HashSet<string> Friends = new HashSet<string>();

		public string Group;

		public int EloRank;

		public int Rank;

		public int KillCount;

		public DateTime RegisteredTime;

		public ItemInfo[] inventory = new ItemInfo[Main.maxInventory + 1];

		public ItemInfo[] armor = new ItemInfo[20];

		public ItemInfo[] dye = new ItemInfo[10];

		public ItemInfo[] miscEquips = new ItemInfo[5];

		public ItemInfo[] miscDye = new ItemInfo[5];

		public ItemInfo[] bank = new ItemInfo[Chest.maxItems];

		public int hideVisual = 0;

		public long GuCoin = 0;

		public Dictionary<string, object> ExtraInfos = new Dictionary<string, object>();

		

		//public ItemInfo[] bank2 = new ItemInfo[Chest.maxItems];

		//public ItemInfo[] bank3 = new ItemInfo[Chest.maxItems];

		public PlayerInfo()
		{
			for (var i = 0; i < inventory.Length; i++)
			{
				inventory[i] = ItemInfo.Create();
			}
			for (var i = 0; i < armor.Length; i++)
			{
				armor[i] = ItemInfo.Create();
			}
			for (var i = 0; i < dye.Length; i++)
			{
				dye[i] = ItemInfo.Create();
			}
			for (var i = 0; i < miscEquips.Length; i++)
			{
				miscEquips[i] = ItemInfo.Create();
			}
			for (var i = 0; i < miscDye.Length; i++)
			{
				miscDye[i] = ItemInfo.Create();
			}
			for (var i = 0; i < bank.Length; i++)
			{
				bank[i] = ItemInfo.Create();
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
