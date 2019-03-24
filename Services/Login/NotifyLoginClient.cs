using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Login
{
	public class NotifyLoginClient : ISSCNetHandler
	{
		public bool Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				ServerSideCharacter2.Instance.IsLoginClientSide = true;
			}
			return false;
		}
	}
}
