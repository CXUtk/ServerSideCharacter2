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
			get { return "给所有玩家奖赏"; }
		}

		public override string Usage
		{
			get { return "/reward"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			//int who = Convert.ToInt32(args[0]);
			//if (who < 0 || who > 255 || !Main.player[who].active)
			//{
			//	Console.WriteLine("该玩家不存在");
			//	return;
			//}
			if (args.Length > 0)
			{
				var target = ServerSideCharacter2.PlayerCollection.Get(Convert.ToInt32(args[0]));
				if(target == null)
				{
					Console.WriteLine("该玩家不存在");
					return;
				}
				else
				{
					Item item = new Item();
					item.SetDefaults(ItemID.GoldCoin);
					item.stack = 5;
					Item item2 = new Item();
					item2.SetDefaults(ItemID.LifeCrystal);
					item2.stack = 5;
					ServerSideCharacter2.MailManager.ServerSendMail(target, "系统奖励", "这是系统给你的奖励，感谢您支持蒸汽之城服务器，有什么建议都可以在群里提出来哦~",
						new System.Collections.Generic.List<Item>() { item, item2 });
					CommandBoardcast.ConsoleMessage($"奖励邮件已经发送给玩家 {target.Name}");
				}
			}
			else
			{
				foreach (var pair in ServerSideCharacter2.PlayerCollection)
				{
					var player = pair.Value;
					Item item = new Item();
					item.SetDefaults(ItemID.GoldCoin);
					item.stack = 5;
					Item item2 = new Item();
					item2.SetDefaults(ItemID.LifeCrystal);
					item2.stack = 5;
					ServerSideCharacter2.MailManager.ServerSendMail(player, "系统奖励", "这是系统给你的奖励，感谢您支持蒸汽之城服务器，有什么建议都可以在群里提出来哦~",
						new System.Collections.Generic.List<Item>() { item, item2 });
					CommandBoardcast.ConsoleMessage($"奖励邮件已经发送给玩家 {player.Name}");
				}
			}
		}
	}
}
