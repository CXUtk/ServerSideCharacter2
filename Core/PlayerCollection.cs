using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using System.Collections;

namespace ServerSideCharacter2.Core
{
	public class PlayerCollection : IEnumerable<KeyValuePair<string, ServerPlayer>>
	{
		[JsonRequired]
		private Dictionary<string, ServerPlayer> _playerList;

		[JsonProperty]
		private int CurrentID = 0;

		public int GetNextID()
		{
			return CurrentID++;
		}

		public void SetID(int id) { CurrentID = id; }

		public PlayerCollection()
		{
			_playerList = new Dictionary<string, ServerPlayer>();
		}

		public void AddNewPlayer(Player p)
		{
			var player = ServerPlayer.CreateNewPlayer(p);
			_playerList.Add(p.name, player);
		}

		public void Add(ServerPlayer p)
		{
			_playerList.Add(p.Name, p);
		}

		public bool ContainsKey(string name)
		{
			return _playerList.ContainsKey(name);
		}

		public ServerPlayer Get(string name)
		{
			return _playerList[name];
		}

		public void SyncPlayers()
		{
			foreach(var p in _playerList)
			{
				if(p.Value.IsLogin)
				p.Value.SyncPlayerToInfo();
			}
		}

		public string GetJson()
		{
			Dictionary<string, PlayerInfo> data = new Dictionary<string, PlayerInfo>();
			ServerPlayerInfo info = new ServerPlayerInfo(data, CurrentID);
			foreach (var p in _playerList)
			{
				if (p.Value.IsLogin || p.Value.HasPassword)
				{
					data.Add(p.Key, p.Value.GetPlayerInfo());
				}
			}
			return JsonConvert.SerializeObject(info);
		}

		public PlayerOnlineInfo getOnlineInfo(int id)
		{
			PlayerOnlineInfo ret = new PlayerOnlineInfo();

			foreach (var player in Main.player)
			{
				if (player.active)
				{
					var serverPlayer = player.GetServerPlayer();
					ret.Player.Add(serverPlayer.GetSimplified(id));
				}
			}
			return ret;
		}

		public IEnumerator<KeyValuePair<string, ServerPlayer>> GetEnumerator()
		{
			return _playerList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _playerList.GetEnumerator();
		}
	}
}
