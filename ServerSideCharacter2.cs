#define DEBUGMODE

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Social;
using ServerSideCharacter2.Network;
using ServerSideCharacter2.Utils;
using ServerSideCharacter2.GUI;
using Newtonsoft.Json;
using Terraria.UI;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Core;
using ServerSideCharacter2.Crypto;
using System.Windows.Forms;
using ServerSideCharacter2.GUI.UI;
using ServerSideCharacter2.Groups;
using ServerSideCharacter2.Unions;
using System.Security.Cryptography;
using ServerSideCharacter2.Regions;
using ServerSideCharacter2.GUI.UI.Component;
using ServerSideCharacter2.Items;
using ServerSideCharacter2.Matches;

namespace ServerSideCharacter2
{
	public class ServerSideCharacter2 : Mod
	{
		public const bool DEBUGMODE = true;

		internal static ServerSideCharacter2 Instance;

		internal static Dictionary<string, Texture2D> ModTexturesTable = new Dictionary<string, Texture2D>();

		internal static PlayerCollection PlayerCollection;

		internal static string APIVersion = "V0.14 正式";

		internal static ErrorLogger ErrorLogger;

		internal static PlayersDocument PlayerDoc;

		internal static ConfigData Config { get; set; }

		internal static string ShowTooltip { get; set; }

		internal static GUIManager GuiManager;

		internal static GroupManager GroupManager;

		internal static UnionManager UnionManager;

		public static RegionManager RegionManager;

		public static Dictionary<string, Region> ClientRegions = new Dictionary<string, Region>();

		public static ToolBarServiceManager ToolBarServiceManager { get; set; }

		public bool IsLoginClientSide { get; set; }

		internal static Group MainPlayerGroup;

		private PacketHandler _packetHandler;

		private SSCPacketHandler _sscPacketHandler;

		private bool Loaded { get; set; }

		public static Point RegionUpperLeft { get; internal set; }
		public static Point RegionLowerRight { get; internal set; }

		public static MatchingSystem MatchingSystem;

		internal void ChangeState(SSCUIState state)
		{
			GuiManager.ToggleState(state);
		}

		internal void TurnOffAllState()
		{
			GuiManager.TurnOffAll();
		}

		public ServerSideCharacter2()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadSounds = true,
				AutoloadGores = true
			};

		}

		public override void PreSaveAndQuit()
		{
		}

		public override bool HijackSendData(int whoAmI, int msgType, int remoteClient, int ignoreClient, NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7)
		{
			try
			{
				switch (msgType)
				{
					case MessageID.WorldData:
						{
							if (Main.netMode != 2) // we will not process this message in client-side
							{
								break;
							}

							var memoryStream = new MemoryStream();
							var binaryWriter = new BinaryWriter(memoryStream);
							var position = binaryWriter.BaseStream.Position;
							binaryWriter.BaseStream.Position += 2L;
							binaryWriter.Write(MessageID.WorldData);

							PacketModifier.ModifyWorldData(ref binaryWriter);

							var currentPosition = (int)binaryWriter.BaseStream.Position;
							binaryWriter.BaseStream.Position = position;
							binaryWriter.Write((short)currentPosition);
							binaryWriter.BaseStream.Position = currentPosition;

							var data = memoryStream.ToArray();
							binaryWriter.Close();

							// Resend the packet
							if (remoteClient == -1)
							{
								for (var index = 0; index < 256; index++)
								{
									if (index != ignoreClient && (NetMessage.buffer[index].broadcast || Netplay.Clients[index].State >= 3 && msgType == 10) && Netplay.Clients[index].IsConnected())
									{
										NetMessage.buffer[index].spamCount++;
										Main.txMsg++;
										Main.txData += currentPosition;
										Main.txMsgType[msgType]++;
										Main.txDataType[msgType] += currentPosition;
										Netplay.Clients[index].Socket.AsyncSend(data, 0, data.Length,
											Netplay.Clients[index].ServerWriteCallBack);
									}
								}
							}
							else
							{
								Netplay.Clients[remoteClient].Socket.AsyncSend(data, 0, data.Length,
									Netplay.Clients[remoteClient].ServerWriteCallBack);

							}

							return true;
						}
					default:
						return false;
				}
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}

			return false;
		}


		public override bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
		{
			try
			{
				// 处理原版消息的地方
				return _packetHandler.Handle(messageType, ref reader, playerNumber);
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
			return false;
		}


		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			try
			{
				// 处理自定义消息的地方
				var type = (SSCMessageType)reader.ReadInt32();
				_sscPacketHandler.Handle(type, reader, whoAmI);
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}
		public override void Unload()
		{
			base.Unload();
			//if (!Main.dedServ)
			//{
			//	MethodSwapper.SwapMethods();
			//}
		}

		public override void Load()
		{
			Instance = this;
			if (Main.dedServ)
			{
				Main.ServerSideCharacter = true;
				// 错误记录日志
				ErrorLogger = new ErrorLogger("SSC-Log.txt", false);
				Console.WriteLine("[ServerSideCharacter Mod, Author: DXTsT	Version: " + APIVersion + "]");


				var sBuilder = new StringBuilder();
				if (System.IO.File.Exists("pw.t"))
				{
					string auth;
					using (var fs = System.IO.File.OpenRead("pw.t"))
					{
						using (var sr = new StreamReader(fs))
						{
							auth = sr.ReadToEnd();
						}
					}

					var data = Encoding.UTF8.GetBytes(auth);
					SHA1 sha = new SHA1CryptoServiceProvider();
					// This is one implementation of the abstract class SHA1.
					sha.ComputeHash(data);
					foreach (var d in data)
					{
						sBuilder.Append(d.ToString("x2"));
					}
				}
				else
				{
					if (!sBuilder.ToString().Equals("44585473545a515a43323031393833307a7a51"))
					{
						CommandBoardcast.ConsoleError("检测到MOD被非法盗取，正在终止加载！");
						while (true) ;
						throw new InsufficientMemoryException();
					}
				}

			}
			GameLanguage.LoadLanguage();
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			var cursorIndex = Math.Max(0, layers.FindIndex((GameInterfaceLayer layer) => layer.Name == "Vanilla: Cursor"));
			var MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if(MouseTextIndex != -1)
			{
				layers.Insert(MouseTextIndex, new SSCLayer(GuiManager));
				layers.Add(new TooltipLayer("SSC: Tooltip", InterfaceScaleType.UI));
				layers.Insert(0, new RegionLayer("SSC: Region", InterfaceScaleType.None));
			}
			else
			{
				throw new SSCException("Unable to add UI interface to the game!");
			}
		}

		public override void UpdateUI(GameTime gameTime)
		{
			GuiManager.UpdateUI(gameTime);
			base.UpdateUI(gameTime);
		}

		public override void AddRecipes()
		{
			base.AddRecipes();
			if (Loaded) return;
			//_messageChecker = new MessageChecker();
			ShowTooltip = "";
			_packetHandler = new PacketHandler();
			_sscPacketHandler = new SSCPacketHandler();
			if (!Main.dedServ)
			{
				// 加载资源只有在非服务器端才会执行
				// MethodSwapper.SwapMethods();
				MainPlayerGroup = new Group("default");
				ToolBarServiceManager = new ToolBarServiceManager();
				ResourceLoader.LoadAll();
				GuiManager = new GUIManager(this);
				IsLoginClientSide = false;
			}
			else
			{
				// 生成玩家存档，这里用json文件存储玩家信息
				PlayerCollection = new PlayerCollection();
				PlayerDoc = new PlayersDocument("players.json");
				PlayerDoc.ExtractPlayersData();
				GroupManager = new GroupManager();
				GroupManager.SetGroups();
				UnionManager = new UnionManager();
				RegionManager = new RegionManager();
				// 服务器端生成RSA私钥
				RSACrypto.GenKey();
				ConfigLoader.Load();
				MatchingSystem = new MatchingSystem();
			}
			Loaded = true;
			if (!Main.dedServ)
				GuiManager.SetNPCDefaults();
		}


		public void ShowMessage(string msg, int time, Color color)
		{
			GuiManager.ShowMessage(msg, time, color);
		}


		public void RelaxButton()
		{
			GuiManager.RelaxGUI();
		}

	}
}
