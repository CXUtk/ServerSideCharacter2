using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

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
			get { return "/forcepvp <0|1|2> 【0 - 正常模式 / 1 - 强制关闭PVP / 2 - 强制开启PVP】"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			var mode = Convert.ToInt32(args[0]);
			if(mode < 0 || mode > 2)
			{
                Main.NewText(Usage, Color.Red);
			}
			MessageSender.SendToggleForcePVP(mode);
		}
	}
}
