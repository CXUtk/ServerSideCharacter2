using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.JsonData
{
	[JsonObject]
	public struct SimplifiedPlayerInfo
	{
		public string Name { get; set; }

		public bool IsLogin { get; set; }

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
