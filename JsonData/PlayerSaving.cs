using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.JsonData
{
	public class PlayerSaving : IName
	{
		public int LifeMax { get; set; }
		public int StatLife { get; set; }
		public int ManaMax { get; set; }
		public int StatMana { get; set; }
		public Item[] inventory = new Item[Main.maxInventory + 1];
		public Item[] armor = new Item[20];
		public Item[] dye = new Item[10];
		public Item[] miscEquips = new Item[5];
		public Item[] miscDye = new Item[5];
		public Chest bank = new Chest(true);

		public PlayerSaving(string name)
		{
			Name = name;
			Reset();
			
		}

		public virtual void Reset()
		{
			LifeMax = 100;
			StatLife = 100;
			ManaMax = 20;
			StatMana = 20;
			for (var i = 0; i < inventory.Length; i++)
			{
				inventory[i] = new Item();
			}
			for (var i = 0; i < armor.Length; i++)
			{
				armor[i] = new Item();
			}
			for (var i = 0; i < dye.Length; i++)
			{
				dye[i] = new Item();
			}
			for (var i = 0; i < miscEquips.Length; i++)
			{
				miscEquips[i] = new Item();
			}
			for (var i = 0; i < miscDye.Length; i++)
			{
				miscDye[i] = new Item();
			}
			for (var i = 0; i < bank.item.Length; i++)
			{
				bank.item[i] = new Item();
			}
		}

		public string Name { get; set; }
	}
}
