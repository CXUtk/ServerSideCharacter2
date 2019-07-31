using Terraria.ModLoader;
using Terraria;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class LockCommand : ModCommand
	{
		public override string Command
		{
			get { return "lock"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "锁定一名玩家"; }
		}

		public override string Usage
		{
			get { return "/lock [玩家ID] [时间]"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			var who = Convert.ToInt32(args[0]);
			if (who < 0 || who > 255 || !Main.player[who].active)
			{
				Main.NewText("玩家不存在", Color.Red);
				return;
			}
			MessageSender.SendLockCommand(Main.myPlayer, who, Convert.ToInt32(args[1]));
		}
	}
}
