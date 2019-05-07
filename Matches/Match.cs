using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Terraria;

namespace ServerSideCharacter2.Matches
{
	public abstract class Match
	{
		public string Name { get; set; }
		public List<ServerPlayer> MatchedPlayers { get; set; }
		public abstract int MaxMatchingTime { get; }
		public abstract int MaxPlayers { get; }
		public abstract int MaxChancePerDay { get; }

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

		public bool Contains(Player player)
		{
			foreach(var pla in MatchedPlayers)
			{
				if(pla.Name == player.name)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool IsPlaying(Player player)
		{
			return Contains(player);
		}

		public void Activate()
		{
			lock (this)
			{
				IsActive = true; GameStarted = false;
				innerCounter = MaxMatchingTime;
				OnActive();
			}
		}
		public void Deactivate()
		{
			IsActive = false; IsMatched = false; GameStarted = false; innerCounter = 0;
			MessageSender.SendMatchesData(-1);
			OnDeactive();
			foreach (var player in MatchedPlayers)
			{
				player.InMatch = false;
				player.CurrentMatch = null;
			}
			MatchedPlayers.Clear();
		}

		protected abstract void OnActive();
		protected abstract void OnDeactive();

		public void CompleteMatch()
		{
			IsMatched = true;
			foreach (var player in MatchedPlayers)
			{
				player.CurrentMatch = this;
			}
			OnMatched();
		}

		protected abstract void OnMatched();

		public void MatchNewPlayer(ServerPlayer player)
		{
			lock (this)
			{
				if (!IsActive)
				{
					player.SendMessageBox("这个活动还没有开始匹配", 120, Color.Red);
					return;
				}
				if (IsMatched)
				{
					player.SendMessageBox("这个活动的匹配已经结束了，等待下一轮吧", 120, Color.Red);
					return;
				}
				else
				{
					MatchedPlayers.Add(player);
					player.InMatch = true;
					if (MatchedPlayers.Count == MaxPlayers)
					{
						CompleteMatch();
					}
				}
			}
		}

		public virtual void Update()
		{
			lock (this)
			{
				if (!GameStarted)
				{
					if (innerCounter > 0)
					{
						innerCounter--;
					}
					else
					{
						CompleteMatch();
						innerCounter = MaxMatchingTime;
					}
				}
			}
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Match)) return false;
			var other = (Match)obj;
			return this.Name.Equals(other.Name);
		}

		public SimplifiedMatchInfo GetSimplified(ServerPlayer caller)
		{
			SimplifiedMatchInfo info = new SimplifiedMatchInfo
			{
				Name = Name,
				MaxPlayers = MaxPlayers,
				MatchedPlayers = MatchedPlayers.Count,
				IsMatching = IsActive,
				IsGameStarted = GameStarted,
				TimeRem = innerCounter,
				ReminChance = (MaxChancePerDay == -1 ? -1 : (MaxChancePerDay - caller.TryGetInt("PVEMatchJoined")))
			};
			return info;
		}

		public override int GetHashCode()
		{
			return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
		}
	}
}
