using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Buffs
{
	public class Locked : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Locked");
			Description.SetDefault("You are locked by the server");

			DisplayName.AddTranslation(GameCulture.Chinese, "锁");
			Description.AddTranslation(GameCulture.Chinese, "你被服务器锁住了");

			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			var modPlayer = player.GetModPlayer<MPlayer>(mod);
			modPlayer.Locked = true;
		}
		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			rare = 10;
			base.ModifyBuffTip(ref tip, ref rare);
		}
	}
}