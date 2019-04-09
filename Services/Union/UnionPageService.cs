using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ServerSideCharacter2.GUI;
using ServerSideCharacter2.GUI.UI;
using Terraria;
using Terraria.UI;

namespace ServerSideCharacter2.Services.Union
{
	public class UnionPageService : ISSCToolBarService
	{
		public Texture2D Texture => ServerSideCharacter2.ModTexturesTable["Home"];

		public string Tooltip => "公会";

		public string Name => "SSC: 公会";

		public bool Enabled { get; set; }

		public UnionPageService()
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
			if (ServerSideCharacter2.ClientUnion != null)
			{
				ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionPage2);
				if (ServerSideCharacter2.GuiManager.IsActive(SSCUIState.UnionPage2))
				{
					UnionPageState2.Instance.RefreshUnion();
				}
			}
			else
			{
				ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionPage);
				if (ServerSideCharacter2.GuiManager.IsActive(SSCUIState.UnionPage))
				{
					UnionPageState.Instance.RefreshUnions();
				}
			}
		}
	}
}
