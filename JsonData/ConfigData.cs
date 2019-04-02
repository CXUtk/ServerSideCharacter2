using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ID;
using Terraria;

namespace ServerSideCharacter2.JsonData
{
	public enum PVPMode : byte
	{
		Normal, 
		Never,
		Always
	}
	[JsonObject]
	public class ConfigData
	{
		public List<ItemInfo> startUpInventory;
		public int PlayerMaxRegions;
		public int MaxRegionWidth;
		public int MaxRegionHeight;
		public int MaxRegions;

		public bool AutoSave { get; set; }
		public int SaveInterval { get; set; }
		public PVPMode PvpMode { get; set; }
		public bool MediumcoreOnly { get; set; }
		public ConfigData()
		{
			startUpInventory = new List<ItemInfo>();
		}

		public static ConfigData DefaultConfig()
		{
			// 给玩家初始物品
			var data = new ConfigData();
			data.startUpInventory.Add(ItemInfo.CreateInfo(ItemID.IronShortsword));
			data.startUpInventory.Add(ItemInfo.CreateInfo(ItemID.IronPickaxe));
			data.startUpInventory.Add(ItemInfo.CreateInfo(ItemID.IronAxe));

			// 开启自动保存
			data.AutoSave = true;
			// 一分钟的保存间隔
			data.SaveInterval = 18000;
			// 禁止墓碑生成
			data.MediumcoreOnly = false;

			data.PvpMode = PVPMode.Normal;

			data.PlayerMaxRegions = 255;
			data.MaxRegions = 255;
			data.MaxRegionWidth = 10000;
			data.MaxRegionHeight = 10000;
			return data;
		}
	}
}
