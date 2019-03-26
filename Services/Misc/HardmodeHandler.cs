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

namespace ServerSideCharacter2.Services.Misc
{
	public class HardmodeHandler : SSCCommandHandler
	{
		public override string PermissionName => "hardmode";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				string s = $"玩家 {Main.player[playerNumber].name} {(!Main.hardMode ? "开启" : "关闭")}了肉山后模式";
				if (Main.hardMode)
				{
					Main.hardMode = false;
					NetMessage.SendData(MessageID.WorldData);
				}
				else
				{
					WorldGen.StartHardmode();
				}
				ServerPlayer.SendInfoMessageToAll(s);
				CommandBoardcast.ConsoleMessage(s);
			}
		}
	}
}
