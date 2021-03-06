﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Misc
{
	public class ModPlayerInfoHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			int plr = reader.ReadByte();
			if (Main.netMode == 2)
			{
				plr = playerNumber;
			}
			BitsByte bb = reader.ReadByte();
			if (plr < 0 || plr >= 255) return;
			var mPlayer = Main.player[plr].GetModPlayer<MPlayer>();
			mPlayer.GodMode = bb[0];
			mPlayer.Piggify = bb[1];
			mPlayer.ShowRank = bb[2];
			mPlayer.ShowCrown = bb[3];
			mPlayer.Rank = reader.ReadInt32();
			if (Main.netMode == 2)
			{
				MessageSender.SyncModPlayerInfo(-1, playerNumber, mPlayer);
			}
		}

	}
}
