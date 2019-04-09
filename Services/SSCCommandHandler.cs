using ServerSideCharacter2.Groups;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services
{
	public abstract class SSCCommandHandler : ISSCNetHandler
	{
		public abstract string PermissionName
		{
			get;
		}
		public void Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 2)
			{
				var player = Main.player[playerNumber].GetServerPlayer();
				if (PermissionName != "" && !Permission.CheckPermission(player, PermissionName))
				{
					player.SendErrorInfo("你没有权限使用这个指令");
					return;
				}
			}
			HandleCommand(reader, playerNumber);
		}

		public abstract void HandleCommand(BinaryReader reader, int playerNumber);
	}
}
