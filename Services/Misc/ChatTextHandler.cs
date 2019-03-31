using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Newtonsoft.Json;
using ServerSideCharacter2.Groups;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Services.Misc
{
	public class ChatTextHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				var id = reader.ReadByte();
				var name = reader.ReadString();
				var text = reader.ReadString();
				var prefix = reader.ReadString();
				var c = reader.ReadRGB();
				if (id < 255)
				{
					Main.player[id].chatOverhead.NewMessage(text, Main.chatLength / 2);
					// text = NameTagHandler.GenerateTag(Main.player[(int)b].name) + " " + text;
				}
				var real = $"<[c/{c.Hex3()}:{prefix}]> {name}: {text}";
				Main.NewTextMultiline(real, false, Color.White, -1);
			}
		}
	}
}
