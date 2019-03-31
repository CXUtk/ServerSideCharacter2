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
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 客户端
			if (Main.netMode == 1)
			{
				var data = reader.ReadString();
				var friendsdata = JsonConvert.DeserializeObject<PlayerOnlineInfo>(data);
				// Utils.CommandBoardcast.ShowInWorldTest(data);
				foreach (var info in friendsdata.Player)
				{
					if (info.PlayerID != Main.myPlayer)
					{
						ServerSideCharacter2.GuiManager.AppendFriends(info);
					}
					else
					{
						ServerSideCharacter2.GuiManager.SetMyPlayerProfile(info);
					}
				}
			}
		}
	}
}
