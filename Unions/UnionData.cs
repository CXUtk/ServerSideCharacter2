using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Unions
{
	public class UnionData
	{
		public Dictionary<string, Union> Unions;
		public UnionData()
		{
			Unions = new Dictionary<string, Union>();
		}
	}
	public class UnionDataManager
	{
		private readonly ConfigConverter converter = new ConfigConverter();
		public bool ConfigExist { get; }

		public Dictionary<string, Union> Unions
		{
			get
			{
				return uniondata.Unions;
			}
		}
		private static string _configPath = "SSC/unions.json";

		private UnionData uniondata;

		public UnionDataManager()
		{
			ConfigExist = File.Exists(_configPath);
			SetConfig();
		}

		private void SetConfig()
		{
			if (!ConfigExist)
			{
				uniondata = new UnionData();
				var data = JsonConvert.SerializeObject(uniondata, Formatting.Indented, converter);
				using (var sw = new StreamWriter(_configPath))
				{
					sw.Write(data);
				}
				CommandBoardcast.ConsoleMessage(GameLanguage.GetText("unionconfigcreate"));
			}
			else
			{
				using (var sr = new StreamReader(_configPath))
				{
					var data = sr.ReadToEnd();
					uniondata = JsonConvert.DeserializeObject<UnionData>(data, converter);
				}
			}

			ServerSideCharacter2.UnionManager.Unions = uniondata.Unions;
		}

		public void Save()
		{
			uniondata.Unions = ServerSideCharacter2.UnionManager.Unions;
			var data = JsonConvert.SerializeObject(uniondata, Formatting.Indented, converter);
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
			return (objectType == typeof(UnionData));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{

			var data = serializer.Deserialize<Dictionary<string, Union>>(reader);
			var config = new UnionData();
			for (var i = 0; i < data.Count; i++)
			{
				var pair = data.ElementAt(i);
				pair.Value.Name = pair.Key;
				data[pair.Key] = pair.Value;
			}
			config.Unions = data;
			return config;
		}
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var config = (UnionData)value;
			var obj = JObject.FromObject(config.Unions);
			obj.WriteTo(writer);
		}
	}
}
