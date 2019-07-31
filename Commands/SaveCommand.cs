using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Terraria.IO;
using Terraria.Social;

namespace ServerSideCharacter2.Commands
{
	public class SaveCommand : ModCommand
	{
		public override string Command
		{
			get { return "save"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "保存玩家数据"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			try
			{
				Console.WriteLine(GameLanguage.GetText("savingText"));
				ServerSideCharacter2.PlayerDoc.SavePlayersData();
				ServerSideCharacter2.MailManager.Save();
				ConfigLoader.Save();
				WorldFile.saveWorld();
				Console.WriteLine(GameLanguage.GetText("savedText"));
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}
	}

	//public class ExitCommand : ModCommand
	//{
	//	public override string Command
	//	{
	//		get { return "exit"; }
	//	}

	//	public override CommandType Type
	//	{
	//		get { return CommandType.Console; }
	//	}

	//	public override string Description
	//	{
	//		get { return "退出服务器"; }
	//	}

	//	public override void Action(CommandCaller caller, string input, string[] args)
	//	{
	//		try
	//		{
	//			Console.WriteLine(GameLanguage.GetText("savingText"));
	//			ServerSideCharacter2.PlayerDoc.SavePlayersData();
	//			ServerSideCharacter2.MailManager.Save();
	//			ConfigLoader.Save();
	//			WorldFile.saveWorld();
	//			Console.WriteLine(GameLanguage.GetText("savedText"));
	//			Netplay.disconnect = true;
	//			SocialAPI.Shutdown();

	//		}
	//		catch (Exception ex)
	//		{
	//			CommandBoardcast.ConsoleError(ex);
	//		}
	//	}
	//}
}
