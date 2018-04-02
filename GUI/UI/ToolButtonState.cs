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
	public class ToolButtonState : UIDraggableState
	{
		private UIButton _mainButton;

		private Texture2D _gearTex;

		public override void OnInitialize()
		{
			_gearTex = ServerSideCharacter2.ModTexturesTable["Cog"];
			UIImageResizable image = new UIImageResizable(_gearTex);
			image.ImageScale = (50f / _gearTex.Height) * 0.85f;
			image.AlignType = UIAlignType.AlignCenter;

			_mainButton = new UIButton();
			_mainButton.Width.Set(50f, 0f);
			_mainButton.Height.Set(50f, 0f);
			_mainButton.ButtonChangeColor = Color.White * 0.3f;
			_mainButton.ButtonDefaultColor = Color.White * 0.75f;
			_mainButton.OnClick += _mainButton_OnClick;
			_mainButton.Append(image);


			AppendDraggableElement(_mainButton);
		}

		private void _mainButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState();
		}
	}
}
