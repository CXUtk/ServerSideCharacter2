using Microsoft.Xna.Framework;
using ServerSideCharacter2.Services.Login;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Union
{
	public class UnionCreateHandler : SSCCommandHandler
	{
		public override string PermissionName => "union-create";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var name = reader.ReadString();
				var player = Main.player[playerNumber];
				var splayer = player.GetServerPlayer();
				if (splayer.Union != null)
				{
					splayer.SendMessageBox("你已经有公会了", 120, Color.Yellow);
					return;
				}
				if (ServerSideCharacter2.UnionManager.ContainsUnion(name))
				{
					splayer.SendMessageBox("该名字的公会已经存在", 120, Color.OrangeRed);
					return;
				}
				if (Authorization.CheckName(name) != 0)
				{
					splayer.SendMessageBox("公会名字不合法，长度应为2-10之间，且不能包含非法字符", 120, Color.OrangeRed);
					return;
				}
				ServerSideCharacter2.UnionManager.CreateUnion(name, splayer);
				splayer.SendMessageBox("公会创建成功", 180, Color.LimeGreen);
				var s = $"玩家 {splayer.Name} 创建了公会 {name}，快来看看吧";
				ServerPlayer.SendInfoMessageToAll(s);
				CommandBoardcast.ConsoleMessage(s);
			}
		}
	}
}
