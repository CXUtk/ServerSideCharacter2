using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerSideCharacter2.Groups;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.JsonData
{
	public struct SimplifiedUnionInfo
	{
		public string Name;
		public int Level;
		public long Wealth;
		public string OwnerName;
		public int NumMember;
	}

	public struct ComplexUnionInfo
	{
		public string Name;
		public int Level;
		public long CurrentEXP;
		public long EXPToNext;
		public long Wealth;
		public SimplifiedPlayerInfo Owner;
		public HashSet<SimplifiedPlayerInfo> Members;
		public HashSet<SimplifiedPlayerInfo> Requests;
	}

	public class UnionInfo
	{
		public List<SimplifiedUnionInfo> Unions;
		public UnionInfo()
		{
			Unions = new List<SimplifiedUnionInfo>();
		}
	}
}
