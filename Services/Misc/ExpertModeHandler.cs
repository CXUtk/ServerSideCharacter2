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
	public class ExpertModeHandler : SSCCommandHandler
	{
		public override string PermissionName => "expert";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				Main.expertMode ^= true;
				NetMessage.SendData(MessageID.WorldData);
				string s = $"玩家 {Main.player[playerNumber].name} {(Main.expertMode ? "开启" : "关闭")} 专家模式";
				ServerPlayer.SendInfoMessageToAll(s);
				CommandBoardcast.ConsoleMessage(s);
			}
		}
	}
}
