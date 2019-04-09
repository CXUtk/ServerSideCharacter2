using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.GUI;
using ServerSideCharacter2.GUI.UI;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Services.Union
{
	public class ClientUnionHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				var unionName = reader.ReadString();
				if(unionName == "无")
				{
					ServerSideCharacter2.ClientUnion = null;
					if (ServerSideCharacter2.GuiManager.IsActive(SSCUIState.UnionPage2))
					{
						ServerSideCharacter2.GuiManager.SetState(SSCUIState.UnionPage2, false);
					}
					return;
				}
				var owner = reader.ReadString();
				if (ServerSideCharacter2.ClientUnion == null)
				{
					ServerSideCharacter2.ClientUnion = new Unions.Union(unionName);
				}
				else
				{
					ServerSideCharacter2.ClientUnion.Name = unionName;
				}
				ServerSideCharacter2.ClientUnion.Owner = owner;
			}
		}
	}
}
