using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.GUI.UI;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Services.Matches
{
	public class GetMatchesHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				MessageSender.SendMatchesData(playerNumber);
				CommandBoardcast.ConsoleMessage($"匹配活动信息已经发送给{Main.player[playerNumber].name}");
			}
			else
			{
				var data = reader.ReadString();
				var info = JsonConvert.DeserializeObject<MatchInfo>(data);
				lock (GameCenterState.Instance)
				{
					GameCenterState.Instance.ClearMatches();
					GameCenterState.Instance.AppendMatches(info);
				}
			}
		}
	}
}
