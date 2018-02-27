﻿using System;

namespace ServerSideCharacter2.Utils
{
	public static class CommandBoardcast
	{

		public static void ConsoleSaveInfo()
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			string info = string.Format("[SSC {0}] Saved all player data", ServerSideCharacter2.APIVersion);
			Console.WriteLine(info);
			LogInfo(info);
			Console.ResetColor();
		}

		public static void ConsoleSavePlayer(ServerPlayer p)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			string info = string.Format("[SSC {0}] Saved {1}'s data", ServerSideCharacter2.APIVersion, p.Name);
			Console.WriteLine(info);
			LogInfo(info);
			Console.ResetColor();
		}
		public static void ConsoleNormalText(string msg)
		{
			string info = string.Format("[SSC {0}] {1}", ServerSideCharacter2.APIVersion, msg);
			Console.WriteLine(info);
			LogInfo(info);
		}
		public static void ConsoleMessage(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			string info = string.Format("[SSC {0}] {1}", ServerSideCharacter2.APIVersion, msg);
			Console.WriteLine(info);
			LogInfo(info);
			Console.ResetColor();
		}
		public static void ConsoleError(Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			string info = string.Format("[SSC {0}] {1}", ServerSideCharacter2.APIVersion, ex);
			Console.WriteLine(info);
			LogInfo(info);
			Console.ResetColor();
		}
		public static void ConsoleError(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			string info = string.Format("[SSC {0}] {1}", ServerSideCharacter2.APIVersion, msg);
			Console.WriteLine(info);
			LogInfo(info);
			Console.ResetColor();
		}
		public static void LogInfo(string msg)
		{
			ServerSideCharacter2.ErrorLogger.WriteToFile(msg);
		}

		//public static void SendErrorToPlayer(int plr, string msg)
		//{
		//	NetMessage.SendData(MessageID.ChatText, plr, -1,
		//					msg,
		//					255, 255, 20);
		//}
		//public static void SendInfoToPlayer(int plr, string msg)
		//{
		//	NetMessage.SendData(MessageID.ChatText, plr, -1,
		//					msg,
		//					255, 255, 255);
		//}
		//public static void SendSuccessToPlayer(int plr, string msg)
		//{
		//	NetMessage.SendData(MessageID.ChatText, plr, -1,
		//					msg,
		//					255, 50, 255, 50);
		//}
		//public static void SendInfoToAll(string msg)
		//{
		//	NetMessage.SendData(MessageID.ChatText, -1, -1,
		//					msg,
		//					255, 255, 255);
		//}
	}
}
