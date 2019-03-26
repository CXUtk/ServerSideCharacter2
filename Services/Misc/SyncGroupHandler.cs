using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Newtonsoft.Json;
using ServerSideCharacter2.Groups;

namespace ServerSideCharacter2.Services.Misc
{
	public class SyncGroupHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				var text = reader.ReadString();
				ServerSideCharacter2.MainPlayerGroup = JsonConvert.DeserializeObject<Group>(text);
				ServerSideCharacter2.GuiManager.CheckGroup();
			}
		}
	}
}
