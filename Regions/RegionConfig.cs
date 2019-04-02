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
	public class RegionData
	{
		public Dictionary<string, Region> ServerRegions = new Dictionary<string, Region>();
	}
	public class RegionConfig
	{
		private readonly ConfigConverter converter = new ConfigConverter();
		public bool ConfigExist { get; }

		public Dictionary<string, Region> Groups
		{
			get
			{
				return _configData.ServerRegions;
			}
		}
		private static string _configPath = "SSC/regions.json";

		private RegionData _configData;

		public RegionConfig()
		{
			ConfigExist = File.Exists(_configPath);
			SetConfig();
		}

		private void SetConfig()
		{
			if (!ConfigExist)
			{
				_configData = new RegionData();
				var data = JsonConvert.SerializeObject(_configData, Formatting.Indented, converter);
				using (var sw = new StreamWriter(_configPath))
				{
					sw.Write(data);
				}
				CommandBoardcast.ConsoleMessage(GameLanguage.GetText("regionconfigcreate"));
			}
			else
			{
				using (var sr = new StreamReader(_configPath))
				{
					var data = sr.ReadToEnd();
					_configData = JsonConvert.DeserializeObject<RegionData>(data, converter);

					//_configData.GroupType.SetupGroups(!_configData.GroupType.Groups.ContainsKey("default"), !_configData.GroupType.Groups.ContainsKey("criminal"), false, !_configData.GroupType.Groups.ContainsKey("spadmin")); //Add default, criminal and spadmin group if not exists
					//_configData.GroupType.Groups["spadmin"].permissions.Clear(); //clear all spadmin user created permissions
					//_configData.GroupType.Groups["spadmin"].permissions.Add(new PermissionInfo("all", "all commands")); //add all permissions to spadmin group
				}
			}

			ServerSideCharacter2.RegionManager.Regions = _configData.ServerRegions;
		}

		public void Save()
		{
			_configData.ServerRegions = ServerSideCharacter2.RegionManager.Regions;
			var data = JsonConvert.SerializeObject(_configData, Formatting.Indented, converter);
			using (var sw = new StreamWriter(_configPath))
			{
				sw.Write(data);
			}
		}

	}
	public class ConfigConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(RegionData));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{

			var data = serializer.Deserialize<Dictionary<string, Region>>(reader);
			var config = new RegionData();
			for (var i = 0; i < data.Count; i++)
			{
				var pair = data.ElementAt(i);
				pair.Value.Name = pair.Key;
				data[pair.Key] = pair.Value;
			}
			config.ServerRegions = data;
			return config;
		}
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var config = (RegionData)value;
			var obj = JObject.FromObject(config.ServerRegions);
			obj.WriteTo(writer);
		}
	}
}

