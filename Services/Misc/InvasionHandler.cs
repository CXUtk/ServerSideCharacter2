using Microsoft.Xna.Framework;
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

		private void SwitchInvasion(int type)
		{
			if (Main.invasionType == type)
			{
				Main.invasionSize = -1;
			}
			Console.WriteLine(Main.invasionType);
			Main.StartInvasion(type);
			NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
		}

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
								SwitchInvasion(InvasionID.SnowLegion);
								string str = $"玩家 {player.Name} 开启了雪人军团入侵";
								ServerPlayer.SendInfoMessageToAll(str);
								CommandBoardcast.ConsoleMessage(str);
								break;
							}
						case (byte)InvasionID.GoblinArmy:
							{
								SwitchInvasion(InvasionID.GoblinArmy);
								string str = $"玩家 {player.Name} 开启了哥布林入侵";
								ServerPlayer.SendInfoMessageToAll(str);
								CommandBoardcast.ConsoleMessage(str);
								break;
							}
						case (byte)InvasionID.MartianMadness:
							{
								SwitchInvasion(InvasionID.MartianMadness);
								string str = $"玩家 {player.Name} 开启了火星人入侵";

								ServerPlayer.SendInfoMessageToAll(str);
								CommandBoardcast.ConsoleMessage(str);
								break;
							}
						case (byte)InvasionID.PirateInvasion:
							{
								SwitchInvasion(InvasionID.PirateInvasion);
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
								NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
								ServerPlayer.SendInfoMessageToAll(str);
								CommandBoardcast.ConsoleMessage(str);
								break;
							}
						case 123:
							{
								if (Main.slimeRain)
								{
									Main.StopSlimeRain();
								}
								else
								{
									Main.StartSlimeRain(true);
								}
								string str = $"玩家 {player.Name} {(Main.slimeRain ? "开启" : "关闭")}了史莱姆雨";
								NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
								ServerPlayer.SendInfoMessageToAll(str);
								CommandBoardcast.ConsoleMessage(str);
								break;
							}
						case 124:
							{
								Main.eclipse ^= true;
								if (Main.eclipse)
								{
									Main.dayTime = true;
									Main.time = 0;
								}
								string str = $"玩家 {player.Name} {(Main.eclipse ? "开启" : "关闭")}了日食";
								NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
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
								NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
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
