using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Commands
{
	public class ResetPWCommand : ModCommand
	{
		public override string Command
		{
			get { return "reset"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "重置玩家的密码"; }
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
					var s = $"玩家 {player.Name} 的密码已经被重置";
					ServerSideCharacter2.ErrorLogger.WriteToFile(s);
					player.Kick("你的密码已经被重置");
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

			//{
			//	CommandBoardcast.ShowInWorldTest("正在向服务器请求在线玩家信息");
			//	MessageSender.SendRequestOnlinePlayer();
			//}

		}
	}
}
