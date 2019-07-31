using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class ItemCommand : ModCommand
	{
		public override string Command
		{
			get { return "item"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "生成物品"; }
		}

		public override string Usage
		{
			get { return "/sm [物品ID] [数量]"; }
		}
		public override void Action(CommandCaller caller, string input, string[] args)
		{
			var type = Convert.ToInt32(args[0]);
			if(type < 0 || type > Main.itemTexture.Length)
			{
				Main.NewText("物品不存在", Color.Red);
				return;
			}
			var amount = 1;
			if(args.Length > 1)
			{
				amount = Convert.ToInt32(args[1]);
			}
			MessageSender.SendItemCommand(Convert.ToInt32(args[0]));
		}
	}
}
