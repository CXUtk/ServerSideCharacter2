using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.GUI.UI;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Services.Union
{
	public class UnionJoinHandler : SSCCommandHandler
	{
		public override string PermissionName => "union-join";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var name = reader.ReadString();
				var player = Main.player[playerNumber].GetServerPlayer();
				var manager = ServerSideCharacter2.UnionManager;
				if (!manager.ContainsUnion(name))
				{
					player.SendMessageBox("这个公会不存在", 120, Color.OrangeRed);
					return;
				}
				var union = manager.Get(name);
				union.AddCandidate(player);
				CommandBoardcast.ConsoleMessage($"玩家 {player.Name} 申请加入公会 {name}");
			}
		}
	}
}
