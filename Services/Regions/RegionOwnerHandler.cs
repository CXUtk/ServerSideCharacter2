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
	public class RegionOwnerHandler : SSCCommandHandler
	{
		public override string PermissionName => "region-owner";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var name = reader.ReadString();
				var guid = reader.ReadInt32();
				var splayer = Main.player[playerNumber].GetServerPlayer();
				if (!ServerSideCharacter2.RegionManager.Contains(name))
				{
					splayer.SendErrorInfo("不存在这个领地");
					return;
				}
				var owner = ServerSideCharacter2.PlayerCollection.Get(guid);
				if(owner == null)
				{
					splayer.SendErrorInfo("不存在这个玩家");
					return;
				}
				var region = ServerSideCharacter2.RegionManager.Get(name);
				region.OwnerName = splayer.Name;
				MessageSender.SyncRegionsToClient(-1);
				var s = $"成功将领地 {region.Name} 的所有权转移给了 {owner.Name}";
				splayer.SendInfoMessage(s, Color.Lime);
				CommandBoardcast.ConsoleMessage(splayer.Name + s);
			}
		}
	}
}
