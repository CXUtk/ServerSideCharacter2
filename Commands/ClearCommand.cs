using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Commands
{
	public class ClearCommand : ModCommand
	{
		public override string Command
		{
			get { return "clear"; }
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
			get { return " /clear [npc|item|proj]"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if(args[0] != "npc" && args[0] != "item" && args[0] != "proj")
			{
				Main.NewText("用法： /clear [npc|item|proj]", Color.Red);
				return;
			}
			int type = 0;
			if (args[0] == "item") type = 1;
			else if (args[0] == "proj") type = 2;
			MessageSender.SendClearCommand(type);
		}
	}
}
