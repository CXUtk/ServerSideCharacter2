using Microsoft.Xna.Framework;
using ServerSideCharacter2.JsonData;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using System;
using ServerSideCharacter2.Utils;
using Newtonsoft.Json;
using Terraria.ModLoader;

namespace ServerSideCharacter2
{
	public class ServerPlayer
	{
		private static int _NextID = 0;

		[JsonRequired]
		private PlayerInfo _info;

		public Item[] inventory = new Item[Main.maxInventory + 1];
		public Item[] armor = new Item[20];
		public Item[] dye = new Item[10];
		public Item[] miscEquips = new Item[5];
		public Item[] miscDye = new Item[5];

		public Chest bank = new Chest(true);
		public Chest bank2 = new Chest(true);
		public Chest bank3 = new Chest(true);

		[JsonIgnore]
		public Player PrototypePlayer { get; set; }

		public string GetSerializedString()
		{
			return JsonConvert.SerializeObject(_info);
		}

		public void SetPlayerInfo(PlayerInfo info)
		{
			_info = info;
		}

		internal PlayerInfo GetPlayerInfo()
		{
			return _info;
		}

		public string Name
		{
			get
			{
				return _info.Name;
			}
		}


		public bool IsLogin { get; set; }

		public bool HasPassword
		{
			get { return _info.HasPassword; }
			set { _info.HasPassword = value; }
		}

		public int StatLife
		{
			get
			{
				return _info.StatLife;
			}
			set
			{
				_info.StatLife = value;
			}
		}
		public int LifeMax
		{
			get
			{
				return _info.LifeMax;
			}
			set
			{
				_info.LifeMax = value;
			}
		}

		public int StatMana
		{
			get
			{
				return _info.StatMana;
			}
			set
			{
				_info.StatMana = value;
			}
		}

		public int ManaMax
		{
			get
			{
				return _info.ManaMax;
			}
			set
			{
				_info.ManaMax = value;
			}
		}

		private void SetupPlayer()
		{
			for (int i = 0; i < inventory.Length; i++)
			{
				inventory[i] = new Item();
			}
			for (int i = 0; i < armor.Length; i++)
			{
				armor[i] = new Item();
			}
			for (int i = 0; i < dye.Length; i++)
			{
				dye[i] = new Item();
			}
			for (int i = 0; i < miscEquips.Length; i++)
			{
				miscEquips[i] = new Item();
			}
			for (int i = 0; i < miscDye.Length; i++)
			{
				miscDye[i] = new Item();
			}
			for (int i = 0; i < bank.item.Length; i++)
			{
				bank.item[i] = new Item();
			}
			for (int i = 0; i < bank2.item.Length; i++)
			{
				bank2.item[i] = new Item();
			}
			for (int i = 0; i < bank3.item.Length; i++)
			{
				bank3.item[i] = new Item();
			}

		}

		public ServerPlayer()
		{
			SetupPlayer();
		}


		public ServerPlayer(Player player)
		{
			SetupPlayer();
			PrototypePlayer = player;
		}

		public void ApplyLockBuffs(int time = 180)
		{
			PrototypePlayer.AddBuff(ServerSideCharacter2.Instance.BuffType("Locked"), time * 2, false);
			PrototypePlayer.AddBuff(BuffID.Frozen, time, false);
			NetMessage.SendData(MessageID.AddPlayerBuff, PrototypePlayer.whoAmI, -1,
				NetworkText.Empty, PrototypePlayer.whoAmI,
				ServerSideCharacter2.Instance.BuffType("Locked"), time * 2, 0f, 0, 0, 0);
			NetMessage.SendData(MessageID.AddPlayerBuff, PrototypePlayer.whoAmI, -1,
				NetworkText.Empty, PrototypePlayer.whoAmI,
				BuffID.Frozen, time, 0f, 0, 0, 0);
		}

		public static ServerPlayer CreateNewPlayer(Player p)
		{
			ServerPlayer instance = new ServerPlayer(p);
			PlayerHooks.SetStartInventory(p);
			PlayerInfo player = new PlayerInfo
			{
				Name = p.name,
				ID = GetNextID(),
				HasPassword = false,
				TPProtect = true,
				Password = "",
				LifeMax = 100,
				StatLife = 100,
				ManaMax = 20,
				StatMana = 20
			};
			int i = 0;
			foreach (var item in ServerSideCharacter2.Config.startUpInventory)
			{
				player.inventory[i++] = item;
			}
			instance._info = player;
			instance.SyncPlayerFromInfo();
			return instance;
		}

		private static int GetNextID()
		{
			return _NextID++;
		}

		public void SendErrorInfo(string msg)
		{
			NetMessage.SendChatMessageToClient(NetworkText.FromLiteral(msg), 
				new Color(255, 20, 20, 0), PrototypePlayer.whoAmI);
		}

		public void SyncPlayerFromInfo()
		{
			ServerUtils.InfoToItem(_info.inventory, inventory);
			ServerUtils.InfoToItem(_info.armor, armor);
			ServerUtils.InfoToItem(_info.dye, dye);
			ServerUtils.InfoToItem(_info.miscEquips, miscEquips);
			ServerUtils.InfoToItem(_info.miscDye, miscDye);
			ServerUtils.InfoToItem(_info.bank, bank.item);
			ServerUtils.InfoToItem(_info.bank2, bank2.item);
			ServerUtils.InfoToItem(_info.bank3, bank3.item);
		}

		public void SyncPlayerToInfo()
		{
			if (PrototypePlayer.active)
			{
				LifeMax = PrototypePlayer.statLifeMax;
				StatLife = PrototypePlayer.statLife;
				StatMana = PrototypePlayer.statMana;
				ManaMax = PrototypePlayer.statManaMax;
				inventory = PrototypePlayer.inventory;
				armor = PrototypePlayer.armor;
				dye = PrototypePlayer.dye;
				miscEquips = PrototypePlayer.miscEquips;
				miscDye = PrototypePlayer.miscDyes;
				bank = PrototypePlayer.bank;
				bank2 = PrototypePlayer.bank2;
				bank3 = PrototypePlayer.bank3;

				ServerUtils.CopyToItemData(inventory, _info.inventory);
				ServerUtils.CopyToItemData(armor, _info.armor);
				ServerUtils.CopyToItemData(dye, _info.dye);
				ServerUtils.CopyToItemData(miscEquips, _info.miscEquips);
				ServerUtils.CopyToItemData(miscDye, _info.miscDye);
				ServerUtils.CopyToItemData(bank.item, _info.bank);
				ServerUtils.CopyToItemData(bank2.item, _info.bank2);
				ServerUtils.CopyToItemData(bank3.item, _info.bank3);
			}
			else
			{
				throw new ArgumentException("Unable to syncronize player data, the player does not exist.");
			}
		}

		public void ApplyToPlayer()
		{
			if (PrototypePlayer.active)
			{
				PrototypePlayer.statLifeMax = LifeMax;
				PrototypePlayer.statLife = StatLife;
				PrototypePlayer.statMana = StatMana;
				PrototypePlayer.statManaMax = ManaMax;

				inventory.CopyTo(PrototypePlayer.inventory, 0);
				armor.CopyTo(PrototypePlayer.armor, 0);
				miscEquips.CopyTo(PrototypePlayer.miscEquips, 0);
				dye.CopyTo(PrototypePlayer.dye, 0);
				miscDye.CopyTo(PrototypePlayer.miscDyes, 0);
				bank.item.CopyTo(PrototypePlayer.bank.item, 0);
				bank2.item.CopyTo(PrototypePlayer.bank2.item, 0);
				bank3.item.CopyTo(PrototypePlayer.bank3.item, 0);
			}
			else
			{
				throw new ArgumentException("Unable to syncronize player data, the player does not exist.");
			}
		}

		public void ClearAllBuffs()
		{
			for(int i = 0; i < PrototypePlayer.buffType.Length; i++)
			{
				PrototypePlayer.DelBuff(i);
			}
		}

		public void Lock()
		{
			PrototypePlayer.AddBuff(ServerSideCharacter2.Instance.BuffType("Locked"), 18000, false);
			PrototypePlayer.AddBuff(BuffID.Frozen, 18000, false);
			NetMessage.SendData(MessageID.AddPlayerBuff, PrototypePlayer.whoAmI, -1,
				NetworkText.Empty, PrototypePlayer.whoAmI,
				ServerSideCharacter2.Instance.BuffType("Locked"), 18000, 0f, 0, 0, 0);
			NetMessage.SendData(MessageID.AddPlayerBuff, PrototypePlayer.whoAmI, -1,
				NetworkText.Empty, PrototypePlayer.whoAmI,
				BuffID.Frozen, 18000, 0f, 0, 0, 0);
		}
	}
}