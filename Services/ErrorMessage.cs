using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services
{
	public class ErrorMessage : ISSCNetHandler
	{
		public ErrorMessage()
		{
		}
		public bool Handle(BinaryReader reader, int playerNumber)
		{
			string msg = reader.ReadString();
			Main.NewText(msg, Color.Red);
			return false;
		}
	}
}
