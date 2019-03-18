using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.JsonData
{
	internal struct ServerPlayerInfo
	{
		public Dictionary<string, PlayerInfo> Playerdata;
		public int CurrentID;

		internal ServerPlayerInfo(Dictionary<string, PlayerInfo> info, int id)
		{
			Playerdata = info;
			CurrentID = id;
		}
	}
}
