using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerSideCharacter2.Groups;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.JsonData
{
	public struct SimplifiedMatchInfo
	{
		public string Name;
		public int MatchedPlayers;
		public int MaxPlayers;
		public int TimeRem;
		public bool IsMatching;
		public bool IsGameStarted;
		public int ReminChance;
	}

	public class MatchInfo
	{
		public List<SimplifiedMatchInfo> Matches;
		public MatchInfo()
		{
			Matches = new List<SimplifiedMatchInfo>();
		}
	}
}
