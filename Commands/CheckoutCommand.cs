using Terraria.ModLoader;
using Terraria;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using ServerSideCharacter2.Utils;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.RankingSystem;

namespace ServerSideCharacter2.Commands
{
	public class CheckoutCommand : ModCommand
	{
		public override string Command
		{
			get { return "checkout"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "结算当前赛季的奖励"; }
		}

		public override string Usage
		{
			get { return "/checkout"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if (args.Length == 0)
			{
				Ranking.CheckOut();
			}
		}
	}
}
