using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Utils
{
	public interface IName
	{
		string Name { get; set; }
	}

	public abstract class DataConfig<T> where T : IName
	{
		protected class ConfigData
		{
			public Dictionary<string, T> Data = new Dictionary<string, T>();
		}
		
		public abstract string Path { get; }
		public abstract string Message { get; }
		public bool ConfigExist { get; }
		private ConfigData _data;
		private readonly ConfigConverter converter = new ConfigConverter();

		public DataConfig()
		{
			ConfigExist = File.Exists(Path);
			if (!ConfigExist)
			{
				_data = new ConfigData();
				SetDefaults(_data);
				CommandBoardcast.ConsoleMessage(_data.Data.ToString());
				var data = JsonConvert.SerializeObject(_data, Formatting.Indented, converter);
				using (var sw = new StreamWriter(Path))
				{
					sw.Write(data);
				}
				CommandBoardcast.ConsoleMessage(Message);
			}
			else
			{
				using (var sr = new StreamReader(Path))
				{
					var data = sr.ReadToEnd();
					_data = JsonConvert.DeserializeObject<ConfigData>(data, converter);
				}
				Receive(_data.Data);
			}
		}

		public void Save()
		{
			_data.Data = Get();
			var data = JsonConvert.SerializeObject(_data, Formatting.Indented, converter);
			using (var sw = new StreamWriter(Path))
			{
				sw.Write(data);
			}
		}

		public abstract void Receive(Dictionary<string, T> data);
		public abstract Dictionary<string, T> Get();
		protected virtual void SetDefaults(ConfigData _data) {  }

		private class ConfigConverter : JsonConverter
		{
			public override bool CanConvert(Type objectType)
			{
				return (objectType == typeof(ConfigData));
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{

				var data = serializer.Deserialize<Dictionary<string, T>>(reader);
				ConfigData config = new ConfigData();
				for (var i = 0; i < data.Count; i++)
				{
					var pair = data.ElementAt(i);
					pair.Value.Name = pair.Key;
					data[pair.Key] = pair.Value;
				}
				config.Data = data;
				return config;
			}
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				var config = (ConfigData)value;
				var obj = JObject.FromObject(config.Data);
				obj.WriteTo(writer);
			}
		}
	}
}
