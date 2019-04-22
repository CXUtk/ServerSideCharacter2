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
	public class UnionRemoveHandler : SSCCommandHandler
	{
		public override string PermissionName => "union-remove";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var name = reader.ReadString();
				var splayer = Main.player[playerNumber].GetServerPlayer();
				if (splayer.Union == null)
				{
					splayer.SendMessageBox("你不在任何一个公会中", 120, Color.OrangeRed);
					return;
				}
				if (!ServerSideCharacter2.UnionManager.ContainsUnion(name))
				{
					splayer.SendMessageBox("不存在这个名字的公会", 120, Color.OrangeRed);
					return;
				}
				if(splayer.Union.Name != name && !splayer.Group.IsSuperAdmin)
				{
					splayer.SendMessageBox("你只能解散/退出自己的公会", 180, Color.OrangeRed);
					return;
				}
				if (splayer.Union.Owner == splayer.Name || splayer.Group.IsSuperAdmin)
				{
					ServerSideCharacter2.UnionManager.RemoveUnion(name);
					splayer.SendMessageBox("公会解散成功", 180, Color.LimeGreen);
					var str = $"玩家 {splayer.Name} 解散了公会 {name}！";
					ServerPlayer.SendInfoMessageToAll(str);
					CommandBoardcast.ConsoleMessage(str);
				}
				else
				{
					var union = splayer.Union;
					union.RemoveMember(splayer);
					splayer.SendMessageBox("退出公会成功", 180, Color.LimeGreen);
					CommandBoardcast.ConsoleMessage($"玩家 {splayer.Name} 退出了公会 {union.Name}");
				}
			}
		}
	}
}
