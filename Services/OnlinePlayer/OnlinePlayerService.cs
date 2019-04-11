using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ServerSideCharacter2.GUI;
using ServerSideCharacter2.GUI.UI.Component;
using Terraria;
using Terraria.UI;

namespace ServerSideCharacter2.Services.OnlinePlayer
{
	public class OnlinePlayerService : ISSCToolBarService
	{
		public Texture2D Texture => ServerSideCharacter2.ModTexturesTable["Cog"];

		public string Tooltip => "在线玩家窗口";

		public string Name => "SSC: 在线玩家";

		public bool Enabled { get; set; }

		public UIDrawEventHandler DrawEvent => null;

		public OnlinePlayerService()
		{
			Enabled = true;
		}

		public void OnButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			if (Main.netMode != 0 && !ServerSideCharacter2.Instance.IsLoginClientSide)
			{
				Main.NewText("您还没有登录，请先登录", Color.Red);
				return;
			}
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.PlayerOnlineWindow);
			if (ServerSideCharacter2.GuiManager.IsActive(SSCUIState.PlayerOnlineWindow))
			{
				ServerSideCharacter2.GuiManager.RefreshOnlinePlayers();
			}
		}
	}
}
