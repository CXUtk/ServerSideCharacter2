﻿using ServerSideCharacter2.Groups;
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
				ServerPlayer player = Main.player[playerNumber].GetServerPlayer();
				if (!Permission.CheckPermission(player, PermissionName))
				{
					return;
				}
			}
			HandleCommand(reader, playerNumber);
		}

		public abstract void HandleCommand(BinaryReader reader, int playerNumber);
	}
}