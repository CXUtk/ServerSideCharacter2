using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class SpawnControlCommand1 : ModCommand
	{
		public override string Command
		{
			get { return "spawnrate"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "调整NPC生成速率"; }
		}

		public override string Usage
		{
			get { return "/spawnrate [数值]"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			int val;
			if (args.Length == 0)
			{
				val = -1;
			}
			else
			{
				val = Convert.ToInt32(args[0]);
				if (val < 0)
				{
					Main.NewText("刷怪率不能为负数", Color.Red);
					return;
				}
			}
			MessageSender.SendSpawnRate(val);
		}
	}

	public class SpawnControlCommand2 : ModCommand
	{
		public override string Command
		{
			get { return "maxspawn"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "调整NPC最大生成数量"; }
		}

		public override string Usage
		{
			get { return "/maxspawn [数值]"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			int val;
			if (args.Length == 0)
			{
				val = -1;
			}
			else
			{
				val = Convert.ToInt32(args[0]);
				if (val < 0)
				{
					Main.NewText("最大刷怪数不能为负数", Color.Red);
					return;
				}
			}
			MessageSender.SendMaxSpawnCount(val);
		}
	}
}
