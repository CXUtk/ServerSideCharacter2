using Terraria.ModLoader;
using Terraria;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using ServerSideCharacter2.Utils;

namespace ServerSideCharacter2.Commands
{
	public class RefuseCommand : ModCommand
	{
		public override string Command
		{
			get { return "refuse"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "拒绝来自该玩家的一切客户端请求"; }
		}

		public override string Usage
		{
			get { return "/refuse $[GUID]"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			ServerPlayer player = null;
			if (args[0][0] == '$')
			{
				var GUID = Convert.ToInt32(args[0].Substring(1));
				player = ServerSideCharacter2.PlayerCollection.Get(GUID);
			}
			else
			{
				player = ServerSideCharacter2.PlayerCollection.Get(args[0]);
			}
			if (player != null)
			{
				try
				{
					throw new NotImplementedException();
					var s = $"玩家 {player.Name} 的连接已经被列入黑名单";
					ServerSideCharacter2.ErrorLogger.WriteToFile(s);
					CommandBoardcast.ConsoleMessage(s);
				}
				catch (Exception ex)
				{
					CommandBoardcast.ConsoleError(ex);
				}
			}
			else
			{
				CommandBoardcast.ConsoleError("该玩家不存在");
			}
		}
	}
}
