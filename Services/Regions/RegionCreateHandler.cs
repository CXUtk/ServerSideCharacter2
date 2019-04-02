﻿using Microsoft.Xna.Framework;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Regions
{
	public class RegionCreateHandler : SSCCommandHandler
	{
		public override string PermissionName => "region-create";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var name = reader.ReadString();
				var upperleft = reader.ReadPackedVector2();
				var lowerright = reader.ReadPackedVector2();
				var splayer = Main.player[playerNumber].GetServerPlayer();
				var rectangle = new Rectangle((int)upperleft.X, (int)upperleft.Y,
					(int)(lowerright.X - upperleft.X), (int)(lowerright.Y - upperleft.Y));
				string err;
				if(ServerSideCharacter2.RegionManager.ValidRegion(splayer, name, rectangle, out err))
				{
					ServerSideCharacter2.RegionManager.CreateNewRegion(rectangle, name, splayer);
					MessageSender.SyncRegionsToClient();
					splayer.SendInfoMessage($"领地 {name} 创建成功！", Color.LimeGreen);
				}
				else
				{
					splayer.SendErrorInfo($"创建领地失败: {err}");
					CommandBoardcast.ConsoleMessage($"玩家 {splayer.Name} 创建领地失败，原因： {err}");
				}
			}
		}
	}
}
