using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ServerSideCharacter2.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.UI;

namespace ServerSideCharacter2.Services.Misc
{
	public class ItemServices : ISSCToolBarService
	{
		public Texture2D Texture => ServerSideCharacter2.ModTexturesTable["Cog"];

		public string Tooltip => "刷物品窗口";

		public string Name => "SSC: 物品";

		public bool Enabled { get; set; }

		public ItemServices()
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
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.ItemPage);
			if (ServerSideCharacter2.GuiManager.IsActive(SSCUIState.ItemPage))
			{
				ServerSideCharacter2.GuiManager.RefreshOnlinePlayers();
			}
		}

	}
}
