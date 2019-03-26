using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Commands
{
	public class ExpertCommand : ModCommand
	{
		public override string Command
		{
			get { return "expert"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "切换地图的专家模式"; }
		}

		public override string Usage
		{
			get { return "/expert"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			MessageSender.SendToggleExpert();
		}
	}
}
