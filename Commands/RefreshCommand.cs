using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using static ServerSideCharacter2.QQAuth;
using MySql.Data.MySqlClient;
using ServerSideCharacter2.RankingSystem;
using System.Text;
using System.IO;

namespace ServerSideCharacter2.Commands
{
	public class RefreshCommand : ModCommand
	{
		public override string Command
		{
			get { return "refresh"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "刷新服务器信息"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			try
			{
				if (args[0] == "board")
				{
					Ranking.RefreshBoard();
					var config = ServerSideCharacter2.RankData;
					StringBuilder sb = new StringBuilder();
					sb.AppendLine("玩家名字, 分数");
					foreach(var data in config.LastBoard)
					{
						sb.AppendLine(data.Name + "," + data.Rank);
					}
					using(StreamWriter sw = new StreamWriter("排行榜.csv", false, Encoding.UTF8))
					{
						sw.Write(sb.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}

			//{
			//	CommandBoardcast.ShowInWorldTest("正在向服务器请求在线玩家信息");
			//	MessageSender.SendRequestOnlinePlayer();
			//}

		}
	}
}
