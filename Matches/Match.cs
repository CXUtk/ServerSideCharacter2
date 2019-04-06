using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerSideCharacter2.Matches
{
	public abstract class Match
	{
		public string Name { get; set; }
		public List<ServerPlayer> MatchedPlayers { get; set; }
		public abstract int MaxMatchingTime { get; }
		public abstract int MaxPlayers { get; }

		public bool IsActive { get; private set; }
		public bool IsMatched { get; protected set; }
		public bool GameStarted { get; protected set; }

		private int innerCounter;

		public Match(string name)
		{
			Name = name;
			MatchedPlayers = new List<ServerPlayer>();
			IsActive = false;
			IsMatched = false;
			GameStarted = false;
			innerCounter = MaxMatchingTime;
		}

		public void Activate() { IsActive = true; innerCounter = MaxMatchingTime; OnActive(); }
		public void Deactivate()
		{
			IsActive = false; IsMatched = false; GameStarted = false; innerCounter = 0;
			MessageSender.SendMatchesData(-1);
			OnDeactive();
		}

		protected abstract void OnActive();
		protected abstract void OnDeactive();

		public abstract void OnMatched();

		public virtual void Update()
		{
			if (!GameStarted)
			{
				if (innerCounter > 0)
				{
					innerCounter--;
				}
				else
				{
					OnMatched();
					innerCounter = MaxMatchingTime;
				}
			}
		}

		public SimplifiedMatchInfo GetSimplified()
		{
			SimplifiedMatchInfo info = new SimplifiedMatchInfo
			{
				Name = Name,
				MaxPlayers = MaxPlayers,
				MatchedPlayers = MatchedPlayers.Count,
				IsMatching = IsActive,
				IsGameStarted = GameStarted,
				TimeRem = innerCounter
			};
			return info;
		}
	}
}
