﻿using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace ServerSideCharacter2.Services.Misc
{
	public class InvasionHandler : SSCCommandHandler
	{
		public override string PermissionName => "sm";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			try
			{
				// 服务器端
				if (Main.netMode == 2)
				{
					var type = reader.ReadByte();
					var p = Main.player[playerNumber];
					var player = p.GetServerPlayer();
					switch (type)
					{
						case 0:
							{
								Main.bloodMoon ^= true;
								if (Main.bloodMoon)
								{
									Main.dayTime = false;
									Main.time = 0.0;
								}
								string str = $"玩家 {player.Name} {(Main.bloodMoon ? "开启" : "关闭")}了血月";
								ServerPlayer.SendInfoMessageToAll(str);
								CommandBoardcast.ConsoleMessage(str);
								// 发送世界信息
								NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
								break;
							}
						case (byte)InvasionID.SnowLegion:
							{
								Main.StartInvasion(type);
								string str = $"玩家 {player.Name} 开启了雪人军团入侵";
								ServerPlayer.SendInfoMessageToAll(str);
								CommandBoardcast.ConsoleMessage(str);
								break;
							}
						case (byte)InvasionID.GoblinArmy:
							{
								Main.StartInvasion(type);
								string str = $"玩家 {player.Name} 开启了哥布林入侵";
								ServerPlayer.SendInfoMessageToAll(str);
								CommandBoardcast.ConsoleMessage(str);
								break;
							}
						case (byte)InvasionID.MartianMadness:
							{
								Main.StartInvasion(type);
								string str = $"玩家 {player.Name} 开启了火星人入侵";
								ServerPlayer.SendInfoMessageToAll(str);
								CommandBoardcast.ConsoleMessage(str);
								break;
							}
						case (byte)InvasionID.PirateInvasion:
							{
								Main.StartInvasion(type);
								string str = $"玩家 {player.Name} 开启了海盗入侵";
								ServerPlayer.SendInfoMessageToAll(str);
								CommandBoardcast.ConsoleMessage(str);
								break;
							}
						case 111:
							{
								Main.pumpkinMoon ^= true;
								if (Main.pumpkinMoon)
								{
									Main.dayTime = false;
									Main.time = 0.0;
								}
								Main.bloodMoon = false;
								NPC.waveKills = 0f;
								NPC.waveNumber = 1;
								string str = $"玩家 {player.Name} {(Main.pumpkinMoon ? "开启" : "关闭")}了南瓜月事件";
								ServerPlayer.SendInfoMessageToAll(str);
								CommandBoardcast.ConsoleMessage(str);
								break;
							}
						case 222:
							{
								Main.snowMoon ^= true;
								if (Main.snowMoon)
								{
									Main.dayTime = false;
									Main.time = 0.0;
								}
								Main.bloodMoon = false;
								NPC.waveKills = 0f;
								NPC.waveNumber = 1;
								string str = $"玩家 {player.Name} {(Main.pumpkinMoon ? "开启" : "关闭")}了霜月事件";
								ServerPlayer.SendInfoMessageToAll(str);
								CommandBoardcast.ConsoleMessage(str);
								break;
							}

						default:
							break;
					}
				}
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}
	}

}