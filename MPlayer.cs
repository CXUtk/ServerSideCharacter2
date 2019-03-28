using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ServerSideCharacter2.Utils;
using System;
using ServerSideCharacter2.RankingSystem;

namespace ServerSideCharacter2
{
	public class MPlayer : ModPlayer
	{
		public int playerCounter = 0;

		public bool Locked = false;

		public bool GodMode = false;

		public int Rank = 1500;

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			MessageSender.SyncModPlayerInfo(toWho, fromWho, this);
		}

		public override void ResetEffects()
		{
			Locked = false;
		}

		public override void SetControls()
		{
			if (Locked)
			{
				player.controlJump = false;
				player.controlDown = false;
				player.controlLeft = false;
				player.controlRight = false;
				player.controlUp = false;
				player.controlUseItem = false;
				player.controlUseTile = false;
				player.controlThrow = false;
				player.controlHook = false;
				player.controlMount = false;
				player.controlInv = false;
				player.gravDir = 0f;
				player.position = player.oldPosition;
			}
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			return !GodMode;
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			return !GodMode;
		}

		public override void PreUpdate()
		{
			if (GodMode)
			{
				player.statLife = player.statLifeMax2;
			}
			if(Main.netMode == 2)
			{
				if (Rank != player.GetServerPlayer().Rank)
				{
					Rank = player.GetServerPlayer().Rank;
					MessageSender.SyncModPlayerInfo(-1, -1, this);
				}
			}

			//if (Main.myPlayer == player.whoAmI)
			//{
			//	Main.NewText(player.active);
			//}

		}

		public override void OnEnterWorld(Player player)
		{
			if (Main.netMode == 1)
			{
				// 给玩家一个很长世界的锁debuff，直到服务器解除
				player.AddBuff(mod.BuffType("Locked"), 18000);
			}
			GodMode = false;
			ServerSideCharacter2.Instance.TurnOffAllState();
			ServerSideCharacter2.Instance.IsLoginClientSide = false;
			//ServerSideCharacter2.ErrorLogger = new ErrorLogger("SSC_log.txt", false)			foreach (var item in player.bank.item)
			//{
			//	ServerSideCharacter2.ErrorLogger.WriteToFile(item.Name);
			//}
			//ServerSideCharacter2.ErrorLogger.WriteToFile("---Bank2---");
			//foreach (var item in player.bank2.item)
			//{
			//	ServerSideCharacter2.ErrorLogger.WriteToFile(item.Name);
			//}
			// ServerSideCharacter2.Instance.RelaxButton();
		}


		public override void PostUpdate()
		{
			playerCounter++;
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			if (Main.netMode == 2)
			{
				if (pvp && damageSource.SourcePlayerIndex != -1)
				{
					ServerPlayer winplayer = Main.player[damageSource.SourcePlayerIndex].GetServerPlayer();
					ServerPlayer loseplayer = player.GetServerPlayer();
					if (!loseplayer.IsLogin)
					{
						MessageSender.SendInfoMessage(winplayer.PrototypePlayer.whoAmI, "杀死没有登录的玩家不算分", Color.Yellow);
						return;
					}
					var changes = Ranking.ComputeRank(winplayer, loseplayer);
					winplayer.ChangeRank(changes.Item1);
					loseplayer.ChangeRank(changes.Item2);

					string winmsg = $"你击杀了 {loseplayer.Name} 并且获得 {changes.Item1} 点积分";
					MessageSender.SendInfoMessage(winplayer.PrototypePlayer.whoAmI, winmsg, Color.LimeGreen);

					string losemsg = $"你被 {winplayer.Name} 击杀了，为此你的积分降低了 {-changes.Item2}";
					MessageSender.SendInfoMessage(loseplayer.PrototypePlayer.whoAmI, losemsg, Color.OrangeRed);

					string servermsg = $"玩家 {winplayer.Name} (+{changes.Item1}) 击杀了 {loseplayer.Name} ((-{changes.Item2}))\n" +
						$"双方的排位积分分别为 {winplayer.Rank} 和 {loseplayer.Rank}";
					CommandBoardcast.ConsoleMessage(servermsg);
				}
			}
		}

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{

			layers.Add(new PlayerLayer(mod.Name, "SSC: Lock", (info) =>
			{
				if (Locked && !player.dead)
				{
					Texture2D lockTex = ServerSideCharacter2.ModTexturesTable["Lock"];
					var dd = new DrawData(lockTex,
						new Vector2(player.Center.X - Main.screenPosition.X,
						player.Center.Y - Main.screenPosition.Y),
						null, Color.White, 0f, lockTex.Size() * 0.5f, 0.75f + Main.essScale * 0.6f, SpriteEffects.None, 0
					);
					Main.playerDrawData.Add(dd);
				}
			}));
			layers.Add(new PlayerLayer(mod.Name, "SSC: Rank", (info) =>
			{
				if (info.shadow != 0) return;
				if (Rank >= 0 && !player.dead)
				{
					var type = Ranking.GetRankType(info.drawPlayer.GetModPlayer<MPlayer>().Rank);
					Texture2D rankTex = ServerSideCharacter2.ModTexturesTable[type.ToString()];
					DrawData dd = new DrawData(rankTex,
						new Vector2(info.position.X + info.drawPlayer.width / 2 - Main.screenPosition.X,
						info.position.Y + info.drawPlayer.gfxOffY - 25 - Main.screenPosition.Y),
						null, Color.White, 0f, rankTex.Size() * 0.5f, 0.8f + Main.essScale * 0.8f, SpriteEffects.None, 0);
					Main.playerDrawData.Add(dd);
				}
			}));
		}

	}
}
