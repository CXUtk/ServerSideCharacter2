using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;

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
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "See online players"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			try
			{

				var info = ServerSideCharacter2.PlayerCollection.GetOnlineInfo(255);
				var s = JsonConvert.SerializeObject(info, Formatting.Indented);
				CommandBoardcast.ConsoleMessage(s);
				ServerSideCharacter2.ErrorLogger.WriteToFile(s);
				//{
				//	CommandBoardcast.ShowInWorldTest("正在向服务器请求在线玩家信息");
				//	MessageSender.SendRequestOnlinePlayer();
				//}
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}
	}
}
