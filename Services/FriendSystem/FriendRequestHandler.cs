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
	public class FriendRequestHandler : ISSCNetHandler
	{
		public bool Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var name = reader.ReadString();
				Utils.CommandBoardcast.ConsoleMessage("收到玩家" + Main.player[playerNumber].name + "请求添加" + name + "为好友");
				if (ServerSideCharacter2.PlayerCollection.ContainsKey(name))
				{
					var target = ServerSideCharacter2.PlayerCollection.Get(name);
					var src = Main.player[playerNumber].GetServerPlayer();
					if (target.Name.Equals(src.Name))
					{
						MessageSender.SendLoginFailed(playerNumber, "你不能添加自己为好友！");
						return false;
					}
					if (src.Friends.Contains(target.Name))
					{
						MessageSender.SendLoginFailed(playerNumber, "你已经是" + target.Name + "的好友了");
						return false;
					}
					src.Friends.Add(target.Name);
					MessageSender.SendLoginSuccess(playerNumber, "添加好友成功！");
				}
				else
				{
					MessageSender.SendLoginFailed(playerNumber, "玩家不存在！");
				}
			}
			return false;
		}
	}
}
