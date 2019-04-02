using Microsoft.Xna.Framework;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Regions
{
	public class RegionRemoveHandler : SSCCommandHandler
	{
		public override string PermissionName => "region-remove";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var name = reader.ReadString();
				var splayer = Main.player[playerNumber].GetServerPlayer();
				if (!ServerSideCharacter2.RegionManager.Contains(name))
				{
					splayer.SendErrorInfo("不存在这个领地");
					return;
				}
				ServerSideCharacter2.RegionManager.RemoveRegionWithName(name);
				MessageSender.SyncRegionsToClient(-1);
				splayer.SendInfoMessage($"领地 {name} 移除成功！", Color.LimeGreen);
				CommandBoardcast.ConsoleMessage($"玩家 {splayer.Name} 移除了领地 {name}");
				
			}
		}
	}
}
