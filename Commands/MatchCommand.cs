using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class MatchCommand : ModCommand
	{
		public override string Command
		{
			get { return "match"; }
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
			get { return " /match [new|join] <活动名字>"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if (args[0] == "new")
			{
				MessageSender.SendNewMatchCommand(args[1]);
			}
			else if(args[0] == "join")
			{
				MessageSender.SendJoinMatchCommand(args[1]);
			}
		}
	}
}
