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

namespace ServerSideCharacter2.GUI.UI
{
	public class PlayerOnlineWindow : AdvWindowUIState
	{

		private int _relaxTimer;
		private List<UIFriendBar> uIFriendBars;
		private UIList _onlinePlayerList;
		private UIPanel _onlinePlayerPanel;

		private const float WINDOW_WIDTH = 600;
		private const float WINDOW_HEIGHT = 480;
		private const float FRIENDLIST_WIDTH = 250;
		private const float FRIENDLIST_HEIGHT = 360;
		private const float FRIENDLIST_OFFSET_LEFT = 150;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;

		protected override void Initialize(UIAdvPanel WindowPanel)
		{
			uIFriendBars = new List<UIFriendBar>();

			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - WINDOW_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - WINDOW_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(WINDOW_WIDTH, 0f);
			WindowPanel.Height.Set(WINDOW_HEIGHT, 0f);
			WindowPanel.Color = Color.White * 0.8f;

			_onlinePlayerPanel = new UIPanel();
			_onlinePlayerPanel.Top.Set(-FRIENDLIST_HEIGHT / 2, 0.5f);
			_onlinePlayerPanel.Left.Set(-FRIENDLIST_WIDTH / 2 - FRIENDLIST_OFFSET_LEFT, 0.5f);
			_onlinePlayerPanel.Width.Set(FRIENDLIST_WIDTH, 0f);
			_onlinePlayerPanel.Height.Set(FRIENDLIST_HEIGHT, 0f);
			WindowPanel.Append(_onlinePlayerPanel);

			_onlinePlayerList = new UIList();
			_onlinePlayerList.Width.Set(-25f, 1f);
			_onlinePlayerList.Height.Set(0f, 1f);
			_onlinePlayerList.ListPadding = 5f;

			_onlinePlayerPanel.Append(_onlinePlayerList);

			// ScrollBar设定
			UIScrollbar uiscrollbar = new UIScrollbar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			_onlinePlayerPanel.Append(uiscrollbar);
			_onlinePlayerList.SetScrollbar(uiscrollbar);

		}


		public void AppendPlayers()
		{
			var info = new JsonData.SimplifiedPlayerInfo();
			info.Name = Main.LocalPlayer.name;
			info.PlayerID = Main.myPlayer;
			UIFriendBar bar = new UIFriendBar(info);
			_onlinePlayerList.Append(bar);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.PlayerOnlineWindow);
		}
	}
}
