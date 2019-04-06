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
	public class BanHandler : SSCCommandHandler
	{
		public override string PermissionName => "ban";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				int target = reader.ReadInt32();
				string reason = reader.ReadString();
				var player = ServerSideCharacter2.PlayerCollection.Get(target);
				player.Ban(reason);
				string str = $"玩家 {player.Name} 被管理员 {Main.player[playerNumber].name} 安排了， 原因是：{reason}";
				ServerPlayer.SendInfoMessageToAll(str);
				CommandBoardcast.ConsoleMessage(str);
			}
		}
	}


}
