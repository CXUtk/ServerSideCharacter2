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

namespace ServerSideCharacter2.GUI.UI
{
	public class HomePageState : AdvWindowUIState
	{

		private int _relaxTimer;

		private const float WINDOW_WIDTH = 320;
		private const float WINDOW_HEIGHT = 200;
		private const float TEXTBOX_WIDTH = 200;
		private const float TEXTBOX_HEIGHT = 30;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;


		protected override void Initialize(UIAdvPanel WindowPanel)
		{
			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - WINDOW_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - WINDOW_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(WINDOW_WIDTH, 0f);
			WindowPanel.Height.Set(WINDOW_HEIGHT, 0f);
			WindowPanel.Color = Color.White * 0.8f;
		}


		public override void Update(GameTime gameTime)
		{

			base.Update(gameTime);
		}

		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.HomePage);
		}
	}
}
