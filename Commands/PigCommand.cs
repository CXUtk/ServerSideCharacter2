﻿using Terraria.ModLoader;
using Terraria;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class PigCommand : ModCommand
	{
		public override string Command
		{
			get { return "pig"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "把一个玩家变成猪头"; }
		}

		public override string Usage
		{
			get { return "/pig [玩家ID]"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			var who = Convert.ToInt32(args[0]);
			if (who < 0 || who > 255 || !Main.player[who].active)
			{
				Main.NewText("玩家不存在", Color.Red);
				return;
			}
			MessageSender.SendPigCommand(who);
		}
	}
}
