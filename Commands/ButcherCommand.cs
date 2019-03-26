using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Commands
{
	public class ButcherCommand : ModCommand
	{
		public override string Command
		{
			get { return "butcher"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "斩杀所有怪物"; }
		}

		public override string Usage
		{
			get { return "/butcher"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			MessageSender.SendButcherCommand();
		}
	}
}
