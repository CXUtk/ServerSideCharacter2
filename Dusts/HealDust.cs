using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Dusts
{
	public class HealDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity.Y = Main.rand.NextFloat(-3f, 3f);
            dust.velocity.X = Main.rand.NextFloat(-2f, 2f);
			//dust.scale *= 0.9f + Main.rand.NextFloat() * 0.2f;
		}

		public override bool MidUpdate(Dust dust)
		{
			if (!dust.noGravity)
			{
				dust.velocity.Y += 0.05f;
			}
			if (!dust.noLight)
			{
				float strength = dust.scale;
				Lighting.AddLight(dust.position, 0.1f * strength, 0.7f * strength, 0.2f * strength);
			}
			dust.scale -= 0.02f;
			if (--dust.fadeIn < 1f || dust.scale < 0.01f)
			{
				dust.active = false;
			}

			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return new Color(lightColor.R, lightColor.G, lightColor.B, 100);
		}
	}
}