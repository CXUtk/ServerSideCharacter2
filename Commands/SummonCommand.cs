using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class SummonCommand : ModCommand
	{
		public override string Command
		{
			get { return "sm"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "召唤NPC"; }
		}

		public override string Usage
		{
			get { return "/sm [NPC ID] [数量]"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			var type = Convert.ToInt32(args[0]);
			if(type > Main.npcTexture.Length)
			{
				Main.NewText("NPC 不存在", Color.Red);
				return;
			}
			var amount = 1;
			if(args.Length > 1)
			{
				amount = Convert.ToInt32(args[1]);
			}
			MessageSender.SendSummonCommand(type, amount);
		}
	}
}
