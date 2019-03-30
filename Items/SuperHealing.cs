using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Linq;
using ServerSideCharacter2.Dusts;

namespace ServerSideCharacter2.Items
{
    public class SuperHealing : ModProjectile
    {
        public int TargetPlayer { get { return (int)projectile.ai[0]; } set { projectile.ai[0] = value; } }

		public override void SetDefaults()
		{
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
        }
		public override void AI()
		{
			projectile.frameCounter++;
			Player p = Main.player[projectile.owner];
			if (projectile.owner == Main.myPlayer)
			{
				Vector2 diff = Main.MouseWorld - p.Center;
				diff.Normalize();
				projectile.velocity = diff;
				projectile.direction = Main.MouseWorld.X < projectile.Center.X ? -1 : 1;
				Player player = FindCloestPlayer(Main.MouseWorld, 300, pla => pla.whoAmI != Main.myPlayer);
				if(player != null && (player.team == p.team || (player.team != p.team && !player.hostile)))
				{
					TargetPlayer = player.whoAmI;
				}
				else
				{
					TargetPlayer = -1;
				}
				projectile.netUpdate = true;
			}
			SetProjPos(projectile, 1.57f, 1);
			if (!p.channel || (projectile.frameCounter % 6 == 0 && !p.CheckMana(1, true)))
			{
				projectile.Kill();
				p.reuseDelay = 4;
			}

			Player target = TargetPlayer == -1 ? null : Main.player[TargetPlayer];
			Vector2 dustPos = projectile.position + projectile.velocity * 55;
			projectile.localAI[0] = (projectile.localAI[0] + 1) % 60;
			Vector2 dustVel = projectile.velocity;
			for(float i = 0; i < 800f; i += 8f)
			{
				dustPos += dustVel * 5f;
				Dust d = Dust.NewDustDirect(dustPos, 1, 1, mod.DustType<HealDust>(), 0, 0, 100, default(Color), 1.2f);
				d.noGravity = true;
				d.velocity *= 0;
				d.fadeIn = 2f;
				if(i % projectile.localAI[0] < 1)
				{
					d.scale = 2.2f;
				}
				

				float dis; 
				if (i > 100f && TargetPlayer != -1 && 
					(dis = Vector2.Distance(target.Center, dustPos)) < 300)
				{
					Vector2 diff = target.Center - dustPos;
					if (diff.LengthSquared() < 100)
					{
						if (projectile.frameCounter % 20 == 0)
						{
							HealPlayer(target, projectile.damage);
						}
						break;
					}
					else
					{
						diff.Normalize();
						float factor = dis / 300f;
						float factor2 = MathHelper.Lerp(3f, 20f, factor);
						dustVel = (dustVel * factor2 + diff) / (factor2 + 1);
					}
				}
			}
		}
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public static Player FindCloestPlayer(Vector2 position, float maxDistance, Func<Player, bool> predicate)
		{
			float maxDis = maxDistance;
			Player res = null;
			foreach (var player in Main.player.Where(pla => pla.active && !pla.dead && predicate(pla)))
			{
				float dis = Vector2.Distance(position, player.Center);
				if (dis < maxDis)
				{
					maxDis = dis;
					res = player;
				}
			}
			return res;
		}

		public static void HealPlayer(Player player, int amount)
		{
			player.statLife += amount;
			if (Main.myPlayer == player.whoAmI)
			{
				player.HealEffect(amount);
			}
			if (player.statLife > player.statLifeMax2)
			{
				player.statLife = player.statLifeMax2;
			}
		}

		public static void SetProjPos(Projectile projectile, float r, float distance)
		{
			Player player = Main.player[projectile.owner];
			projectile.position = player.RotatedRelativePoint(player.MountedCenter, true) - projectile.Size / 2f + projectile.velocity * distance;
			projectile.rotation = projectile.velocity.ToRotation() + r;
			player.ChangeDir(projectile.direction);
			projectile.spriteDirection = projectile.direction;
			projectile.timeLeft = 2;
			player.heldProj = projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * projectile.direction,
				projectile.velocity.X * projectile.direction);
		}

	}
}
