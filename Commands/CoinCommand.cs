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
			get { return " /coin $[玩家GUID] <咕币数量>"; }
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
			if (args.Length < 2)
			{
				Console.WriteLine("用法： /coin $[玩家GUID] <数量>");
				return;
			}
			
			if (args[0][0] == '$')
			{
				var GUID = Convert.ToInt32(args[0].Substring(1));
				player = ServerSideCharacter2.PlayerCollection.Get(GUID);
			}
			else
			{
				player = ServerSideCharacter2.PlayerCollection.Get(args[0]);
			}
			if (player != null)
			{
				try
				{
					var coin = Convert.ToInt32(args[1]);
					player.GuCoin = coin;
					player.SendInfoMessage($"系统将你的咕币数量设为了 {coin}");
					CommandBoardcast.ConsoleMessage($"成功设置玩家 {player.Name} 的咕币数量为 {coin}");
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
