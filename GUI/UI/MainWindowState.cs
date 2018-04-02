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
	public class MainWindowState : WindowUIState
	{
		private UIPages _pageList;

		protected override void Initialize(UIPanel WindowPanel)
		{
			_pageList = new UIPages();
			UIText text = new UIText("Default Page!");
			text.Top.Set(0, 0.5f);
			text.Left.Set(0, 0.5f);
			text.TextColor = Color.Red;
			UIText text1 = new UIText("Second Page!");
			text1.Top.Set(0, 0.5f);
			text1.Left.Set(0, 0.5f);
			text1.TextColor = Color.Red;
			UIButton button = new UIButton();
			button.Top.Set(300f, 0f);
			button.Left.Set(10f, 0f);
			button.Width.Set(100f, 0f);
			button.Height.Set(30f, 0f);
			button.OnClick += Button_OnClick;
			button.ButtonText = "Switch";
			WindowPanel.Append(button);
			_pageList.AddPage("default", text);
			_pageList.AddPage("second", text1);
			_pageList.Top.Set(0, 0.5f);
			_pageList.Left.Set(0, 0.5f);
			_pageList.SetPage("second");
			WindowPanel.Append(_pageList);

		}

		private void Button_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			_pageList.SetPage("default");
		}

		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState();
		}
	}
}
