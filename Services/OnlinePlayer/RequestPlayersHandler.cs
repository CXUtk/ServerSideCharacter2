using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.OnlinePlayer
{
	public class RequestPlayersHandler : ISSCNetHandler
	{
		public bool Handle(BinaryReader reader, int playerNumber)
		{
			// 如果在服务器端
			if (Main.netMode == 2)
			{
				var info = ServerSideCharacter2.PlayerCollection.getOnlineInfo();
				MessageSender.SendOnlineInformation(playerNumber, JsonConvert.SerializeObject(info, Formatting.Indented));
				Utils.CommandBoardcast.ConsoleMessage("收到在线玩家请求");
			}
			return false;
		}
	}
}
