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

namespace ServerSideCharacter2.GUI.UI
{
	public class WindowUIState : UIDraggableState
	{
		private UIPanel WindowPanel;
		private UIButton close;



		public sealed override void OnInitialize()
		{
			WindowPanel = new UIPanel();
			Texture2D closeTex = ServerSideCharacter2.ModTexturesTable["CloseButton"];
			close = new UIButton(closeTex, false);
			close.Left.Set(-30f, 1f);
			close.Top.Set(10f, 0f);
			close.Width.Set(20f, 0f);
			close.Height.Set(20f, 0f);
			close.OnClick += Close_OnClick;
			//close.OnMouseOver += Close_OnMouseOver;
			WindowPanel.Left.Set(Main.screenWidth / 2 - 150f, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - 150f, 0f);
			WindowPanel.Width.Set(300f, 0f);
			WindowPanel.Height.Set(300f, 0f);
			WindowPanel.BackgroundColor = new Color(73, 94, 171) * 0.7f;
			WindowPanel.Append(close);
			Initialize(WindowPanel);
			base.AppendDraggableElement(WindowPanel);
		}

		public void SetCloseTexture(Texture2D tex)
		{
			close.Texture = tex;
		}



		protected virtual void Initialize(UIPanel WindowPanel)
		{

		}

		private void Close_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			OnClose(evt, listeningElement);
		}

		protected virtual void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{

		}



	}
}
