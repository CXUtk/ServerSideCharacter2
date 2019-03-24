using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ServerSideCharacter2.GUI;
using Terraria;
using Terraria.UI;

namespace ServerSideCharacter2.Services.HomePage
{
	public class HomePageService : ISSCToolBarService
	{
		public Texture2D Texture => ServerSideCharacter2.ModTexturesTable["Home"];

		public string Tooltip => "主页";

		public string Name => "SSC: 主页";

		public bool Enabled { get; set; }

		public HomePageService()
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
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.HomePage);
			if (ServerSideCharacter2.GuiManager.IsActive(SSCUIState.HomePage))
			{
				ServerSideCharacter2.GuiManager.RefreshFriends();
			}
		}
	}
}
