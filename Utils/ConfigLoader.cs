using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ServerSideCharacter2.JsonData;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Utils
{
	public static class ConfigLoader
	{
		public static string defaultName = "SSC/config.json";
		public static void Load()
		{
			if (!File.Exists("SSC/config.json"))
			{
				ServerSideCharacter2.Config = ConfigData.DefaultConfig();
				Save();
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
					CommandBoardcast.ConsoleError("读取配置文件出错");
					CommandBoardcast.ConsoleError(ex);
				} 
			}
		}

		public static void Save()
		{
			string data = JsonConvert.SerializeObject(ServerSideCharacter2.Config, Formatting.Indented);
			using (StreamWriter writer = new StreamWriter(defaultName, false, Encoding.UTF8))
			{
				writer.Write(data);
			}
		}
	}
}
