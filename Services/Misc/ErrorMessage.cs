﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Misc
{
	public class ErrorMessage : ISSCNetHandler
	{
		public ErrorMessage()
		{
		}
		public void Handle(BinaryReader reader, int playerNumber)
		{
			var msg = reader.ReadString();
			Main.NewText(msg, Color.Red);
		}
	}
}
