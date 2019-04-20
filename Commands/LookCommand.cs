using Terraria.ModLoader;
using Terraria;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class LookCommand : ModCommand
	{
		public override string Command
		{
			get { return "look"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "?????"; }
		}

		public override string Usage
		{
			get { return "/look [玩家GUID]"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			var who = Convert.ToInt32(args[0]);
			MessageSender.SendOfflineInventory(who);
		}
	}
}
