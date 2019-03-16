using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ID;
using Terraria;

namespace ServerSideCharacter2.JsonData
{
	[JsonObject]
	public class ConfigData
	{
		public List<ItemInfo> startUpInventory;
		public int SaveInterval { get; set; }
		public ConfigData()
		{
			startUpInventory = new List<ItemInfo>();
		}

		public static ConfigData DefaultConfig()
		{
			ConfigData data = new ConfigData();
			data.startUpInventory.Add(ItemInfo.CreateInfo(ItemID.IronShortsword));
			data.startUpInventory.Add(ItemInfo.CreateInfo(ItemID.IronPickaxe));
			data.startUpInventory.Add(ItemInfo.CreateInfo(ItemID.IronAxe));

			// 一分钟的保存间隔
			data.SaveInterval = 3600;
			return data;
		}
	}
}
