
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


		public override void PostUpdate()
		{
			if (Main.netMode == 2)
			{
				try
				{
					ServerStarted = true;
					foreach (var p in Main.player)
					{
						if (p.active && p.whoAmI != 255)
						{
							ServerPlayer player = p.GetServerPlayer();
							player.SyncPlayerToInfo();
						}
					}
					if (Main.time % 180 < 1)
					{
						lock(ServerSideCharacter2.PlayerCollection)
						{
							foreach (var player in ServerSideCharacter2.PlayerCollection)
							{
								if (player.Value.PrototypePlayer != null)
								{
									int playerID = player.Value.PrototypePlayer.whoAmI;
									if (!player.Value.HasPassword)
									{
										player.Value.ApplyLockBuffs();
										NetMessage.SendChatMessageToClient(NetworkText.FromLiteral("Welcome! You are new to here. Please use /register <password> to register an account!"), new Color(255, 255, 30, 30), playerID);
									}
									if (player.Value.HasPassword && !player.Value.IsLogin)
									{
										player.Value.ApplyLockBuffs();
										NetMessage.SendChatMessageToClient(NetworkText.FromLiteral("Welcome! You have already created an account. Please type /login <password> to login!"), new Color(255, 255, 30, 30), playerID);
									}
								}
							}
						}
					}
					if (Main.time % ServerSideCharacter2.Config.SaveInterval < 1)
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
				ServerSideCharacter2.PlayerDoc.SavePlayersData();
				ConfigLoader.Save();
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}


	}
}
