using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.RankingSystem
{
	public class Ranking
	{
		public const int S_CHALLENGER = 2900;
		public const int S_MASTER = 2600;
		public const int S_DIAMOND = 2300;
		public const int S_PLATINUM = 2000;
		public const int S_GOLD = 1700;
		public const int S_SILVER = 1300;

		public static RankType GetRankType(int score)
		{
			if(score > S_CHALLENGER)
			{
				return RankType.Challenger;
			}
			else if(score > S_MASTER)
			{
				return RankType.Master;
			}
			else if (score > S_DIAMOND)
			{
				return RankType.Diamond;
			}
			else if (score > S_PLATINUM)
			{
				return RankType.Platinum;
			}
			else if (score > S_GOLD)
			{
				return RankType.Gold;
			}
			else if (score > S_SILVER)
			{
				return RankType.Silver;
			}
			else
			{
				return RankType.Bronze;
			}
		}

		public static Tuple<int, int> GetRankRange(RankType rankType)
		{
			switch (rankType)
			{
				case RankType.Bronze:
					return new Tuple<int, int>(0, 1299);
				case RankType.Silver:
					return new Tuple<int, int>(1300, 1699);
				case RankType.Gold:
					return new Tuple<int, int>(1700, 1999);
				case RankType.Platinum:
					return new Tuple<int, int>(2000, 2299);
				case RankType.Diamond:
					return new Tuple<int, int>(2300, 2599);
				case RankType.Master:
					return new Tuple<int, int>(2600, 2899);
				case RankType.Challenger:
					return new Tuple<int, int>(2900, 10000);
				default:
					return new Tuple<int, int>(0, 0);
			}
		}
	}
}
