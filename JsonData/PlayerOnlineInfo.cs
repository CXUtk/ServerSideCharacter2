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
		public string Name { get; set; }

		public bool IsLogin { get; set; }

		public string GroupName { get; set; }

		public string ChatPrefix { get; set; }

		public Color ChatColor { get; set; }

		public int PlayerInnerID { get; set; }

		public int PlayerID { get; set; }

		public bool IsFriend { get; set; }
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
