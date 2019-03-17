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

namespace ServerSideCharacter2
{
	public class ServerSideCharacter2 : Mod
	{
		public static ServerSideCharacter2 Instance;

		public static Dictionary<string, Texture2D> ModTexturesTable = new Dictionary<string, Texture2D>();

		public static PlayerCollection PlayerCollection;

		public static string APIVersion = "V1.0";

		public static ErrorLogger ErrorLogger;

		public static PlayersDocument PlayerDoc;

		public static UIElement UIMouseLocker;

		public static ConfigData Config { get; set; }

		public static string ShowTooltip { get; internal set; }

		private string _authcode;

		private PacketHandler _packetHandler;

		private GUIManager _manager;

		private bool Loaded { get; set; }

		public void ChangeState()
		{
			_manager.SwitchState();
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
			_manager.Reset();
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

							byte[] data = memoryStream.ToArray();
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
				return _packetHandler.Handle(messageType, ref reader, playerNumber);
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
			return false;
		}


		//public override void HandlePacket(BinaryReader reader, int whoAmI)
		//{
		//	try
		//	{
		//		_packetHandler.DispatchPacket(reader, whoAmI);
		//	}
		//	catch(Exception ex)
		//	{
		//		CommandBoardcast.ConsoleError(ex);
		//	}
		//}

		public override void PostSetupContent()
		{
			if (Loaded) return;
			//_messageChecker = new MessageChecker();
			_packetHandler = new PacketHandler();
			if (!Main.dedServ)
			{
				ResourceLoader.LoadAll();
				_manager = new GUIManager(this);
			}
			else
			{
				PlayerCollection = new PlayerCollection();
				PlayerDoc = new PlayersDocument("players.json");
				PlayerDoc.ExtractPlayersData();
				ConfigLoader.Load();
			}
			Loaded = true;
		}

		public override void Load()
		{
			Instance = this;
			if (Main.dedServ)
			{
				Main.ServerSideCharacter = true;
				ErrorLogger = new ErrorLogger("SSC-Log.txt", false);
				Console.WriteLine("[ServerSideCharacter Mod, Author: DXTsT	Version: " + APIVersion + "]");
				RSACrypto.GenKey();
			}
			GameLanguage.LoadLanguage();
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if(MouseTextIndex != -1)
			{
				layers.Insert(MouseTextIndex, new SSCLayer(_manager));
				layers.Insert(MouseTextIndex + 1, new GameInterfaceLayer("SSC: Tooltip", InterfaceScaleType.UI));
			}
			else
			{
				throw new SSCException("Unable to add UI interface to the game!");
			}
		}

	}
}
