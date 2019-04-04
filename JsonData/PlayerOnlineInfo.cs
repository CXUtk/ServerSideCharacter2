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

		public string ChatPrefix;

		public Color ChatColor;

		public int GUID;

		public int PlayerID;

		public bool IsFriend;
		
		public int Rank;

		public int KillCount;

		public DateTime RegistedTime;

        public string QQNumber;
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
