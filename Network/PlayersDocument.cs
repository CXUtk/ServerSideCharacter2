using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System.IO;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Network
{
	public class PlayersDocument
	{
		public string FileName { get; }

		public PlayersDocument(string fileName)
		{
			if (!Directory.Exists("SSC"))
			{
				Directory.CreateDirectory("SSC");
			}
			FileName = "SSC/" + fileName;
		}

		public void SavePlayersData()
		{
			ServerSideCharacter2.PlayerCollection.SyncPlayers();
			string data = ServerSideCharacter2.PlayerCollection.GetJson();
			using (StreamWriter writer = new StreamWriter(FileName, false, Encoding.UTF8))
			{
				writer.Write(data);
			}
		}

		public void ExtractPlayersData()
		{
			if (!File.Exists(FileName))
			{
				CommandBoardcast.ConsoleMessage(GameLanguage.GetText("creatingPlayerDoc"));
				ServerSideCharacter2.PlayerCollection = new PlayerCollection();
				SavePlayersData();
				return;
			}

			CommandBoardcast.ConsoleMessage(GameLanguage.GetText("readingPlayerDoc"));
			string data;
			using(StreamReader reader = new StreamReader(FileName, Encoding.UTF8))
			{
				data = reader.ReadToEnd();
			}

			var dict = JsonConvert.DeserializeObject<Dictionary<string, PlayerInfo>>(data);
			if (dict.Count > 0)
			{
				foreach (var player in dict)
				{
					ServerPlayer p = new ServerPlayer();
					p.SetPlayerInfo(player.Value);
					p.SyncPlayerFromInfo();
					ServerSideCharacter2.PlayerCollection.Add(p);
				}
			}
			CommandBoardcast.ConsoleMessage(GameLanguage.GetText("FinishReadPlayerDoc"));
		}
	}
}
