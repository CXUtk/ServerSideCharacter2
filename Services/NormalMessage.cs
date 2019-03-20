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
		private int time;
		public NormalMessage(int time)
		{
			this.time = time;
		}
		public bool Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				string msg = reader.ReadString();
				ServerSideCharacter2.Instance.ShowMessage(msg, time, Color.White);
			}
			return false;
		}
	}
}
