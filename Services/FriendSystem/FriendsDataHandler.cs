using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.FriendSystem
{
	public class FriendsDataHandler : ISSCNetHandler
	{
		public bool Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 1)
			{
				var data = reader.ReadString();
				PlayerOnlineInfo friendsdata = JsonConvert.DeserializeObject<PlayerOnlineInfo>(data);
				// Utils.CommandBoardcast.ShowInWorldTest(data);
				foreach (var info in friendsdata.Player)
				{
					ServerSideCharacter2.GuiManager.AppendFriends(info);
				}
			}
			return false;
		}
	}
}
