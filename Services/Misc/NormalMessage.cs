using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Misc
{
	public class NormalMessage : ISSCNetHandler
	{
		public NormalMessage()
		{
		}
		public void Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				var msg = reader.ReadString();
				var color = reader.ReadRGB();
				var time = reader.ReadInt32();
				ServerSideCharacter2.Instance.ShowMessage(msg, time, color);
			}
		}
	}
}
