using Microsoft.Xna.Framework;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Regions
{
	public class RegionForbidHandler : SSCCommandHandler
	{
		public override string PermissionName => "region-forbid";

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
				var region = ServerSideCharacter2.RegionManager.Get(name);
				region.Forbidden ^= true;
				MessageSender.SyncRegionsToClient(-1);
				var s = $"成功将领地 {region.Name} {(region.Forbidden ? "禁用" : "解禁")}";
				splayer.SendInfoMessage(s, region.Forbidden ? Color.Red : Color.Lime);
				CommandBoardcast.ConsoleMessage(splayer.Name + s);
			}
		}
	}
}
