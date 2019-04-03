using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			foreach(var pair in Matches)
			{
				var match = pair.Value;
				if (match.IsActive)
				{
					match.Update();
				}
			}
		}

		public void StartMatch(string name)
		{
			Matches[name].Activate();
		}

		public void MatchPlayer(string name, ServerPlayer player)
		{
			var match = Matches[name];
			match.MatchedPlayers.Add(player);
			player.InMatch = true;
			if(match.MatchedPlayers.Count == match.MaxPlayers)
			{
				match.OnMatched();
			}
		}
	}
}
