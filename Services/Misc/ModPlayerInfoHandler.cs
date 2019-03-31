using System;
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
			var mPlayer = Main.player[plr].GetModPlayer<MPlayer>();
			mPlayer.GodMode = reader.ReadBoolean();
			mPlayer.Rank = reader.ReadInt32();
			if (Main.netMode == 2)
			{
				MessageSender.SyncModPlayerInfo(-1, playerNumber, mPlayer);
			}
		}

	}
}
