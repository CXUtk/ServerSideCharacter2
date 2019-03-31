using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Linq;

namespace ServerSideCharacter2.Items
{
    public class HealingStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("高级治疗法杖");
			Tooltip.SetDefault("射出追踪队友的治疗射线，持续为队友加血");
		}
		public override void SetDefaults()
		{
			item.rare = 7;
			item.useTime = 24;
			item.useAnimation = 24;
			item.autoReuse = false;
			item.channel = true;
			item.damage = 15;
			item.UseSound = SoundID.Item44;
			item.mana = 10;
			Item.staff[item.type] = true;
			item.shoot = mod.ProjectileType("SuperHealing");
			item.shootSpeed = 13;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.noMelee = true;
			item.useStyle = 5;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			var tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null)
			{
				var splitText = tt.text.Split(' ');
				var damageValue = splitText.First();
				tt.text = damageValue + " 治疗效果";
				tt.overrideColor = Color.LimeGreen;
			}
		}
	}
}
