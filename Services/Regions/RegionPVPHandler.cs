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
	public class RegionPVPHandler : SSCCommandHandler
	{
		public override string PermissionName => "region-pvp";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var name = reader.ReadString();
				var mode = reader.ReadByte();
				var splayer = Main.player[playerNumber].GetServerPlayer();
				if (mode < 0 || mode > 2)
				{
					splayer.SendErrorInfo("PVP模式不合法，0-普通，1-强制不PVP，2-强制PVP");
					return;
				}
				if (!ServerSideCharacter2.RegionManager.Contains(name))
				{
					splayer.SendErrorInfo("不存在这个领地");
					return;
				}
				var region = ServerSideCharacter2.RegionManager.Get(name);
				region.PVP = (PVPMode)mode;
				MessageSender.SyncRegionsToClient(-1);
				var s = $"成功将领地 {region.Name} 的PVP模式设置为 {region.PVP.ToString()}";
				splayer.SendInfoMessage(s, Color.LimeGreen);
				CommandBoardcast.ConsoleMessage(splayer.Name + s);
			}
		}
	}
}
