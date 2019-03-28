using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ServerSideCharacter2.GUI.UI.Component;
using Terraria.GameContent.UI.States;
using ServerSideCharacter2.Utils;
using System.Security.Cryptography;
using ServerSideCharacter2.Core;
using ServerSideCharacter2.GUI.UI.Component.Special;
using Terraria.ModLoader.UI.Elements;

namespace ServerSideCharacter2.GUI.UI
{
	public class ItemState : AdvWindowUIState
	{

		private int _relaxTimer;
		private float _rotation;
		private UIGrid _itemGrid;
		private UIPanel _itemPanel;

		private const float WINDOW_WIDTH = 480;
		private const float WINDOW_HEIGHT = 480;



		protected override void Initialize(UIAdvPanel WindowPanel)
		{
			
			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - WINDOW_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - WINDOW_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(WINDOW_WIDTH, 0f);
			WindowPanel.Height.Set(WINDOW_HEIGHT, 0f);
			WindowPanel.Color = Color.White * 0.8f;

			_itemPanel = new UIPanel();
			_itemPanel.Top.Set(20, 0f);
			_itemPanel.Left.Set(20, 0f);
			_itemPanel.Width.Set(-40, 1f);
			_itemPanel.Height.Set(-40, 1f);

			_itemGrid = new UIGrid();
			_itemGrid.Width.Set(-25f, 1f);
			_itemGrid.Height.Set(0f, 1f);
			_itemGrid.ListPadding = 5f;
			_itemPanel.Append(_itemGrid);


			for(int i = 0; i < Main.itemTexture.Length; i++)
			{
				var but = new UIPanel();
				but.Width.Set(35, 0f);
				but.Height.Set(35, 0f);
				_itemGrid.Add(but);
			}

			// ScrollBar设定
			UIScrollbar uiscrollbar = new UIScrollbar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			_itemPanel.Append(uiscrollbar);
			_itemGrid.SetScrollbar(uiscrollbar);

			WindowPanel.Append(_itemPanel);
		}


		

		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.ItemPage);
		}
	}
}
