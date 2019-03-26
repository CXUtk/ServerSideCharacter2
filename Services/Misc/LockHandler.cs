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
	public class LockHandler : SSCCommandHandler
	{
		public override string PermissionName => "lock";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				int plr = reader.ReadByte();
				int target = reader.ReadByte();
				int time = reader.ReadInt32();
				Player p = Main.player[plr];
				Player target0 = Main.player[target];
				ServerPlayer player = p.GetServerPlayer();
				ServerPlayer target1 = target0.GetServerPlayer();

				target1.ApplyLockBuffs(time);
				NetMessage.SendChatMessageToClient(NetworkText.FromLiteral(string.Format("你成功的锁住了 {0} 持续 {1:N2} 秒", target1.Name, time / 60.0f)), new Color(255, 50, 255, 50), plr);
				MessageSender.SendInfoMessage(target0.whoAmI, string.Format("你被管理员锁住了，持续 {0:N2} 秒", time / 60f), Color.Red);
				CommandBoardcast.ConsoleMessage($"玩家 {player.Name} 锁住了 {target1.Name} {time / 60f:N2} 秒.");
			}
		}
	}
}
