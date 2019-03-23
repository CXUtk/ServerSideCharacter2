using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ServerSideCharacter2.JsonData;
using Newtonsoft.Json;
using ServerSideCharacter2.Group;

namespace ServerSideCharacter2.Utils
{
	public static class ConfigLoader
	{
		public static string defaultName = "SSC/config.json";
		private static GroupConfigManager GroupConfigManager;

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
					using (StreamReader reader = new StreamReader(defaultName, Encoding.UTF8))
					{
						data = reader.ReadToEnd();
					}

					ServerSideCharacter2.Config = JsonConvert.DeserializeObject<ConfigData>(data);
				}
				catch(Exception ex)
				{
					CommandBoardcast.ConsoleError("读取配置文件出错，重置为默认配置");
					CommandBoardcast.ConsoleError(ex);
					ServerSideCharacter2.Config = ConfigData.DefaultConfig();
				} 
			}
			CommandBoardcast.ConsoleMessage("配置文件已经加载");
			CommandBoardcast.ConsoleMessage(string.Format("当前配置  自动保存: {0}，自动保存间隔：{1}s",
				ServerSideCharacter2.Config.AutoSave ? "开" : "关", ServerSideCharacter2.Config.SaveInterval / 60f));

			GroupConfigManager = new GroupConfigManager();

			Save();
		}

		public static void Save()
		{
			string data = JsonConvert.SerializeObject(ServerSideCharacter2.Config, Formatting.Indented);
			using (StreamWriter writer = new StreamWriter(defaultName, false, Encoding.UTF8))
			{
				writer.Write(data);
			}

			GroupConfigManager.Save();
		}
	}
}
