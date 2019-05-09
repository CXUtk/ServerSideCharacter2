using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using static ServerSideCharacter2.QQAuth;
using MySql.Data.MySqlClient;

namespace ServerSideCharacter2.Commands
{
	public class ReloadCommand : ModCommand
	{
		public override string Command
		{
			get { return "reload"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "重新加载某些配置"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if (args.Length == 1)
			{
				if (args[0] == "shop")
				{
					ServerSideCharacter2.MarketManager.Load();
				}
				else if(args[0] == "")
				{

				}
			}
			else
			{

			}
			//{
			//	CommandBoardcast.ShowInWorldTest("正在向服务器请求在线玩家信息");
			//	MessageSender.SendRequestOnlinePlayer();
			//}

		}
	}
}
