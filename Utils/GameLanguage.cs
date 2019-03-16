using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerSideCharacter2.JsonData;
using System.IO;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Utils
{
	public enum LanguageType
	{
		Chinese,
		English
	}

	public class GameLanguage
	{
		private static LanguageData _languageData;

		private static LanguageType currentLanguage;


		public static void LoadLanguage()
		{
			// 获取语言包信息，默认zh-cn
			byte[] content = ServerSideCharacter2.Instance.GetFileBytes("Language/zh-cn.json");
			string str;
			using (MemoryStream ms = new MemoryStream(content))
			{
				using(StreamReader tr = new StreamReader(ms, Encoding.UTF8))
				{
					str = tr.ReadToEnd();
				}
			}
			_languageData = JsonConvert.DeserializeObject<LanguageData>(str);
			if(_languageData == null)
			{
				throw new SSCException("Failed to read Lanaguage file");
			}
		}

		public static void SwitchLanguage(LanguageType language)
		{

		}

		public static string GetText(string name)
		{
			return _languageData.GetIfExist(name);
		}
	}
}
