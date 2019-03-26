using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2
{
	public class MPlayer : ModPlayer
	{
		public int playerCounter = 0;

		public bool Locked = false;

		public bool GodMode = false;

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
				//player.controlInv = false; // With this the users will not be abble to exit the server without login first (Exept by pressing ALT + F4). This is not a good thing
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

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			layers.Add(new PlayerLayer(mod.Name, "SSC: Lock", (info) =>
			{
				if (Locked)
				{
					Texture2D dizzyTex = ServerSideCharacter2.ModTexturesTable["Lock"];
					var dd = new DrawData(dizzyTex,
						new Vector2(player.Center.X - Main.screenPosition.X,
						player.Center.Y - Main.screenPosition.Y),
						null, Color.White, 0f, dizzyTex.Size() * 0.5f, 0.75f + Main.essScale * 0.6f, SpriteEffects.None, 0
					);
					Main.playerDrawData.Add(dd);
				}
			}));
		}

	}
}
