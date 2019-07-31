using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace ServerSideCharacter2.Commands
{
	public class InvasionCommand : ModCommand
	{
		public override string Command
		{
			get { return "invasion"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "召唤事件"; }
		}

		public override string Usage
		{
            get
            {
                return "/invasion [blood|snowman|goblin|ufo|pirate|pumpkin|frost|slime]";
            }
        }

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if(args.Length == 0)
			{
				Main.NewText(Usage, Color.Red);
				return;
			}
			if (args[0] == "blood")
			{
				MessageSender.SendInvasion(0);
			}
			else if (args[0] == "snowman")
			{
				MessageSender.SendInvasion(InvasionID.SnowLegion);
			}
			else if (args[0] == "goblin")
			{
				MessageSender.SendInvasion(InvasionID.GoblinArmy);
			}
			else if(args[0] == "ufo")
			{
				MessageSender.SendInvasion(InvasionID.MartianMadness);
			}
			else if (args[0] == "pirate")
			{
				MessageSender.SendInvasion(InvasionID.PirateInvasion);
			}
			else if (args[0] == "pumpkin")
			{
				MessageSender.SendInvasion(111);
			}
			else if (args[0] == "frost")
			{
				MessageSender.SendInvasion(222);
			}
			else if(args[0] == "slime")
			{
				MessageSender.SendInvasion(123);
			}
			else if (args[0] == "eclipse")
			{
				MessageSender.SendInvasion(124);
			}
			else
			{
				Main.NewText(Usage, Color.Red);
				return;
			}
		}
	}
}
