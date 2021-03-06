﻿using Microsoft.Xna.Framework;
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

namespace ServerSideCharacter2.Services.Misc
{
	public class GodModeHandler : SSCCommandHandler
	{
		public override string PermissionName => "god";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var p = Main.player[playerNumber];
				var player = p.GetServerPlayer();
				var mPlayer = p.GetModPlayer<MPlayer>();
				mPlayer.GodMode ^= true;

				MessageSender.SyncModPlayerInfo(-1, -1, mPlayer);
				player.SendInfoMessage($"你成功{(mPlayer.GodMode ? "开启" : "关闭")}了无敌模式");
				CommandBoardcast.ConsoleMessage($"玩家 {player.Name} {(mPlayer.GodMode ? "开启" : "关闭")}了无敌模式");
			}
		}
	}


}
