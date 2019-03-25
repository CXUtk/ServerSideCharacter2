using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services
{
	public class InfoMessage : ISSCNetHandler
	{
		public InfoMessage()
		{
		}
		public bool Handle(BinaryReader reader, int playerNumber)
		{
			string msg = reader.ReadString();
			Color c = reader.ReadRGB();
			Main.NewText(msg, c);
			return false;
		}
	}
}
