using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ID;

namespace ServerSideCharacter2.Items
{
	public class RegionItem : ModItem
	{
		public string FullName;

		public override void SetDefaults()
		{
			item.height = 32;
			item.width = 32;
			item.rare = 10;
			item.expert = true;
			item.value = 0;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 4;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Region Item");

			DisplayName.AddTranslation(GameCulture.Chinese, "»¶µÿŒÔ∆∑");

		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool UseItem(Player player)
		{
			foreach(var item in player.armor)
			{
				item.Prefix(PrefixID.Quick2); 
			}
			Main.dayTime ^= true;
			if (player.altFunctionUse != 2 && Main.mouseLeftRelease)
			{
				var tilePos = new Vector2(Player.tileTargetX, Player.tileTargetY);
				ServerSideCharacter2.TilePos1 = tilePos;
				Main.NewText($"Selected tile positon 1 at ({tilePos.X}, {tilePos.Y})");
			}
			else if (player.altFunctionUse == 2 && Main.mouseRightRelease)
			{
				var tilePos = new Vector2(Player.tileTargetX, Player.tileTargetY);
				ServerSideCharacter2.TilePos2 = tilePos;
				Main.NewText($"Selected tile positon 2 at ({tilePos.X}, {tilePos.Y})");
			}
			return true;
		}

	}
}