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

namespace ServerSideCharacter2.GUI.UI
{
	public class LoginWindowState : AdvWindowUIState
	{
		private UIPages _pageList;
		private UIAdvTextBox _usernameText;
		private UIAdvTextBox _passwordText;
		private UIButton _submitFormButton;

		private const float LOGIN_WIDTH = 320;
		private const float LOGIN_HEIGHT = 200;
		private const float TEXTBOX_WIDTH = 200;
		private const float TEXTBOX_HEIGHT = 30;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;

		protected override void Initialize(UIAdvPanel WindowPanel)
		{
			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - LOGIN_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - LOGIN_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(LOGIN_WIDTH, 0f);
			WindowPanel.Height.Set(LOGIN_HEIGHT, 0f);
			WindowPanel.Color = Color.White * 0.8f;

			var label1 = new UIText("账号");
			label1.Top.Set(7, 0);
			label1.Left.Set(-50, 0);
			label1.Width.Set(50, 0);
			label1.Height.Set(0, 1);
			_usernameText = new UIAdvTextBox();
			_usernameText.Top.Set(-TEXTBOX_HEIGHT - Y_OFFSET, 0.5f);
			_usernameText.Left.Set(-TEXTBOX_WIDTH / 2 + X_OFFSET, 0.5f);
			_usernameText.Width.Set(TEXTBOX_WIDTH, 0f);
			_usernameText.Height.Set(TEXTBOX_HEIGHT, 0f);
			_usernameText.Append(label1);
			WindowPanel.Append(_usernameText);


			var label2 = new UIText("密码");
			label2.Top.Set(7, 0);
			label2.Left.Set(-50, 0);
			label2.Width.Set(50, 0);
			label2.Height.Set(0, 1);
			_passwordText = new UIAdvTextBox();
			_passwordText.Top.Set(15 - Y_OFFSET, 0.5f);
			_passwordText.Left.Set(-TEXTBOX_WIDTH / 2 + X_OFFSET, 0.5f);
			_passwordText.Width.Set(TEXTBOX_WIDTH, 0f);
			_passwordText.Height.Set(TEXTBOX_HEIGHT, 0f);
			_passwordText.Password = true;
			_passwordText.Append(label2);
			WindowPanel.Append(_passwordText);

			_submitFormButton = new UIButton(null, true);
			_submitFormButton.Left.Set(-BUTTON_WIDTH / 2, 0.5f);
			_submitFormButton.Top.Set(60 - BUTTON_HEIGHT / 2, 0.5f);
			_submitFormButton.Width.Set(BUTTON_WIDTH, 0f);
			_submitFormButton.Height.Set(BUTTON_HEIGHT, 0f);
			_submitFormButton.ButtonText = "提交";
			_submitFormButton.CornerSize = new Vector2(12, 12);
			_submitFormButton.ButtonDefaultColor = new Color(200, 200, 200);
			_submitFormButton.ButtonChangeColor = Color.White;
			_submitFormButton.OnClick += _submitFormButton_OnClick;
			WindowPanel.Append(_submitFormButton);
		}

		private void _submitFormButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			var username = _usernameText.Text;
			var password = _passwordText.Text;

		}

		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState();
		}
	}
}
