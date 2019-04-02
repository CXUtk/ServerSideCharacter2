using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Misc
{
	public class InfoMessage : ISSCNetHandler
	{
		public InfoMessage()
		{
		}
		public void Handle(BinaryReader reader, int playerNumber)
		{
			var msg = reader.ReadString();
			var c = reader.ReadRGB();
			Main.NewTextMultiline(msg, false, c);
		}
	}
}
