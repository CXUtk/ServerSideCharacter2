using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Matches
{
	public class MatchingSystem
	{
		public Dictionary<string, Match> Matches;

		public MatchingSystem()
		{
			Matches = new Dictionary<string, Match>();
		}

		public void AppendMatch(Match match)
		{
			if (!HasMatch(match.Name))
			{
				Matches.Add(match.Name, match);
			}
		}

		public bool HasMatch(string name)
		{
			return Matches.ContainsKey(name);
		}

		public bool MatchRunning(string name)
		{
			return Matches[name].IsActive;
		}

		public void Run()
		{
			foreach (var pair in Matches)
			{
				var match = pair.Value;
				if (match.IsActive)
				{
					match.Update();
					//foreach (var player in Main.player)
					//{
					//	if (!player.active) continue;
					//	CommandBoardcast.ConsoleMessage($"玩家 {player.name} 的位置为 {player.position}");
					//}
				}
			}
		}

		public void StartMatch(string name)
		{
			Matches[name].Activate();
		}

		public MatchInfo GetMatchInfo()
		{
			MatchInfo matchInfo = new MatchInfo();
			foreach(var match in Matches.ToList())
			{
				matchInfo.Matches.Add(match.Value.GetSimplified());
			}
			return matchInfo;
		}

		public void MatchPlayer(string name, ServerPlayer player)
		{ 
			var match = Matches[name];
			match.MatchNewPlayer(player);
		}
	}
}
