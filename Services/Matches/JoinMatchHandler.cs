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
					player.SendMessageBox("不存在这个活动", 120, Color.Red);
					return;
				}
				var match = ServerSideCharacter2.MatchingSystem.Matches[name];
				if (player.InMatch)
				{
					player.SendMessageBox("你已经在匹配中了", 120, Color.Red);
					return;
				}
				if (!match.IsActive)
				{
					player.SendMessageBox("这个活动还没开始匹配", 120, Color.Red);
					return;
				}
				if (match.GameStarted)
				{
					player.SendMessageBox("这个活动正在进行，请等待下一轮", 120, Color.Red);
					return;
				}
				if (!player.Group.IsSuperAdmin && match.MaxChancePerDay != -1 && player.TryGetInt("PVEMatchJoined") >= match.MaxChancePerDay)
				{
					player.SendMessageBox("您已用完今日的参与次数，请明日再来吧", 120, Color.Red);
					return;
				}
				ServerSideCharacter2.MatchingSystem.MatchPlayer(name, player);
				string str = $"玩家 {player.Name} 加入了 {name} 匹配。当前人数：{ServerSideCharacter2.MatchingSystem.Matches[name].MatchedPlayers.Count} / {ServerSideCharacter2.MatchingSystem.Matches[name].MaxPlayers}";
				MessageSender.SendMatchesData(-1);
				ServerPlayer.SendInfoMessageToAll(str);
				CommandBoardcast.ConsoleMessage(str);
			}
		}
	}


}
