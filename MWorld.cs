
using System;
using System.Linq;
using System.Threading;
using Terraria.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ServerSideCharacter2.Utils;

namespace ServerSideCharacter2
{
	public class MWorld : ModWorld
	{
		public static bool ServerStarted = false;

		internal static int[] TileMessageCD = new int[Main.maxPlayers];

		private static int Timer = 0;

		public override void PreUpdate()
		{
			if (Main.netMode == 2)
			{
				try
				{
					Timer++;
					if (Timer > 10000000) Timer = 0;
					ServerStarted = true;
					foreach (var p in Main.player)
					{
						if (p.whoAmI != 255)
						{
							if (p.active)
							{
								ServerPlayer player = p.GetServerPlayer();
								if (player.IsLogin)
									player.SyncPlayerToInfo();
							}
						}
					}
					for(int i = 0; i < Main.maxPlayers; i++)
					{
						if(TileMessageCD[i] > 0)
						{
							TileMessageCD[i]--;
						}
					}
					if (Timer % 180 < 1)
					{
						foreach(var player in Main.player)
						{
							if (player.active)
							{
								ServerPlayer serverPlayer = player.GetServerPlayer();
								int playerID = player.whoAmI;
								if (!serverPlayer.HasPassword)
								{
									serverPlayer.ApplyLockBuffs();
									NetMessage.SendChatMessageToClient(NetworkText.FromLiteral("您还没有注册，请使用登录窗口注册哦~"), new Color(255, 255, 30, 30), playerID);
								}
								else if (serverPlayer.HasPassword && !serverPlayer.IsLogin)
								{
									serverPlayer.ApplyLockBuffs();
									NetMessage.SendChatMessageToClient(NetworkText.FromLiteral("您已注册，输入密码就可以登录了！"), new Color(255, 255, 30, 30), playerID);
								}
							}
						}
						foreach (var player in ServerSideCharacter2.PlayerCollection)
						{
							if (player.Value.PrototypePlayer == null || !player.Value.PrototypePlayer.active)
							{
								player.Value.IsLogin = false;
								player.Value.SetID(-1);
							}
							if (player.Value.PrototypePlayer != null)
							{
								ServerSideCharacter2.ErrorLogger.WriteToFile(player.Key + " -> " + player.Value.PrototypePlayer.whoAmI);
							}
							else
							{
								ServerSideCharacter2.ErrorLogger.WriteToFile(player.Key + " 没有原型");
							}
						}

					}
					if (ServerSideCharacter2.Config.AutoSave && Timer % ServerSideCharacter2.Config.SaveInterval < 1)
					{
						ThreadPool.QueueUserWorkItem(Do_Save);
					}
				}
				catch (Exception ex)
				{
					CommandBoardcast.ConsoleError(ex);
					WorldFile.saveWorld();
					Netplay.disconnect = true;
					Terraria.Social.SocialAPI.Shutdown();
				}
			}
		}

		private void Do_Save(object state)
		{
			try
			{
				CommandBoardcast.ConsoleSaveInfo();
				lock (ServerSideCharacter2.PlayerCollection)
				{
					ServerSideCharacter2.PlayerDoc.SavePlayersData();
				}
				ConfigLoader.Save();
				WorldFile.saveWorld();
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}


	}
}
