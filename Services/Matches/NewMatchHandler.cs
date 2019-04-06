using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Services.Matches
{
	public class NewMatchHandler : SSCCommandHandler
	{
		public override string PermissionName => "match-new";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				string name = reader.ReadString();
				var player = Main.player[playerNumber].GetServerPlayer();
				if (!ServerSideCharacter2.MatchingSystem.HasMatch(name))
				{
					player.SendMessageBox("不存在这个活动", 120, Color.Red);
					return;
				}
				if (player.InMatch)
				{
					player.SendMessageBox("你已经在匹配中了，不能发起匹配多个活动", 120, Color.Red);
					return;
				}
				if (ServerSideCharacter2.MatchingSystem.Matches[name].IsActive)
				{
					player.SendMessageBox("这个活动已经在匹配中了", 120, Color.Red);
					return;
				}
				ServerSideCharacter2.MatchingSystem.StartMatch(name);
				string str = $"玩家 {player.Name} 开启了 {name} 匹配，快加入吧";
				ServerPlayer.SendInfoMessageToAll(str);
				MessageSender.SendMatchesData(-1);
				CommandBoardcast.ConsoleMessage(str);
			}
		}
	}


}
