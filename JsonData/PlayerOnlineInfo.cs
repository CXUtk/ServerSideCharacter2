using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerSideCharacter2.Groups;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.JsonData
{
	[JsonObject]
	public struct SimplifiedPlayerInfo
	{
		public string Name;

		public bool IsLogin;

		public string GroupName;

		public string UnionName;

		public string ChatPrefix;

        public string CustomChatPrefix;

		public Color ChatColor;

		public int GUID;

		public int PlayerID;

		public bool IsFriend;
		
		public int Rank;

		public int KillCount;

		public DateTime RegistedTime;

        public string QQNumber;

		public string CurrentMatch;

		public int VIPLevel;

		public long GuCoin;

		public Dictionary<string, object> ExtraData;

		public static int CompareB(SimplifiedPlayerInfo a, SimplifiedPlayerInfo b)
		{
			return a.Rank.CompareTo(b.Rank);
		}

		public static int CompareA(string a, string b)
		{
			var aplayer = ServerSideCharacter2.PlayerCollection.Get(a);
			var bplayer = ServerSideCharacter2.PlayerCollection.Get(b);
			return aplayer.Rank.CompareTo(bplayer.Rank);
		}
	}

	[JsonObject]
	public class PlayerOnlineInfo
	{
		public List<SimplifiedPlayerInfo> Player;
		public PlayerOnlineInfo()
		{
			Player = new List<SimplifiedPlayerInfo>();
		}
	}
}
