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
using Terraria.GameContent.UI;
using ServerSideCharacter2.Unions;

namespace ServerSideCharacter2.GUI.UI
{
	public class UnionCreatePage : AdvWindowUIState
	{

		private int _relaxTimer;
		private float _rotation;

		public static UnionCreatePage Instance;

		private UIPlayerProfileHead uIPlayerProfileHead;


		private const float WINDOW_WIDTH = 380;
		private const float WINDOW_HEIGHT = 160;
		private readonly int TEXTBOX_HEIGHT = 35;
		private readonly int TEXTBOX_WIDTH = 200;
		private readonly int Y_OFFSET = 50;
		private readonly int X_OFFSET = -40;
		private UIAdvTextBox _unionNameText;
		private UICDButton _submitButton;


		public UnionCreatePage()
		{
			Instance = this;
		}


		protected override void Initialize(UIAdvPanel WindowPanel)
		{
			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - WINDOW_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - WINDOW_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(WINDOW_WIDTH, 0f);
			WindowPanel.Height.Set(WINDOW_HEIGHT, 0f);
			WindowPanel.Color = Color.White * 0.8f;

			_unionNameText = new UIAdvTextBox();
			_unionNameText.Top.Set(-TEXTBOX_HEIGHT + Y_OFFSET, 0.5f);
			_unionNameText.Left.Set(-TEXTBOX_WIDTH / 2 + X_OFFSET, 0.5f);
			_unionNameText.Width.Set(TEXTBOX_WIDTH, 0f);
			_unionNameText.Height.Set(TEXTBOX_HEIGHT, 0f);
			WindowPanel.Append(_unionNameText);

			var label = new UIText("输入公会名称");
			label.Top.Set(60, 0f);
			label.Left.Set(-TEXTBOX_WIDTH / 2 + X_OFFSET, 0.5f);
			WindowPanel.Append(label);

			var submitButton = new UICDButton(null, true);
			submitButton.Top.Set(-TEXTBOX_HEIGHT + Y_OFFSET - 5, 0.5f);
			submitButton.Left.Set(TEXTBOX_WIDTH / 2 + X_OFFSET + 20, 0.5f);
			submitButton.Width.Set(70f, 0f);
			submitButton.Height.Set(38f, 0f);
			submitButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
			submitButton.ButtonDefaultColor = new Color(200, 200, 200);
			submitButton.ButtonChangeColor = Color.White;
			submitButton.CornerSize = new Vector2(12, 12);
			submitButton.ButtonText = "创建";
			submitButton.OnClick += SubmitButton_OnClick;
			WindowPanel.Append(submitButton);
		}

		private void SubmitButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if(_unionNameText.Text.Length > 10)
			{
				ServerSideCharacter2.Instance.ShowMessage("公会名字过长", 120, Color.OrangeRed);
				return;
			}
			MessageSender.SendUnionCreate(_unionNameText.Text);
		}

		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionPage3);
		}
	}
}
