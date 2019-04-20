using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ServerSideCharacter2.GUI;
using ServerSideCharacter2.GUI.UI;
using ServerSideCharacter2.GUI.UI.Component;
using Terraria;
using Terraria.UI;

namespace ServerSideCharacter2.Services.OnlinePlayer
{
	public class PlayerInventoryService : ISSCToolBarService
	{
		public Texture2D Texture => ServerSideCharacter2.ModTexturesTable["Cog"];

		public string Tooltip => "背包查看器";

		public string Name => "SSC: 背包查看";

		public bool Enabled { get; set; }

		public UIDrawEventHandler DrawEvent => null;

		public PlayerInventoryService()
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
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.InventoryPage);
			if (ServerSideCharacter2.GuiManager.IsActive(SSCUIState.InventoryPage))
			{
			 	PlayerInventoryState.Instance.GetInventory(Main.myPlayer);
			}
		}


	}
}
