using Microsoft.Xna.Framework;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Regions;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Regions
{
	public class RegionSyncHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 1)
			{
				int count = reader.ReadInt32();
				ServerSideCharacter2.ClientRegions.Clear();
				for(int i = 0; i < count; i++)
				{
					var name = reader.ReadString();
					var ownername = reader.ReadString();
					var rect = reader.ReadRect();
					var pvp = reader.ReadByte();
					var forbidden = reader.ReadBoolean();
					var region = new Region(name, rect);
					region.OwnerName = ownername;
					region.PVP = (PVPMode)pvp;
					region.Forbidden = forbidden;
					ServerSideCharacter2.ClientRegions.Add(name, region);
				}
			}
		}
	}
}
