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
	public class GetFriendsHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var player = Main.player[playerNumber].GetServerPlayer();
				PlayerOnlineInfo ret = new PlayerOnlineInfo();
				foreach (var f in player.Friends)
				{
					var friend = ServerSideCharacter2.PlayerCollection.Get(f);
					ret.Player.Add(friend.GetSimplified(playerNumber));
				}
				ret.Player.Add(player.GetSimplified(playerNumber));
				var data = JsonConvert.SerializeObject(ret, Formatting.None);
				MessageSender.SendFriendsData(playerNumber, data);
			}
		}
	}
}
