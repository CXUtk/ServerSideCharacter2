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
			get { return "Save player's data"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			try
			{
				Console.WriteLine(GameLanguage.GetText("savingText"));
				ServerSideCharacter2.PlayerDoc.SavePlayersData();
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

	public class ExitCommand : ModCommand
	{
		public override string Command
		{
			get { return "exit"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "退出服务器"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			try
			{
				Console.WriteLine(GameLanguage.GetText("savingText"));
				ServerSideCharacter2.PlayerDoc.SavePlayersData();
				ConfigLoader.Save();
				WorldFile.saveWorld();
				Netplay.disconnect = true;
				SocialAPI.Shutdown();
				Console.WriteLine(GameLanguage.GetText("savedText"));
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}
	}
}
