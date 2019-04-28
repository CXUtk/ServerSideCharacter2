using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System.IO;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Core
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
			var data = ServerSideCharacter2.PlayerCollection.GetJson();
			using (var writer = new StreamWriter(FileName, false, Encoding.UTF8))
			{
				writer.Write(data);
			}

			using (var writer = new StreamWriter(FileName + "bk", false, Encoding.UTF8))
			{
				writer.Write(data);
			}
		}

		public void ExtractPlayersData()
		{
			try
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
				using (var reader = new StreamReader(FileName, Encoding.UTF8))
				{
					data = reader.ReadToEnd();
				}

				var dict = JsonConvert.DeserializeObject<ServerPlayerInfo>(data);
				if (dict.Playerdata.Count > 0)
				{
					foreach (var player in dict.Playerdata)
					{
						var p = new ServerPlayer();
						p.SetPlayerInfo(player.Value);
						p.LoadFromInfo();
						ServerSideCharacter2.PlayerCollection.Add(p);
					}
					ServerSideCharacter2.PlayerCollection.SetID(dict.CurrentID);

				}
				CommandBoardcast.ConsoleMessage(GameLanguage.GetText("FinishReadPlayerDoc"));
				CommandBoardcast.ConsoleMessage($"共计 {ServerSideCharacter2.PlayerCollection.Count} 名玩家的存档");
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}
	}
}
