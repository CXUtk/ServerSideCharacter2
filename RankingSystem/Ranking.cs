using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.RankingSystem
{
	public class Ranking
	{
		public const int S_CHALLENGER = 2600;
		public const int S_MASTER = 2400;
		public const int S_DIAMOND = 2200;
		public const int S_PLATINUM = 1900;
		public const int S_GOLD = 1700;
		public const int S_SILVER = 1300;

		private static int getR(int rank)
		{
			if(rank > 2200)
			{
				return 32 + Main.rand.Next(10) - 5;
			}
			else if(rank > 1900)
			{
				return 42 + Main.rand.Next(12) - 5;
			}
			else
			{
				return 48 + Main.rand.Next(20) - 8;
			}
		}

		public static Tuple<int, int> ComputeRank(ServerPlayer win, ServerPlayer lose)
		{
			int rA = win.Rank;
			int rB = lose.Rank;
			double eA = 1.0 / (1.0 + Math.Pow(10, (rB - rA) / 400.0));
			double eB = 1.0 / (1.0 + Math.Pow(10, (rA - rB) / 400.0));
			return new Tuple<int, int>((int)(getR(rA) * (1.0 - eA)), (int)(getR(rB) * (0.0 - eB)));
		}

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
					return new Tuple<int, int>(0, 1299);
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
