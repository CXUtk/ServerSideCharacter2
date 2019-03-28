using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Commands
{
	public class ForcePVPCommand : ModCommand
	{
		public override string Command
		{
			get { return "forcepvp"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "切换强制PVP"; }
		}

		public override string Usage
		{
			get { return "/forcepvp"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			MessageSender.SendToggleForcePVP();
		}
	}
}
