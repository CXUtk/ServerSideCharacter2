using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Commands
{
	public class GodCommand : ModCommand
	{
		public override string Command
		{
			get { return "god"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "开关上帝模式"; }
		}

		public override string Usage
		{
			get { return "/god"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			var pack = ServerSideCharacter2.Instance.GetPacket();
			pack.Write((int)SSCMessageType.ToggleGodMode);
			pack.Send();
		}
	}
}
