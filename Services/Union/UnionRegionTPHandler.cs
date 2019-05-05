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
	public class UnionRegionTPHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var splayer = Main.player[playerNumber].GetServerPlayer();
				if (splayer.Union == null)
				{
					splayer.SendMessageBox("你不在任何一个公会中", 120, Color.OrangeRed);
					return;
				}
				var union = splayer.Union;
				var region = union.OwnedRegion;
				if(region == null)
				{
					splayer.SendMessageBox("公会并没有分配领地", 120, Color.OrangeRed);
					return;
				}
				splayer.SafeTeleport(region.GetWorldHitBox().Center());
				CommandBoardcast.ConsoleMessage($"玩家{splayer.Name} 传送到了公会{union.Name} 的领地{region.Name}");
			}
		}
	}
}
