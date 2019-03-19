using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services
{
	public class NormalMessage : ISSCNetHandler
	{
		public bool Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				string msg = reader.ReadString();
				ServerSideCharacter2.Instance.ShowMessage(msg, 120, Color.White);
			}
			return false;
		}
	}
}
