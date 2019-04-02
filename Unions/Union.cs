using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Unions
{
	public class Union : IName
	{
		public int Level { get; set; }
		private const int EXP_BASE = 200;
		public long EXPtoNextLevel => EXP_BASE * (long)Math.Exp(Level);
		public long CurrentEXP { get; set; }
		public string Name { get; set; }
		public HashSet<string> members { get; set; }

		public Union(string name)
		{
			Name = name;
			CurrentEXP = 0;
			Level = 1;
		} 

		public void IncreaseEXP(int amount)
		{
			CurrentEXP += amount;
			// TODO: 联机同步
		}

		public void AddMember(ServerPlayer player)
		{
			members.Add(player.Name);
			player.SetUnion(this.Name);
			// TODO: 联机同步
		}

		public void RemoveMember(string name)
		{
			members.Remove(name);
			// TODO: 联机同步
		}
	}
}