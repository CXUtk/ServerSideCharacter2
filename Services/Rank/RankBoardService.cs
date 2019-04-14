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

namespace ServerSideCharacter2.Services.Rank
{
	public class RankBoardService : ISSCToolBarService
	{
		public Texture2D Texture => ServerSideCharacter2.ModTexturesTable["JB"];

		public string Tooltip => "排位排行榜";

		public string Name => "SSC: 排行榜";

		public bool Enabled { get; set; }

		public UIDrawEventHandler DrawEvent => null;

		public RankBoardService()
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
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.RankBoard);
			if (ServerSideCharacter2.GuiManager.IsActive(SSCUIState.RankBoard))
			{
				RankBoardState.Instance.RefreshBoard();
			}
		}
	}
}
