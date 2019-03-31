using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class TPCommand : ModCommand
	{
		public override string Command
		{
			get { return "tp"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "传送到一名玩家身边"; }
		}

		public override string Usage
		{
			get { return " /tp <玩家ID>"; }
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
			var who = Convert.ToInt32(args[0]);
			if (who < 0 || who > 255 || !Main.player[who].active)
			{
				Main.NewText("玩家不存在", Color.Red);
				return;
			}
			MessageSender.SendTeleportCommand(who);
		}
	}
}
