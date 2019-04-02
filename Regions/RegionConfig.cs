using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Regions
{
	public class RegionConfig : DataConfig<Region>
	{
		public override string Path => "SSC/regions.json";

		public override string Message => GameLanguage.GetText("regionconfigcreate");

		public override Dictionary<string, Region> Get()
		{
			return ServerSideCharacter2.RegionManager.Regions;
		}

		public override void Receive(Dictionary<string, Region> data)
		{
			ServerSideCharacter2.RegionManager.Regions = data;
		}
	}
	//public class RegionData
	//{
	//	public Dictionary<string, Region> ServerRegions = new Dictionary<string, Region>();
	//}
	//public class RegionConfig
	//{
	//	private readonly ConfigConverter converter = new ConfigConverter();
	//	public bool ConfigExist { get; }

	//	public Dictionary<string, Region> Groups
	//	{
	//		get
	//		{
	//			return _configData.ServerRegions;
	//		}
	//	}
	//	private static string _configPath = "SSC/regions.json";

	//	private RegionData _configData;

	//	public RegionConfig()
	//	{
	//		ConfigExist = File.Exists(_configPath);
	//		SetConfig();
	//	}

	//	private void SetConfig()
	//	{
	//		if (!ConfigExist)
	//		{
	//			_configData = new RegionData();
	//			var data = JsonConvert.SerializeObject(_configData, Formatting.Indented, converter);
	//			using (var sw = new StreamWriter(_configPath))
	//			{
	//				sw.Write(data);
	//			}
	//			CommandBoardcast.ConsoleMessage(GameLanguage.GetText("regionconfigcreate"));
	//		}
	//		else
	//		{
	//			using (var sr = new StreamReader(_configPath))
	//			{
	//				var data = sr.ReadToEnd();
	//				_configData = JsonConvert.DeserializeObject<RegionData>(data, converter);
	//			}
	//		}

	//		ServerSideCharacter2.RegionManager.Regions = _configData.ServerRegions;
	//	}

	//	public void Save()
	//	{
	//		_configData.ServerRegions = ServerSideCharacter2.RegionManager.Regions;
	//		var data = JsonConvert.SerializeObject(_configData, Formatting.Indented, converter);
	//		using (var sw = new StreamWriter(_configPath))
	//		{
	//			sw.Write(data);
	//		}
	//	}

	//}
	//public class ConfigConverter : JsonConverter
	//{
	//	public override bool CanConvert(Type objectType)
	//	{
	//		return (objectType == typeof(RegionData));
	//	}

	//	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	//	{

	//		var data = serializer.Deserialize<Dictionary<string, Region>>(reader);
	//		var config = new RegionData();
	//		for (var i = 0; i < data.Count; i++)
	//		{
	//			var pair = data.ElementAt(i);
	//			pair.Value.Name = pair.Key;
	//			data[pair.Key] = pair.Value;
	//		}
	//		config.ServerRegions = data;
	//		return config;
	//	}
	//	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	//	{
	//		var config = (RegionData)value;
	//		var obj = JObject.FromObject(config.ServerRegions);
	//		obj.WriteTo(writer);
	//	}
	//}
}

