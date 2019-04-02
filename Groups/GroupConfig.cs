using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ServerSideCharacter2.Groups
{
	public class GroupConfig : DataConfig<Group>
	{
		public override string Path => "SSC/groups.json";

		public override string Message => GameLanguage.GetText("groupconfigcreate");

		public override Dictionary<string, Group> Get()
		{
			return ServerSideCharacter2.GroupManager.Groups;
		}

		public override void Receive(Dictionary<string, Group> data)
		{
			ServerSideCharacter2.GroupManager.Groups = data;
		}

		protected override void SetDefaults(ConfigData data)
		{
			data.Data = ServerSideCharacter2.GroupManager.DefaultGroups;
			ServerSideCharacter2.GroupManager.Groups = ServerSideCharacter2.GroupManager.DefaultGroups;

		}
	}
	//public class GroupConfigDatas
	//{
	//	public Dictionary<string, Group> Groups;
	//	public GroupConfigData()
	//	{
	//		Groups = new Dictionary<string, Group>();
	//	}

	//	public void SetDefaults()
	//	{
	//		Groups = ServerSideCharacter2.GroupManager.DefaultGroups;
	//	}
	//}
	//public class GroupConfigManager
	//{
	//	private readonly ConfigConverter converter = new ConfigConverter();
	//	public bool ConfigExist { get; }

	//	public Dictionary<string, Group> Groups
	//	{
	//		get
	//		{
	//			return _configData.Groups;
	//		}
	//	}
	//	private static string _configPath = "SSC/groups.json";

	//	private GroupConfigData _configData;

	//	public GroupConfigManager()
	//	{
	//		ConfigExist = File.Exists(_configPath);
	//		SetConfig();
	//	}

	//	private void SetConfig()
	//	{
	//		if (!ConfigExist)
	//		{
	//			_configData = new GroupConfigData();
	//			_configData.SetDefaults();
	//			var data = JsonConvert.SerializeObject(_configData, Formatting.Indented, converter);
	//			using (var sw = new StreamWriter(_configPath))
	//			{
	//				sw.Write(data);
	//			}
	//			CommandBoardcast.ConsoleMessage(GameLanguage.GetText("groupconfigcreate"));
	//		}
	//		else
	//		{
	//			using (var sr = new StreamReader(_configPath))
	//			{
	//				var data = sr.ReadToEnd();
	//				_configData = JsonConvert.DeserializeObject<GroupConfigData>(data, converter);

	//				//_configData.GroupType.SetupGroups(!_configData.GroupType.Groups.ContainsKey("default"), !_configData.GroupType.Groups.ContainsKey("criminal"), false, !_configData.GroupType.Groups.ContainsKey("spadmin")); //Add default, criminal and spadmin group if not exists
	//				//_configData.GroupType.Groups["spadmin"].permissions.Clear(); //clear all spadmin user created permissions
	//				//_configData.GroupType.Groups["spadmin"].permissions.Add(new PermissionInfo("all", "all commands")); //add all permissions to spadmin group
	//			}
	//		}

	//		ServerSideCharacter2.GroupManager.Groups = _configData.Groups;
	//	}

	//	public void Save()
	//	{
	//		_configData.Groups = ServerSideCharacter2.GroupManager.Groups;
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
	//		return (objectType == typeof(GroupConfigData));
	//	}

	//	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	//	{
	//		var data = serializer.Deserialize<Dictionary<string, Group>>(reader);
	//		var config = new GroupConfigData();
	//		for (var i = 0; i < data.Count; i++)
	//		{
	//			var pair = data.ElementAt(i);
	//			pair.Value.Name = pair.Key;
	//			data[pair.Key] = pair.Value;
	//		}
	//		config.Groups = data;
	//		return config;
	//	}
	//	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	//	{
	//		var config = (GroupConfigData)value;
	//		var obj = JObject.FromObject(config.Groups);
	//		obj.WriteTo(writer);
	//	}
	//}
}
