using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class CoinCommand : ModCommand
	{
		public override string Command
		{
			get { return "coin"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "改变玩家的咕币数量"; }
		}

		public override string Usage
		{
			get { return "coin <add|set> [玩家名|$玩家GUID] [咕币数量]"; }
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
					var coin = Convert.ToInt32(args[2]);
                    if (args[0] == "add")
                    { player.GuCoin += coin; player.SendInfoMessage($"您的咕币数量增加了 {coin}");
                        CommandBoardcast.ConsoleMessage($"成功增加玩家 {player.Name} 的咕币数量为 {coin}");
                    }
                    else if (args[0] == "set")
                    { player.GuCoin = coin; player.SendInfoMessage($"您的咕币数量变为了 {coin}");
                        CommandBoardcast.ConsoleMessage($"成功设置玩家 {player.Name} 的咕币数量为 {coin}");
                    }
                    else
                    {
                        Console.WriteLine(Usage);
                        return;
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
