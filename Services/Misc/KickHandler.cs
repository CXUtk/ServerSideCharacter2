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

namespace ServerSideCharacter2.Services.Misc
{
	public class KickHandler : SSCCommandHandler
	{
		public override string PermissionName => "kick";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				int target = reader.ReadByte();
				var player = Main.player[playerNumber];
				var target0 = Main.player[target];
				var target1 = target0.GetServerPlayer();

				var str = $"玩家 {player.name} 把玩家 {target0.name} 踢出了服务器";
				ServerPlayer.SendInfoMessageToAll(str);
				target1.Kick("你被管理员踢出了服务器");
				CommandBoardcast.ConsoleMessage(str);
			}
		}
	}


}
