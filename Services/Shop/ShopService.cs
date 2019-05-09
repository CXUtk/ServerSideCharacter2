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

namespace ServerSideCharacter2.Services.Shop
{
	public class ShopService : ISSCToolBarService
	{
		public Texture2D Texture => ServerSideCharacter2.ModTexturesTable["Cog"];

		public string Tooltip => "商城";

		public string Name => "SSC: 商城";

		public bool Enabled { get; set; }

		public UIDrawEventHandler DrawEvent => null;

		public ShopService()
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
			//Main.NewText("商城系统暂未完成，还不能使用", Color.Red);
			//return;
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.NormalShopPage);
			NormalShopState.Instance.RefreshItems();
		}
	}
}
