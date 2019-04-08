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
		public int OwnerName;
		public int NumMember;
	}

	public class UnionInfo
	{
		public List<SimplifiedMatchInfo> Unions;
		public UnionInfo()
		{
			Unions = new List<SimplifiedMatchInfo>();
		}
	}
}
