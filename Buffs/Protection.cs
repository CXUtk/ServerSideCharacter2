using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Buffs
{
    public class Protection : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("传送保护");
            Description.SetDefault("获得短暂无敌");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
			player.noFallDmg = true;
			player.immune = true;
			player.immuneTime = 2;
        }
    }
}
