using Terraria.ModLoader;
using Terraria;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using ServerSideCharacter2.Utils;

namespace ServerSideCharacter2.Commands
{
	public class GroupCommand : ModCommand
	{
		public override string Command
		{
			get { return "group"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "改变一名玩家的权限组"; }
		}

		public override string Usage
		{
			get { return "/group <player name / [$]GUID> <group name>"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			ServerPlayer player = null;
			if (args[0][0] == '$')
			{
				int GUID = Convert.ToInt32(args[0].Substring(1));
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
					player.SetGroup(args[1]);
					player.SyncGroupInfo();
					player.SendInfoMessage($"你已经被系统设置为权限组 {args[1]}");
					CommandBoardcast.ConsoleMessage("成功设置玩家" + player.Name + "为组" + args[1]);
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
