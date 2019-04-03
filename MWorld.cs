
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
using ServerSideCharacter2.Matches;

namespace ServerSideCharacter2
{
	public class MWorld : ModWorld
	{
		public static bool ServerStarted = false;

		internal static int[] TileMessageCD = new int[Main.maxPlayers];

		private static int _timer = 0;

		public override void PreUpdate()
		{
			if (Main.netMode != 2) return;
			try
			{
				_timer++;
				if (_timer > 10000000) _timer = 0;
				ServerStarted = true;
				foreach (var p in Main.player)
				{
					if (p.whoAmI == 255) continue;
					if (!p.active) continue;
					var player = p.GetServerPlayer();
					if (player.IsLogin)
						player.SyncPlayerToInfo();
					UpdateRegion(p);
				}
				foreach (var player in ServerSideCharacter2.PlayerCollection)
				{
					if (player.Value.PrototypePlayer == null || !player.Value.PrototypePlayer.active)
					{
						player.Value.IsLogin = false;
						player.Value.SetID(-1);
					}
				}

				for (var i = 0; i < Main.maxPlayers; i++)
				{
					if(TileMessageCD[i] > 0)
					{
						TileMessageCD[i]--;
					}
				}
				ServerSideCharacter2.MatchingSystem.Run();
				if (_timer % 300 < 1)
				{
					foreach(var player in Main.player)
					{
						if (!player.active) continue;
						var serverPlayer = player.GetServerPlayer();
						var playerID = player.whoAmI;
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
				if (ServerSideCharacter2.Config.AutoSave && _timer % ServerSideCharacter2.Config.SaveInterval < 1)
				{
					ThreadPool.QueueUserWorkItem(Do_Save);
				}
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}

		private void UpdateRegion(Player player)
		{
			var splayer = player.GetServerPlayer();
			foreach (var pair in ServerSideCharacter2.RegionManager.Regions)
			{
				var region = pair.Value;
				var rect = new Rectangle(region.Area.X * 16, region.Area.Y * 16, region.Area.Width * 16, region.Area.Height * 16);
				if (player.Hitbox.Intersects(rect))
				{
					if (splayer.InRegion && splayer.CurrentRegion.Equals(region)) return;
					splayer.SetCurRegion(region);
					splayer.SendInfoMessage(region.WelcomeInfo());
					return;
				}
			}
			splayer.SetCurRegion(null);
			splayer.CheckPVP();
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
