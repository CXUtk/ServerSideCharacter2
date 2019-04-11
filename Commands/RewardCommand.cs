using Terraria.ModLoader;
using Terraria;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using ServerSideCharacter2.Utils;
using ServerSideCharacter2.Mailing;
using Terraria.ID;

namespace ServerSideCharacter2.Commands
{
	public class RewardCommand : ModCommand
	{
		public override string Command
		{
			get { return "reward"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "给一名玩家奖赏"; }
		}

		public override string Usage
		{
			get { return "/reward <player ID>"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			int who = Convert.ToInt32(args[0]);
			if (who < 0 || who > 255 || !Main.player[who].active)
			{
				Console.WriteLine("该玩家不存在");
				return;
			}
			var player = Main.player[who].GetServerPlayer();
			Item item = new Item();
			item.SetDefaults(ItemID.LifeCrystal);
			item.stack = 5;
			ServerSideCharacter2.MailManager.ServerSendMail(player, "系统奖励", "这是系统给你的奖励",
				new System.Collections.Generic.List<Item>() { item });
			CommandBoardcast.ConsoleMessage($"邮件已经发送给玩家 {player.Name}");
		}
	}
}
