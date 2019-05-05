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
	public class UnionToggleBuilderHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var name = reader.ReadString();
				var splayer = Main.player[playerNumber].GetServerPlayer();
				if (!ServerSideCharacter2.PlayerCollection.ContainsKey(name))
				{
					splayer.SendMessageBox("这个玩家不存在", 120, Color.OrangeRed);
					return;
				}
				if (splayer.Union == null)
				{
					splayer.SendMessageBox("你不在任何一个公会中", 120, Color.OrangeRed);
					return;
				}
				var target = ServerSideCharacter2.PlayerCollection.Get(name);

				if (splayer.Union.Owner != splayer.Name)
				{
					splayer.SendMessageBox("只有会长可以切换玩家的建筑师权限", 180, Color.OrangeRed);
					return;
				}
				if (splayer.Union.Name != target.Union.Name)
				{
					splayer.SendMessageBox("你们不在一个公会中", 180, Color.OrangeRed);
					return;
				}
				splayer.Union.ToggleBuilder(target);
				if (splayer.Union.Builders.Contains(target.Name))
				{
					splayer.SendMessageBox($"你已成功设置 {target.Name} 为建筑师", 180, Color.LimeGreen);
					target.SendInfoMessage($"你已被设置为 {splayer.Union.Name} 的建筑师");
				}
				else
				{
					splayer.SendMessageBox($"你已成功取消了 {target.Name} 的建筑师权限", 180, Color.LimeGreen);
					target.SendInfoMessage($"你已被取消 {splayer.Union.Name} 的建筑师权限");
				}
				CommandBoardcast.ConsoleMessage($"{splayer.Union.Name} 切换了玩家 {target.Name} 的建筑师身份");
			}
		}
	}
}
