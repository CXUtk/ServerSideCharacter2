using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.GUI.UI;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.RankingSystem;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Rank
{
	public class GetRankBoardHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 2)
			{
				List<SimplifiedPlayerInfo> infos = new List<SimplifiedPlayerInfo>();
				foreach (var info in ServerSideCharacter2.RankData.LastBoard)
				{
					var simpl = ServerSideCharacter2.PlayerCollection.Get(info.Name).GetSimplified(-1);
					simpl.Rank = info.Rank;
					infos.Add(simpl);
				}
				string data = JsonConvert.SerializeObject(ServerSideCharacter2.RankData, Formatting.None);
				string liststr = JsonConvert.SerializeObject(infos, Formatting.None);
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.RequestRankBoard);
				p.Write(data);
				p.Write(liststr);
				p.Send(playerNumber);
				CommandBoardcast.ConsoleMessage($"排位榜单已经发送给 {Main.player[playerNumber].name}");
			}
			else
			{
				string data = reader.ReadString();
				string playerinfostr = reader.ReadString();
				RankData rankdata = JsonConvert.DeserializeObject<RankData>(data);
				List<SimplifiedPlayerInfo> infos = JsonConvert.DeserializeObject<List<SimplifiedPlayerInfo>>(playerinfostr);
				lock (RankBoardState.Instance)
				{
					RankBoardState.Instance.Apply(rankdata, infos);
				}
			}
		}
	}
}
