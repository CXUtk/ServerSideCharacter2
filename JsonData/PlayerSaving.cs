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
		public int hideVisual;
		public Chest bank = new Chest(true);

		public void SaveHideVisual(Player player)
		{
			int target = 0;
			for (int i = 0; i < 10; i++)
			{
				if (player.hideVisual[i])
				{
					target |= 1;
				}
				target <<= 1;
			}
			hideVisual = target;
		}

		public void SetHideVisual(Player player)
		{
			int target = hideVisual;
			target >>= 1;
			for (int i = 9; i >= 0; i--)
			{
				player.hideVisual[i] = (target & 1) > 0 ? true : false;
				target >>= 1;
			}
		}

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
