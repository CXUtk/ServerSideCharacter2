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
	public class MProjectiles : GlobalProjectile
	{
		public override void SetDefaults(Projectile projectile)
		{
			if (Main.netMode == 2)
			{
				// 清除所有墓碑
				if ((projectile.type == ProjectileID.Tombstone || (projectile.type >= ProjectileID.GraveMarker && projectile.type <= 205)
					|| (projectile.type >= ProjectileID.RichGravestone1 && projectile.type <= ProjectileID.RichGravestone5)))
				{
					projectile.SetDefaults(0);
				}
			}
			if (Main.netMode == 1 && (projectile.type == ProjectileID.Tombstone || (projectile.type >= ProjectileID.GraveMarker && projectile.type <= 205)
					|| (projectile.type >= ProjectileID.RichGravestone1 && projectile.type <= ProjectileID.RichGravestone5)))
			{
				projectile.SetDefaults(0);
			}
		}


	}
}
