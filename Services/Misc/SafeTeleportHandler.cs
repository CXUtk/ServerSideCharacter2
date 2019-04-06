using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;

namespace ServerSideCharacter2.Services.Misc
{
	public class SafeTeleportHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			if(Main.netMode == 1)
			{
				int plr = reader.ReadByte();
				var pos = reader.ReadVector2();
				var player = Main.player[plr];
				player.grappling[0] = -1;
				player.grapCount = 0;
				Main.TeleportEffect(player.Hitbox, 0, 0, 1);
				PressurePlateHelper.UpdatePlayerPosition(player);
				player.position = pos;
				player.fallStart = player.fallStart2 = (int)(player.position.Y / 16f);
				PressurePlateHelper.UpdatePlayerPosition(player);
				Main.TeleportEffect(player.Hitbox, 0, 0, 1);
				for (int j = 0; j < 3; j++)
				{
					player.UpdateSocialShadow();
				}
				player.oldPosition = player.position + player.BlehOldPositionFixer;
			}
		}
	}

}
