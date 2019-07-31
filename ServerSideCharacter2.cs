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
using ServerSideCharacter2.Mailing;
using ServerSideCharacter2.RankingSystem;
using ServerSideCharacter2.Market;

namespace ServerSideCharacter2
{
	public delegate void RankBoardEventHandler(List<SimplifiedPlayerInfo> players);
	public class ServerSideCharacter2 : Mod
	{
		public const bool DEBUGMODE = false;

		internal static ServerSideCharacter2 Instance;

		internal static Dictionary<string, Texture2D> ModTexturesTable = new Dictionary<string, Texture2D>();

		public static PlayerCollection PlayerCollection;

		internal static string APIVersion = "V0.20 Beta";

		internal static ErrorLogger ErrorLogger;

		internal static PlayersDocument PlayerDoc;

		internal static ConfigData Config { get; set; }

		internal static string ShowTooltip { get; set; }

		internal static GUIManager GuiManager;

		internal static GroupManager GroupManager;

		internal static UnionManager UnionManager;

		public static RegionManager RegionManager;

		public static MailManager MailManager;

		public static MarketManager MarketManager;

		public static Dictionary<string, Region> ClientRegions = new Dictionary<string, Region>();

		public static ToolBarServiceManager ToolBarServiceManager { get; set; }

		public bool IsLoginClientSide { get; set; }

		public static Group MainPlayerGroup;

		internal static Union ClientUnion;

		private PacketHandler _packetHandler;

		private SSCPacketHandler _sscPacketHandler;

		private bool Loaded { get; set; }

		public static int UnreadCount = 0;

		public static Point RegionUpperLeft { get; internal set; }
		public static Point RegionLowerRight { get; internal set; }

		public static RankData RankData;

		public static MatchingSystem MatchingSystem;

		internal void ChangeState(SSCUIState state)
		{
			GuiManager.ToggleState(state);
		}

		internal void TurnOffAllState()
		{
			GuiManager.TurnOffAll();
		}

		public static ServerPlayer GetPlayer(string name)
		{
			return PlayerCollection.Get(name);
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



		public override bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
		{
			try
			{
				if (PacketHandler.CheckBlockPacket(playerNumber, messageType, ref reader))
					return true;
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
			// 处理自定义消息的地方
			var type = (SSCMessageType)reader.ReadInt32();
			_sscPacketHandler.Handle(type, reader, whoAmI);
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
				Console.WriteLine("[SSC Version: " + APIVersion + "]");

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
				ClientUnion = null;
				if (DEBUGMODE)
				{
					ClientUnion = new Union("裙中世界");
					ClientUnion.Owner = "我不是裙子";
				}
			}
			else
			{
				// 生成玩家存档，这里用json文件存储玩家信息
				// 顺序一定不能错
				PlayerCollection = new PlayerCollection();
				PlayerDoc = new PlayersDocument("players.json");
				PlayerDoc.ExtractPlayersData();
				GroupManager = new GroupManager();
				GroupManager.SetGroups();
				UnionManager = new UnionManager();
				RegionManager = new RegionManager();
				MailManager = new MailManager();
				// 服务器端生成RSA私钥
				RSACrypto.GenKey();
				ConfigLoader.Load();
				MarketManager = new MarketManager();
				MatchingSystem = new MatchingSystem();
				AddUnionRegions();

			}
			Loaded = true;
			if (!Main.dedServ)
				GuiManager.SetNPCDefaults();
		}

		private void AddUnionRegions()
		{
			int width = 202;
			int height = 102;
			for (int i = 0; i < 20; i++)
			{
				Point start = new Point(55 + i * width, 800);
				if (!RegionManager.Contains($"公会领地{i + 1}"))
				{
					RegionManager.CreateNewRegion(new Rectangle(start.X + 1, start.Y + 1, width - 2, height - 2), $"公会领地{i + 1}");
				}
			}
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
