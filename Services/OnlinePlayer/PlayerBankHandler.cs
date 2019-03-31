using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.OnlinePlayer
{
	public class PlayerBankHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 如果在服务器端
			if (Main.netMode == 1)
			{
				int id = reader.ReadByte();
				if (!Main.ServerSideCharacter && !Main.player[id].IsStackingItems())
				{
					return;
				}
				var player = Main.LocalPlayer;
				lock (player)
				{
					foreach (var item in player.bank.item)
					{
						var type = reader.ReadInt32();
						int prefix = reader.ReadInt16();
						int stack = reader.ReadInt16();
						item.SetDefaults(type);
						item.Prefix(prefix);
						item.stack = stack;
					}
				}
			}
		}
	}
}
