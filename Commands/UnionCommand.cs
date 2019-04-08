using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class UnionCommand : ModCommand
	{
		public override string Command
		{
			get { return "union"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "控制公会"; }
		}

		public override string Usage
		{
			get { return " /union <new|remove> [公会名字]"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if (args[0] == "new")
			{
				MessageSender.SendRegionCreate(args[1]);
			}
			else if (args[0] == "remove")
			{
				MessageSender.SendRegionRemove(args[1]);
			}
		}
	}
}
