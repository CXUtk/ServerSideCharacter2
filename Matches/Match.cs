using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
				player.CurrentMatch = null;
			}
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
			bool lockTaken = false;
			Monitor.TryEnter(this, 3000, ref lockTaken);
			if (lockTaken)
			{
				try
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
				finally
				{
					Monitor.Exit(this);
				}
			}
			else
			{
				throw new SSCException("获取锁失败，可能导致死锁");
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
