using Terraria.ModLoader;
using Terraria;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class BanCommand : ModCommand
	{
		public override string Command
		{
			get { return "ban"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "把指定玩家安排了"; }
		}

		public override string Usage
		{
			get { return "/ban [玩家GUID] [原因]"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			var GUID = Convert.ToInt32(args[0]);
			MessageSender.SendBanCommand(GUID, args[1]);
		}
	}
}
