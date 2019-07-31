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
			get { return "查看玩家数据"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			try
			{
				if (args.Length == 1)
				{
                    switch (args[0])
                    {
                        case "all":
                            var info = ServerSideCharacter2.PlayerCollection.GetAllInfo();
                            var s = JsonConvert.SerializeObject(info, Formatting.Indented);
                            CommandBoardcast.ConsoleMessage(s);
                            ServerSideCharacter2.ErrorLogger.WriteToFile(s);
                            break;
                        case "elo":
                                foreach (var pair in ServerSideCharacter2.PlayerCollection)
                                {
                                    var player = pair.Value;
                                    CommandBoardcast.ConsoleMessage($"玩家 {player.Name} 的隐藏分为 {player.EloRank}");
                                }
                            break;
                        default:
                            Console.WriteLine("请指定参数：");
                            Console.WriteLine("all - 角色数据");
                            Console.WriteLine("elo - 角色隐藏分");
                            break;
                    }
				}
				else
				{
					var info = ServerSideCharacter2.PlayerCollection.GetOnlineInfo(-1);
					var s = JsonConvert.SerializeObject(info, Formatting.Indented);
					CommandBoardcast.ConsoleMessage(s);
					ServerSideCharacter2.ErrorLogger.WriteToFile(s);
				}
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
