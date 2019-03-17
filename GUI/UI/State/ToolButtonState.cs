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
	public class ToolButtonState : UIState
	{
		private UIPicButton _mainButton;

		private Texture2D _gearTex;

		public override void OnInitialize()
		{
			_mainButton = new UIPicButton();
			_mainButton.Texture = ServerSideCharacter2.ModTexturesTable["Cog"];
			_mainButton.Width.Set(50f, 0f);
			_mainButton.Height.Set(50f, 0f);
			_mainButton.ButtonDefaultColor = Color.White * 0.75f;
			_mainButton.ButtonSecondColor = Color.White * 0.3f;
			_mainButton.OnClick += _mainButton_OnClick;

			Append(_mainButton);
		}

		private void _mainButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (Main.playerInventory) return;
			base.Draw(spriteBatch);
		}
	}
}
