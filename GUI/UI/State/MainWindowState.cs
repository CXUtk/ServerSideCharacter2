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


namespace ServerSideCharacter2.GUI.UI
{
	public class LoginWindowState : WindowUIState
	{
		private UIPages _pageList;
		private UITextBox _usernameText;
		private UITextBox _passwordText;

		private const float LOGIN_WIDTH = 320;
		private const float LOGIN_HEIGHT = 200;

		protected override void Initialize(UIPanel WindowPanel)
		{
			WindowPanel.Left.Set(Main.screenWidth / 2 - LOGIN_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - LOGIN_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(LOGIN_WIDTH, 0f);
			WindowPanel.Height.Set(LOGIN_HEIGHT, 0f);
			_usernameText = new UITextBox("");
		}

		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState();
		}
	}
}
