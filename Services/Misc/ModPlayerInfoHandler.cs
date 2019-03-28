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
			MPlayer mPlayer = Main.player[plr].GetModPlayer<MPlayer>();
			mPlayer.GodMode = reader.ReadBoolean();
			mPlayer.Rank = reader.ReadInt32();
		}

	}
}
