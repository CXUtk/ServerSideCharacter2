﻿using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.RankingSystem
{
	[JsonObject]
	public class RankData 
	{
		public DateTime LastRankBoardTime { get; set; }
		public DateTime RankSeasonEndTime { get; set; }
		public List<SimplifiedPlayerInfo> LastBoard { get; set; }


		public RankData()
		{
			LastRankBoardTime = new DateTime(0);
			RankSeasonEndTime = DateTime.Now.AddDays(15.0);
			LastBoard = new List<SimplifiedPlayerInfo>();
		}

		private static string path = "SSC/ranks.json";

		public static RankData Load()
		{
			try
			{
				CommandBoardcast.ConsoleMessage("加载排行榜信息");
				if (!File.Exists(path))
				{
					return new RankData();
				}
				else
				{
					RankData ret = new RankData();
					using (var sr = new StreamReader(path))
					{
						var data = sr.ReadToEnd();
						ret = JsonConvert.DeserializeObject<RankData>(data);
					}
					return ret;
				}
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
				return new RankData();
			}
		}

		public static void Save(RankData data)
		{
			var tosave = JsonConvert.SerializeObject(data, Formatting.None);
			using (var writer = new StreamWriter(path, false, Encoding.UTF8))
			{
				writer.Write(tosave);
			}
			CommandBoardcast.ConsoleMessage("排行榜保存完成");
		}
	}
}
