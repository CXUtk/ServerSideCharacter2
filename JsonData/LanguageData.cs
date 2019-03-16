using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.JsonData
{
	[JsonObject]
	public class LanguageData
	{
		public Dictionary<string, string> languageDictionary;

		public LanguageData()
		{
			languageDictionary = new Dictionary<string, string>();

		}

		public string GetIfExist(string name)
		{
			if (languageDictionary.ContainsKey(name))
			{
				return languageDictionary[name];
			}
			else
			{
				throw new ArgumentException("Language name does not exist.");
			}
		}
	}
}
