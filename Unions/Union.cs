using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Unions
{
	public class Union : IName
	{

		private const int EXP_BASE = 200;
		public int Level { get; set; }

		[JsonIgnore]
		public long EXPtoNextLevel => EXP_BASE * (long)Math.Exp(Level);
		public long CurrentEXP { get; set; }
		public long Wealth { get; set; }
		public string Name { get; set; }
		public HashSet<string> Members { get; set; }
		public ServerPlayer Owner { get; set; }


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
				OwnerName = Owner.Name,
				Wealth = Wealth
			};
			return info;
		}

		public void IncreaseEXP(int amount)
		{
			CurrentEXP += amount;
			// TODO: 联机同步
		}

		public void AddMember(ServerPlayer player)
		{
			Members.Add(name);
			player.SetUnion(this.Name);
			// TODO: 联机同步
		}

		public void RemoveMember(string name)
		{
			Members.RemoveWhere((p) => p.Name == name);
			// TODO: 联机同步
		}
	}
}