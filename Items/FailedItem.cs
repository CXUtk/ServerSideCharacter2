using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Items
{
	public class FailedItem : ModItem
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
		public override bool UseItem(Player player)
		{
			//foreach(var item in player.armor)
			//{
			//	item.Prefix(PrefixID.Quick2);
			//}
			return true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unloaded Item");
			DisplayName.AddTranslation(GameCulture.Portuguese, "Item Descarregado");
		}

		public void SetUp(string fullName)
		{
			string modName = fullName.Substring(0, fullName.IndexOf('.'));
			string itemName = fullName.Substring(fullName.LastIndexOf('.') + 1);
			Console.WriteLine(modName + "   " + itemName);
			Tooltip.SetDefault("Mod: " + modName + Environment.NewLine + "Name: " + itemName);
			Tooltip.AddTranslation(GameCulture.Chinese, $"����ģ��: {modName}{Environment.NewLine}����: {itemName}");
			FullName = fullName;
		}
	}
}