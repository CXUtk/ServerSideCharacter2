using Microsoft.Xna.Framework;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.RankingSystem
{
	public class Ranking
	{
		public const int S_CHALLENGER = 2400;
		public const int S_MASTER = 2200;
		public const int S_DIAMOND = 2000;
		public const int S_PLATINUM = 1850;
		public const int S_GOLD = 1700;
		public const int S_SILVER = 1300;
		public const int RANK_BOARD_PLAYER_MAX = 50;
		public const int RANK_SEASON_INTERVAL_DAY = 15;
		public static event RankBoardEventHandler OnSeasonEnd;


		private static List<RankInfo2> SelectTops()
		{
			List<RankInfo2> ranks = new List<RankInfo2>();
			foreach (var pair in ServerSideCharacter2.PlayerCollection)
			{
				if(pair.Value.HasPassword)
					ranks.Add(new RankInfo2(pair.Value.Name, pair.Value.Rank));
			}
			ranks.Sort();

			List<RankInfo2> res = new List<RankInfo2>();
			for (int i = 0; i < ranks.Count; i++)
			{
				if (i == RANK_BOARD_PLAYER_MAX) break;
				res.Add(ranks[i]);
			}
			return res;
		}

		public static void CheckRankBoard()
		{
			var config = ServerSideCharacter2.RankData;
			if (config.LastRankBoardTime.Day != DateTime.Now.Day)
			{
				config.LastBoard = SelectTops();
				config.LastRankBoardTime = DateTime.Now;
				CommandBoardcast.ConsoleMessage("每日排行榜更新完成");
			}
			if (config.RankSeasonEndTime < DateTime.Now)
			{
				config.RankSeasonEndTime = DateTime.Now.AddDays(RANK_SEASON_INTERVAL_DAY);
				List<SimplifiedPlayerInfo> playerInfos = new List<SimplifiedPlayerInfo>();
				var list = SelectTops();
				foreach (var player in list)
				{
					playerInfos.Add(ServerSideCharacter2.PlayerCollection.Get(player.Name).GetSimplified(-1));
				}
				config.LastBoard = SelectTops();
				config.LastRankBoardTime = DateTime.Now;
				OnSeasonEnd?.Invoke(playerInfos);
				CommandBoardcast.ConsoleMessage("赛季已经结束");
			}
		}


		//public static Tuple<int, int> ComputeRank(ServerPlayer win, ServerPlayer lose)
		//{
		//	var rA = win.EloRank;
		//	var rB = lose.EloRank;
		//	var eA = 1.0 / (1.0 + Math.Pow(10, (rB - rA) / 400.0));
		//	var eB = 1.0 / (1.0 + Math.Pow(10, (rA - rB) / 400.0));
		//	return new Tuple<int, int>((int)(getR(rA) * (1.0 - eA)), (int)(getR(rB) * (0.0 - eB)));
		//}

		public static RankType GetRankType(int score)
		{
			if(score >= S_CHALLENGER)
			{
				return RankType.Challenger;
			}
			else if(score >= S_MASTER)
			{
				return RankType.Master;
			}
			else if (score >= S_DIAMOND)
			{
				return RankType.Diamond;
			}
			else if (score >= S_PLATINUM)
			{
				return RankType.Platinum;
			}
			else if (score >= S_GOLD)
			{
				return RankType.Gold;
			}
			else if (score >= S_SILVER)
			{
				return RankType.Silver;
			}
			else
			{
				return RankType.Bronze;
			}
		}

		public static string GetName(RankType type)
		{
			switch (type)
			{
				case RankType.Bronze:
					return "青铜";
				case RankType.Silver:
					return "白银";
				case RankType.Gold:
					return "黄金";
				case RankType.Platinum:
					return "白金";
				case RankType.Diamond:
					return "钻石";
				case RankType.Master:
					return "大师";
				case RankType.Challenger:
					return "王者";
				case RankType.Crown:
					return "VIP";
				default:
					return "ERROR";
			}
		}

		public static Tuple<int, int> GetRankRange(RankType rankType)
		{
			switch (rankType)
			{
				case RankType.Bronze:
					return new Tuple<int, int>(0, S_SILVER - 1);
				case RankType.Silver:
					return new Tuple<int, int>(S_SILVER, S_GOLD - 1);
				case RankType.Gold:
					return new Tuple<int, int>(S_GOLD, S_PLATINUM - 1);
				case RankType.Platinum:
					return new Tuple<int, int>(S_PLATINUM, S_DIAMOND - 1);
				case RankType.Diamond:
					return new Tuple<int, int>(S_DIAMOND, S_MASTER - 1);
				case RankType.Master:
					return new Tuple<int, int>(S_MASTER, S_CHALLENGER - 1);
				case RankType.Challenger:
					return new Tuple<int, int>(S_CHALLENGER, 10000);
				default:
					return new Tuple<int, int>(0, 0);
			}
		}
	}
}
