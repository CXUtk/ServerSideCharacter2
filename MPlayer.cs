﻿using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ServerSideCharacter2.Utils;
using System;
using ServerSideCharacter2.RankingSystem;
using ServerSideCharacter2.Items;

namespace ServerSideCharacter2
{
	public class MPlayer : ModPlayer
	{
		public int playerCounter = 0;

		public bool Locked = false;

		public bool GodMode = false;

		public bool Piggify = false;

		public bool ShowRank = false;

		public bool ShowCrown = false;

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
				player.statMana = player.statManaMax2;
			}
			if (Locked)
			{
				player.position = player.oldPosition;
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
			if (Main.netMode == 1 && Main.myPlayer == player.whoAmI)
			{
				var rect = player.Hitbox;
				foreach (var pair in ServerSideCharacter2.ClientRegions)
				{
					var region = pair.Value;
					var area = new Rectangle(region.Area.X * 16, region.Area.Y * 16, region.Area.Width * 16, region.Area.Height * 16);
					if (area.Intersects(rect))
					{
						if (region.PVP == JsonData.PVPMode.Always)
						{
							if (!player.hostile)
							{
								Main.LocalPlayer.hostile = true;
								NetMessage.SendData(30, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
							}
						}
						else if (region.PVP == JsonData.PVPMode.Never)
						{
							if (player.hostile)
							{
								Main.LocalPlayer.hostile = false;
								NetMessage.SendData(30, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
							}
						}
						return;
					}
				}
			}

		}

		public override void ModifyZoom(ref float zoom)
		{
			if(Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == mod.ItemType<FailedItem>())
			{
				zoom = 1000f;
			}
			base.ModifyZoom(ref zoom);
		}

		public override void ModifyScreenPosition()
		{
			base.ModifyScreenPosition();
		}

		public override void OnEnterWorld(Player player)
		{
			if (Main.netMode == 1)
			{
				player.AddBuff(mod.BuffType("Locked"), 18000);
			}
			Piggify = false;
			if (Main.netMode != 2)
			{
				ServerSideCharacter2.Instance.TurnOffAllState();
				ServerSideCharacter2.Instance.IsLoginClientSide = false;
				ServerSideCharacter2.GuiManager.ToggleState(GUI.SSCUIState.LoginWindow);
			}
			GodMode = false;

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
			
		}

		public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(Main.netMode == 1 && player.whoAmI == Main.myPlayer && player.hostile && ServerSideCharacter2.MainPlayerGroup.IsSuperAdmin)
			{
				if (item.damage > 0 && item.noMelee)
				{
					var pos = Vector2.Zero;
					var maxDis = 700f;
					foreach(var pla in Main.player)
					{
						if(pla.active && !pla.dead && pla.whoAmI != Main.myPlayer && pla.hostile && (pla.team != player.team || pla.team == 0))
						{
							var dis = Vector2.Distance(pla.Center, Main.MouseWorld);
							if(dis < maxDis)
							{
								pos = pla.Center;
								maxDis = dis;
							}
						}
					}
					if(pos != Vector2.Zero)
					{
						var ori = new Vector2(speedX, speedY);
						var speed = ori.Length();
						var diff = pos - player.Center;
						diff.Normalize();
						speedX = diff.X * speed;
						speedY = diff.Y * speed;
						player.itemRotation = diff.ToRotation();
					}
				}
			}
			return base.Shoot(item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

		
		public override bool ConsumeAmmo(Item weapon, Item ammo)
		{
			return !GodMode;
		}

		public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
		{
			if(Main.netMode == 1 && ServerSideCharacter2.MainPlayerGroup.IsSuperAdmin)
			{
				damage = (int)(damage * 0.8f);
			}
		}
		public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
		{
			if (Locked)
			{
				player.noKnockback = true;
			}
			if (GodMode)
			{
				player.aggro = -9999;
			}
			base.UpdateEquips(ref wallSpeedBuff, ref tileSpeedBuff, ref tileRangeBuff);
		}

		public override void ModifyDrawHeadLayers(List<PlayerHeadLayer> layers)
		{
			layers.Add(new PlayerHeadLayer(mod.Name, "SSC: Pig", (info) =>
			{

				var drawPlayer = info.drawPlayer;
				if (drawPlayer.GetModPlayer<MPlayer>().Piggify)
				{
					var pigTex = ServerSideCharacter2.ModTexturesTable["Pig"];
					var pos = new Vector2(drawPlayer.position.X - Main.screenPosition.X - drawPlayer.bodyFrame.Width / 2 + drawPlayer.width / 2,
						drawPlayer.position.Y - Main.screenPosition.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4f) + drawPlayer.headPosition + info.drawOrigin;
					var dd = new DrawData(pigTex, pos, null, Color.White, 0f, info.drawOrigin, info.scale * 0.5f, SpriteEffects.None, 0);
					dd.Draw(Main.spriteBatch);
				}
			}));
			base.ModifyDrawHeadLayers(layers);
		}

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{

			layers.Add(new PlayerLayer(mod.Name, "SSC: Lock", (info) =>
			{
				if (Locked && !player.dead)
				{
					var lockTex = ServerSideCharacter2.ModTexturesTable["Lock"];
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
				var modplayer = info.drawPlayer.GetModPlayer<MPlayer>();
				if (Rank >= 0 && !player.dead && (ShowRank || ShowCrown))
				{
					Texture2D tex;
					if (ShowRank)
					{
						var type = Ranking.GetRankType(info.drawPlayer.GetModPlayer<MPlayer>().Rank);
						tex = ServerSideCharacter2.ModTexturesTable[type.ToString()];
						var dd = new DrawData(tex,
							new Vector2(info.position.X + info.drawPlayer.width / 2 - Main.screenPosition.X,
							info.position.Y + info.drawPlayer.gfxOffY - 25 - Main.screenPosition.Y),
							null, Color.White, 0f, tex.Size() * 0.5f, 0.8f + Main.essScale * 0.8f, SpriteEffects.None, 0);
						Main.playerDrawData.Add(dd);
					}
					else
					{
						var spstate = Main.spriteBatch.GraphicsDevice.SamplerStates[0];
						Main.spriteBatch.End();
						tex = ServerSideCharacter2.ModTexturesTable[RankType.Crown.ToString()];
						Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
						Main.spriteBatch.Draw(tex,
							new Vector2(info.position.X + info.drawPlayer.width / 2 - Main.screenPosition.X,
							info.position.Y + info.drawPlayer.gfxOffY - 25 - Main.screenPosition.Y),
							null, Color.White, 0f, tex.Size() * 0.5f, 1.2f + Main.essScale * 0.3f, SpriteEffects.None, 0);
						Main.spriteBatch.End();
						Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, spstate, DepthStencilState.None, Main.instance.Rasterizer, null, Main.Transform);
					}

				}
			}));
			layers.Add(new PlayerLayer(mod.Name, "SSC: Pig", (info) =>
			{
				if (info.shadow != 0) return;
				if (info.drawPlayer.GetModPlayer<MPlayer>().Piggify && !player.dead)
				{
					var drawPlayer = info.drawPlayer;
					var pos = new Vector2(drawPlayer.position.X - Main.screenPosition.X - drawPlayer.bodyFrame.Width / 2 + drawPlayer.width / 2,
						drawPlayer.position.Y - Main.screenPosition.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4f) + drawPlayer.headPosition + info.headOrigin - new Vector2(0, 4);
					var pigTex = ServerSideCharacter2.ModTexturesTable["Pig"];
					var dd = new DrawData(pigTex, pos, null, Color.White, 0f, info.headOrigin, 0.6f, SpriteEffects.None, 0);
					Main.playerDrawData.Add(dd);
				}
			}));
		}

	}
}
