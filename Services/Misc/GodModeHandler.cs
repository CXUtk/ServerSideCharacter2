using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Services.Misc
{
	public class GodModeHandler : SSCCommandHandler
	{
		public override string PermissionName => "god";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				Player p = Main.player[playerNumber];
				ServerPlayer player = p.GetServerPlayer();
				MPlayer mPlayer = p.GetModPlayer<MPlayer>();
				mPlayer.GodMode ^= true;

				ModPacket pack = ServerSideCharacter2.Instance.GetPacket();
				pack.Write((int)SSCMessageType.SetGodMode);
				pack.Write(mPlayer.GodMode);
				pack.Send(playerNumber);

				player.SendInfoMessage($"你成功{(mPlayer.GodMode ? "开启" : "关闭")}了无敌模式");
				CommandBoardcast.ConsoleMessage($"玩家 {player.Name} {(mPlayer.GodMode ? "开启" : "关闭")}了无敌模式");
			}
		}
	}

	public class SetGodModeHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			if(Main.netMode == 1)
			{
				MPlayer mPlayer = Main.LocalPlayer.GetModPlayer<MPlayer>();
				mPlayer.GodMode = reader.ReadBoolean();
			}
		}

	}
}
