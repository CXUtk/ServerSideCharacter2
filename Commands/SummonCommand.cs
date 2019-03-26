using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class SummonCommand : ModCommand
	{
		public override string Command
		{
			get { return "sm"; }
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
			get { return " /sm <npc id> [number]"; }
		}
		//static private string[] GetArgs(string[] source)
		//{
		//	string name;
		//	int amount;
		//	if (source.Length > 1 && int.TryParse(source.Last(), out amount))
		//	{
		//		name = string.Join(" ", source.Take(source.Length - 1));
		//	}
		//	else
		//	{
		//		amount = 1;
		//		name = string.Join(" ", source);
		//	}
		//	return new string[2] { name, amount.ToString() };
		//}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			int type = Convert.ToInt32(args[0]);
			if(type > Main.npcTexture.Length)
			{
				Main.NewText("NPC 不存在", Color.Red);
				return;
			}
			int amount = 1;
			if(args.Length > 1)
			{
				amount = Convert.ToInt32(args[1]);
			}
			MessageSender.SendSummonCommand(type, amount);
		}
	}
}
