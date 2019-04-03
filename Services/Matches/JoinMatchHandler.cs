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
	public class JoinMatchHandler : SSCCommandHandler
	{
		public override string PermissionName => "match-join";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				string name = reader.ReadString();
				var player = Main.player[playerNumber].GetServerPlayer();
				if (!ServerSideCharacter2.MatchingSystem.HasMatch(name))
				{
					player.SendErrorInfo("不存在这个活动");
					return;
				}
				if (!ServerSideCharacter2.MatchingSystem.Matches[name].IsActive)
				{
					player.SendErrorInfo("这个活动还没有开始匹配");
					return;
				}
				if (player.InMatch)
				{
					player.SendErrorInfo("你已经在匹配中了");
					return;
				}
				if (ServerSideCharacter2.MatchingSystem.Matches[name].IsMatched)
				{
					player.SendErrorInfo("这个活动的匹配已经结束了，等待下一轮吧");
					return;
				}
				ServerSideCharacter2.MatchingSystem.MatchPlayer(name, player);
				string str = $"玩家 {player.Name} 加入了 {name} 匹配。当前人数：{ServerSideCharacter2.MatchingSystem.Matches[name].MatchedPlayers.Count} / {ServerSideCharacter2.MatchingSystem.Matches[name].MaxPlayers}";
				ServerPlayer.SendInfoMessageToAll(str);
				CommandBoardcast.ConsoleMessage(str);
			}
		}
	}


}
