using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Unions
{
	public class Union : IName
	{
		private const int EXP_BASE = 200;
		private const int MAX_CANDIDATES = 10;
		private const int MAX_LEVEL = 15;
		public int Level { get; set; }

		[JsonIgnore]
		public long EXPForNextLevel => EXP_BASE * (long)Math.Exp(Level);


		public long CurrentEXP { get; set; }
		public long Wealth { get; set; }
		public string Name { get; set; }
		public HashSet<string> Members { get; set; }
		public string Owner { get; set; }
		public HashSet<string> Candidates { get; set; }
		public string RegionName { get; set; }
		public Dictionary<string, long> DonationTable { get; set; }


		public static int GetMaxMembers(int level)
		{
			if(level >= 8)
			{
				return 15;
			}
			else if(level >= 5)
			{
				return 10;
			}
			else
			{
				return 7;
			}
		}

		public Union(string name)
		{
			Name = name;
			CurrentEXP = 0;
			Level = 1;
			Wealth = 0;
			Members = new HashSet<string>();
			Candidates = new HashSet<string>();
			RegionName = "";
			DonationTable = new Dictionary<string, long>();
		} 

		public SimplifiedUnionInfo GetSimplified()
		{
			var info = new SimplifiedUnionInfo
			{
				Name = Name,
				Level = Level,
				NumMember = Members.Count,
				OwnerName = Owner,
				Wealth = Wealth
			};
			return info;
		}

		public ComplexUnionInfo GetVerbose(int plr)
		{
			CheckCandidates();
			CheckDonationTable();
			var info = new ComplexUnionInfo
			{
				Name = Name,
				Level = Level,
				CurrentEXP = CurrentEXP,
				EXPToNext = EXPForNextLevel,
				Wealth = Wealth
			};
			info.Members = new HashSet<SimplifiedPlayerInfo>();
			foreach(var member in Members)
			{
				// 成员的简化信息
				var player = ServerSideCharacter2.PlayerCollection.Get(member);
				if (player == null) continue;
				var simplinfo = player.GetSimplified(plr);
				info.Members.Add(simplinfo);
				if(member == Owner)
				{
					info.Owner = simplinfo;
				}
			}
			info.Requests = new HashSet<SimplifiedPlayerInfo>();
			foreach (var candidate in Candidates)
			{
				var player = ServerSideCharacter2.PlayerCollection.Get(candidate);
				if (player == null) continue;
				info.Requests.Add(player.GetSimplified(plr));
			}
			info.Donation = new Dictionary<string, long>();
			foreach (var pair in DonationTable)
			{
				info.Donation.Add(pair.Key, pair.Value);
			}
			return info;
		}

		public void CheckCandidates()
		{
			lock (this)
			{
				var list = new List<string>();
				foreach (var candidate in Candidates)
				{
					var player = ServerSideCharacter2.PlayerCollection.Get(candidate);
					if (player != null && player.Union != null) { list.Add(candidate); }
				}
				foreach (var del in list)
				{
					Candidates.Remove(del);
				}
			}
		}

		public void CheckDonationTable()
		{
			lock (this)
			{
				var list = new List<string>();
				foreach (var member in Members)
				{
					if (!DonationTable.ContainsKey(member))
					{
						DonationTable.Add(member, 0);
					}
				}
			}
		}

		public void AddCandidate(ServerPlayer player)
		{
			lock (this)
			{
				if (player.Union != null)
				{
					player.SendMessageBox("您已经有公会了", 180, Color.Yellow);
					return;
				}
				if (Candidates.Contains(player.Name))
				{
					player.SendMessageBox("您已经在公会的申请列表中，请耐心等待审核", 180, Color.Yellow);
					return;
				}
				if (Candidates.Count >= MAX_CANDIDATES)
				{
					player.SendMessageBox("公会申请人数已满，无法申请", 180, Color.OrangeRed);
					return;
				}
				Candidates.Add(player.Name);
				SyncToOwner();
				player.SendMessageBox("申请成功，请等待会长审核", 180, Color.LimeGreen);
			}
		}

		public bool HasCandidate(ServerPlayer player)
		{
			lock (this)
			{
				return Candidates.Contains(player.Name);
			}
		}

		public bool CanAccept()
		{
			return Members.Count < GetMaxMembers(Level);
		}

		public void AcceptCandidate(ServerPlayer player)
		{
			lock (this)
			{
				if (player.Union != null) return;
				if (!Candidates.Contains(player.Name)) return;
				Candidates.Remove(player.Name);
				AddMember(player);
				string s = $"欢迎 {player.Name} 加入公会";
				foreach(var member in Members)
				{
					var splayer = ServerSideCharacter2.PlayerCollection.Get(member);
					splayer?.SendInfoMessage(s);
				}
				CommandBoardcast.ConsoleMessage($"玩家 {player.Name} 加入公会 {Name}");
			}
		}

		public void RejectCandidate(ServerPlayer player)
		{
			lock (this)
			{
				if (player.Union != null) return;
				if (!Candidates.Contains(player.Name)) return;
				Candidates.Remove(player.Name);
				player.SendMessageBox($"很抱歉，公会 {Name} 拒绝了您的申请", 180, Color.Red);
			}
		}

		private void IncreaseEXP(int amount)
		{
			CurrentEXP += amount;
			while(CurrentEXP >= EXPForNextLevel)
			{
				Level++;
				foreach (var member in Members)
				{
					var player = ServerSideCharacter2.PlayerCollection.Get(member);
					if (player.PrototypePlayer != null)
					{
						player.SendInfoMessage($"公会等级成功升级至 {Level} 级");
					}
				}
			}
			SyncToAllMembers();
		}


		private void IncreaseWealth(int amount)
		{
			Wealth += amount;
		}

		public void Donate(ServerPlayer splayer, int amount)
		{
			lock (this)
			{
				IncreaseEXP(amount);
				IncreaseWealth(amount);
				if (!DonationTable.ContainsKey(splayer.Name))
				{
					DonationTable.Add(splayer.Name, amount);
				}
				else
				{
					DonationTable[splayer.Name] += amount;
				}
				foreach (var member in Members)
				{
					var player = ServerSideCharacter2.PlayerCollection.Get(member);
					if (player.PrototypePlayer != null)
					{
						player.SendInfoMessage($"玩家 {splayer.Name} 给公会捐献了 {amount} 财富", Color.LimeGreen);
					}
				}
				SyncToAllMembers();
			}
		}


		public void AddMember(ServerPlayer player)
		{
			lock (this)
			{
				player.Union = this;
				Members.Add(player.Name);
				SyncToAllMembers();
				player.SyncUnionInfo();
			}
		}


		public void SyncToOwner()
		{
			var player = ServerSideCharacter2.PlayerCollection.Get(Owner);
			if (player.PrototypePlayer != null && player.RealPlayer && player.ConnectionAlive)
			{
				MessageSender.SendComplexUnionData(this, player.PrototypePlayer.whoAmI);
			}
		}

		public void SyncToAllMembers()
		{
			foreach (var member in Members)
			{
				var player = ServerSideCharacter2.PlayerCollection.Get(member);
				if (player.PrototypePlayer != null && player.RealPlayer && player.ConnectionAlive) {
					ModPacket p = ServerSideCharacter2.Instance.GetPacket();
					p.Write((int)SSCMessageType.UnionInfoComplex);
					var tmp = JsonConvert.SerializeObject(this.GetVerbose(player.PrototypePlayer.whoAmI), Formatting.None);
					p.Write(tmp);
					p.Send(player.PrototypePlayer.whoAmI);
				}
			}
		}

		public void RemoveMember(ServerPlayer player)
		{
			lock (this)
			{
				if (player == null) return;
				if (player.Name == Owner) return;
				Members.Remove(player.Name);
				player.Union = null;
				SyncToAllMembers();
				player.SyncUnionInfo();
				string s = $"玩家 {player.Name} 退出了公会";
				foreach (var member in Members)
				{
					var splayer = ServerSideCharacter2.PlayerCollection.Get(member);
					splayer?.SendInfoMessage(s);
				}
			}
		}

		public void KickMember(ServerPlayer player)
		{
			lock (this)
			{
				if (player == null) return;
				if (player.Name == Owner) return;
				Members.Remove(player.Name);
				player.Union = null;
				SyncToAllMembers();
				player.SyncUnionInfo();
				string s = $"玩家 {player.Name} 被踢出了公会";
				foreach (var member in Members)
				{
					var splayer = ServerSideCharacter2.PlayerCollection.Get(member);
					splayer?.SendInfoMessage(s);
				}
			}
		}
	}
}