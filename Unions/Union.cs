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
		public int Level { get; set; }

		[JsonIgnore]
		public long EXPtoNextLevel => EXP_BASE * (long)Math.Exp(Level);
		public long CurrentEXP { get; set; }
		public long Wealth { get; set; }
		public string Name { get; set; }
		public HashSet<string> Members { get; set; }
		public string Owner { get; set; }
		public HashSet<string> Candidates { get; set; }


		public static int GetMaxMembers(int level)
		{
			if(level == 8)
			{
				return 20;
			}
			else if(level >= 4)
			{
				return 15;
			}
			else
			{
				return 10;
			}
		}

		public Union(string name)
		{
			Name = name;
			CurrentEXP = 0;
			Level = 1;
			Wealth = 0;
			Members = new HashSet<string>();
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

		public ComplexUnionInfo GetComplex(int plr)
		{
			CheckCandidates();
			var info = new ComplexUnionInfo
			{
				Name = Name,
				Level = Level,
				CurrentEXP = CurrentEXP,
				EXPToNext = EXPtoNextLevel,
				Wealth = Wealth
			};
			info.Members = new HashSet<SimplifiedPlayerInfo>();
			foreach(var member in Members)
			{
				// 成员的简化信息
				var player = ServerSideCharacter2.PlayerCollection.Get(member);
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
				info.Members.Add(player.GetSimplified(plr));
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
					if (player.Union != null) { list.Add(candidate); }
				}
				foreach (var del in list)
				{
					Candidates.Remove(del);
				}
			}
		}

		public void AddCandidate(ServerPlayer player)
		{
			lock (this)
			{
				if (player.Union != null) return;
				if (Candidates.Count >= MAX_CANDIDATES) return;
				Candidates.Add(player.Name);
				SyncToOwner();
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
			return Candidates.Count < MAX_CANDIDATES;
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

		public void IncreaseEXP(int amount)
		{
			lock (this)
			{
				CurrentEXP += amount;
				SyncToAllMembers();
			}
		}

		public void AddMember(ServerPlayer player)
		{
			lock (this)
			{
				Members.Add(player.Name);
				player.SetUnion(this.Name);
				SyncToAllMembers();
			}
		}

		public void RemoveMember(ServerPlayer player)
		{
			throw new NotImplementedException();
		}

		public void SyncToOwner()
		{
			var player = ServerSideCharacter2.PlayerCollection.Get(Owner);
			if (player.PrototypePlayer != null)
			{
				MessageSender.SendComplexUnionData(this, player.PrototypePlayer.whoAmI);
			}
		}

		public void SyncToAllMembers()
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.UnionInfoComplex);
			p.Write(JsonConvert.SerializeObject(GetComplex(), Formatting.None));

			foreach (var member in Members)
			{
				var player = ServerSideCharacter2.PlayerCollection.Get(member);
				if (player.PrototypePlayer != null) {
					p.Send(player.PrototypePlayer.whoAmI);
				}
			}
		}

		public void RemoveMember(string name)
		{
			lock (this)
			{
				if (name == Owner) return;
				Members.Remove(name);
				SyncToAllMembers();
			}
		}
	}
}