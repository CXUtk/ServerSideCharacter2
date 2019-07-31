using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class RankCommand : ModCommand
	{
		public override string Command
		{
			get { return "rank"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "改变玩家段位"; }
		}

		public override string Usage
		{
			get { return "/rank [n|e] $[玩家GUID] [rank分数] 【n - 段位分 / e - 隐藏分】"; }
		}
		//static private string[] GetArgs(string[] source)
		//{
		//	string name;
		//	int amount;
		//	if (source.Length > 1 && int.TryParse(source.Last(), out amount))
		//	{
		//		name = string.Join(" ", source.Take(source.Length - 1));
		//	}
		//	else
		//	{
		//		amount = 1;
		//		name = string.Join(" ", source);
		//	}
		//	return new string[2] { name, amount.ToString() };
		//}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			ServerPlayer player = null;
			if (args.Length < 3)
			{
				Console.WriteLine(Usage);
				return;
			}
			
			if (args[1][0] == '$')
			{
				var GUID = Convert.ToInt32(args[1].Substring(1));
				player = ServerSideCharacter2.PlayerCollection.Get(GUID);
			}
			else
			{
				player = ServerSideCharacter2.PlayerCollection.Get(args[1]);
			}
			if (player != null)
			{
				try
				{
					var rank = Convert.ToInt32(args[2]);
					if (args[0] == "n")
					{
						player.IncreaseRank(rank - player.Rank);
						player.SendInfoMessage($"您的排位积分设为了 {rank}");
						CommandBoardcast.ConsoleMessage($"成功设置玩家 {player.Name} 的段位分数为 {rank}");
					}
					else
					{
						player.IncreaseElo(rank - player.EloRank);
						CommandBoardcast.ConsoleMessage($"成功设置玩家 {player.Name} 隐藏分为 {rank}");
					}
				}
				catch (Exception ex)
				{
					CommandBoardcast.ConsoleError(ex);
				}
			}
			else
			{
				CommandBoardcast.ConsoleError("该玩家不存在");
			}
		}
	}
}
