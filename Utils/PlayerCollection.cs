using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;

namespace ServerSideCharacter2.Utils
{
	public class PlayerCollection
	{
		[JsonRequired]
		private Dictionary<string, ServerPlayer> _playerList;

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
				p.Value.SyncPlayerToInfo();
			}
		}

		public string GetJson()
		{
			Dictionary<string, PlayerInfo> PlayersData = new Dictionary<string, PlayerInfo>();
			foreach(var p in _playerList)
			{
				PlayersData.Add(p.Key, p.Value.GetPlayerInfo());
			}
			return JsonConvert.SerializeObject(PlayersData);
		}
	}
}
