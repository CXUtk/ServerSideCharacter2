using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ServerSideCharacter2.JsonData;
using Newtonsoft.Json;
using ServerSideCharacter2.Groups;
using ServerSideCharacter2.Regions;
using ServerSideCharacter2.RankingSystem;

namespace ServerSideCharacter2.Utils
{
	public static class ConfigLoader
	{
		public static string defaultName = "SSC/config.json";
		private static GroupConfig GroupConfigManager;
		private static Unions.UnionConfig UnionDataManager;
		private static RegionConfig RegionConfig;

		public static void Load()
		{
			if (!File.Exists("SSC/config.json"))
			{
				ServerSideCharacter2.Config = ConfigData.DefaultConfig();
			}
			else
			{
				try
				{
					string data;
					using (var reader = new StreamReader(defaultName, Encoding.UTF8))
					{
						data = reader.ReadToEnd();
					}

					ServerSideCharacter2.Config = JsonConvert.DeserializeObject<ConfigData>(data);
					QQAuth.ConnectionStr = $"server={ServerSideCharacter2.Config.ServerAddr};User Id={ServerSideCharacter2.Config.ServerUserID};Password={ServerSideCharacter2.Config.ServerPassword};" +
						$"Database={ServerSideCharacter2.Config.DatabaseName};port={ServerSideCharacter2.Config.ServerPort}";
				}
				catch(Exception ex)
				{
					CommandBoardcast.ConsoleError("读取配置文件出错，重置为默认配置");
					CommandBoardcast.ConsoleError(ex);
					ServerSideCharacter2.Config = ConfigData.DefaultConfig();
				} 
			}
			CommandBoardcast.ConsoleMessage("配置文件已经加载");
			CommandBoardcast.ConsoleMessage(
				$"当前配置  自动保存: {(ServerSideCharacter2.Config.AutoSave ? "开" : "关")}，自动保存间隔：{ServerSideCharacter2.Config.SaveInterval / 60f}s");

			GroupConfigManager = new GroupConfig();
			UnionDataManager = new Unions.UnionConfig();
			RegionConfig = new RegionConfig();
			ServerSideCharacter2.RankData = RankData.Load();

			Save();
		}

		public static void Save()
		{
			var data = JsonConvert.SerializeObject(ServerSideCharacter2.Config, Formatting.Indented);
			using (var writer = new StreamWriter(defaultName, false, Encoding.UTF8))
			{
				writer.Write(data);
			}

			GroupConfigManager.Save();
			UnionDataManager.Save();
			RegionConfig.Save();
			RankData.Save(ServerSideCharacter2.RankData);
		}
	}
}
