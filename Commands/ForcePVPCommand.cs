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
			get { return "/forcepvp [模式]"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			var mode = Convert.ToInt32(args[0]);
			if(mode < 0 || mode > 2)
			{
				Main.NewText("不合法的模式，正常模式=0，强制不PVP=1，强制PVP=2");
			}
			MessageSender.SendToggleForcePVP(mode);
		}
	}
}
