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
	public class ToolBarState : UIState
	{
		private UIAdvPanel windowPanel;
		private bool _collapseOn;
		private UIButton _openButton;
		private UIButton _toggleDestructButton;
		private UIButton _toggleScrollButton;

		private List<UIButton> _toolButtons = new List<UIButton>();

		private const float TOOLBAR_INIT_WIDTH = 200f;
		private const float TOOLBAR_INIT_HEIGHT = 74f;
		private const float TOOLBAR_ICON_PADDING_LEFT = 30f;
		private const float TOOLBAR_ICON_MARGIN_LEFT = 50f;

		public void UnlockButtons(int n)
		{
			if (n > _toolButtons.Count) return;
			windowPanel.RemoveAllChildren();
			for(int i = 0; i < n; i++)
			{
				var but = _toolButtons[i];
				but.Top.Set(-but.Height.Pixels / 2, 0.5f);
				but.Left.Set(TOOLBAR_ICON_PADDING_LEFT + i * TOOLBAR_ICON_MARGIN_LEFT, 0f);
				windowPanel.Append(but);
			}
			float estimatedWidth = TOOLBAR_ICON_PADDING_LEFT * 2 + n * TOOLBAR_ICON_MARGIN_LEFT;
			if(estimatedWidth < TOOLBAR_INIT_WIDTH)
			{
				estimatedWidth = TOOLBAR_INIT_WIDTH;
			}
			windowPanel.Left.Set(Main.screenWidth / 2 - 122f, 0f);
			windowPanel.Width.Set(estimatedWidth, 0f);
		}


		private void SetUpButtons()
		{
			var boxTex = ServerSideCharacter2.ModTexturesTable["Box"];
			UIButton loginButton = new UIButton(ServerSideCharacter2.ModTexturesTable["Cog"], false);
			loginButton.OnClick += LoginButton_OnClick;
			loginButton.Width.Set(35, 0f);
			loginButton.Height.Set(35, 0f);
			loginButton.Tooltip = "登录界面";
			_toolButtons.Add(loginButton);
		}

		private void LoginButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.LoginWindow);
		}

		public override void OnInitialize()
		{
			_collapseOn = false;
			windowPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["AdvInvBack1"]);
			windowPanel.Left.Set(Main.screenWidth / 2 - TOOLBAR_INIT_WIDTH / 2, 0f);
			windowPanel.Top.Set(Main.screenHeight - 12f, 0f);
			windowPanel.Width.Set(TOOLBAR_INIT_WIDTH, 0f);
			windowPanel.Height.Set(TOOLBAR_INIT_HEIGHT, 0f);
			windowPanel.Color = Color.Transparent;
			windowPanel.CornerSize = new Vector2(12, 12);

			_openButton = new UIButton(ServerSideCharacter2.ModTexturesTable["CollapseButtonUp"], false);
			_openButton.Left.Set(Main.screenWidth / 2 - 122f, 0f);
			_openButton.Top.Set(windowPanel.GetDimensions().Position().Y - 12f, 0f);
			_openButton.Width.Set(48f, 0f);
			_openButton.Height.Set(14f, 0f);
			_openButton.ButtonDefaultColor = Color.White;
			_openButton.ButtonChangeColor = new Color(0.8f, 0.8f, 0.8f, 1f);
			_openButton.Tooltip = "打开底栏";
			_openButton.OnClick += OpenPanel_OnClick;

			SetUpButtons();
			UnlockButtons(1);
			base.Append(_openButton);
			base.Append(windowPanel);
			

		}

		private void swapButtonTexture()
		{
			if (_collapseOn)
			{
				_openButton.Texture = ServerSideCharacter2.ModTexturesTable["CollapseButtonDown"];
				_openButton.Tooltip = "收回底栏";
			}
			else
			{
				_openButton.Texture = ServerSideCharacter2.ModTexturesTable["CollapseButtonUp"];
				_openButton.Tooltip = "打开底栏";
			}
		}

		internal void OpenPanel_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			_collapseOn ^= true;
			swapButtonTexture();
			windowPanel.Color = Color.White;
		}

		public override void Update(GameTime gameTime)
		{
			float maxH = Main.screenHeight - TOOLBAR_INIT_HEIGHT - 12f;
			if (_collapseOn && windowPanel.Top.Pixels > maxH)
			{
				windowPanel.Top.Set(windowPanel.Top.Pixels - 6f, 0f);
				if (windowPanel.Top.Pixels < maxH)
				{
					windowPanel.Top.Pixels = maxH;
				}
				windowPanel.Color = Color.White;
			}
			else if (!_collapseOn && windowPanel.Top.Pixels < Main.screenHeight)
			{
				windowPanel.Top.Set(windowPanel.Top.Pixels + 6f, 0f);
				if (windowPanel.Top.Pixels >= Main.screenHeight)
				{
					windowPanel.Color = Color.Transparent;
					windowPanel.Top.Pixels = Main.screenHeight;
				}
			}
			_openButton.Left.Set(windowPanel.GetDimensions().Center().X - 24f, 0f);
			_openButton.Top.Set(windowPanel.Top.Pixels - 12f, 0f);
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{

			if (windowPanel.ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
				Main.LocalPlayer.showItemIcon = false;
			}
			base.Draw(spriteBatch);
		}
	}
}
