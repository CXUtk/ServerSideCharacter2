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
		public long Wealth;
		public string OwnerName;
		public HashSet<string> Members;
		public string Owner;
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
