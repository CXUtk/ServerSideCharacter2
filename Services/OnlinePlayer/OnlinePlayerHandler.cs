using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.OnlinePlayer
{
	public class OnlinePlayerHandler : ISSCNetHandler
	{
		public bool Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				var data = reader.ReadString();
				PlayerOnlineInfo onlineinfo = JsonConvert.DeserializeObject<PlayerOnlineInfo>(data);
				Utils.CommandBoardcast.ShowInWorldTest(data);
				foreach(var info in onlineinfo.Player)
				{
					ServerSideCharacter2.GuiManager.AppendOnlinePlayers(info);
				}
			}
			return false;
		}
	}
}
