using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;

namespace ServerSideCharacter2.Commands
{
	public class OnlinePlayerCommand : ModCommand
	{
		public override string Command
		{
			get { return "see"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "See online players"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			try
			{
				if(Main.netMode != 1)
				{
					Main.NewText("仅在多人模式下有用");
				}
				else
				{
					CommandBoardcast.ShowInWorldTest("正在向服务器请求在线玩家信息");
					MessageSender.SendRequestOnlinePlayer();
				}
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}
	}
}
