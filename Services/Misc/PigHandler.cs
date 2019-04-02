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
using Terraria.Localization;

namespace ServerSideCharacter2.Services.Misc
{
	public class PigHandler : SSCCommandHandler
	{
		public override string PermissionName => "pig";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				int target = reader.ReadByte();
				var p = Main.player[playerNumber];
				var target0 = Main.player[target];
				var player = p.GetServerPlayer();
				var mplayer = target0.GetModPlayer<MPlayer>();
				mplayer.Piggify ^= true;
				MessageSender.SyncModPlayerInfo(-1, -1, mplayer);
				player.SendInfoMessage($"你成功的把 {target0.name} 变{(mplayer.Piggify ? "成了猪头" : "了回来")}", Color.Purple);
				MessageSender.SendInfoMessage(target0.whoAmI, $"你被 {player.Name} 变{(mplayer.Piggify ? "成了猪头" : "了回来")}", Color.Purple);
				CommandBoardcast.ConsoleMessage($"玩家 {player.Name} 把玩家 {target0.name} 变{(mplayer.Piggify ? "成了猪头" : "了回来")}");
			}
		}
	}
}
