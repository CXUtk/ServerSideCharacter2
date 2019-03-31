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
	public class UnionPageState : AdvWindowUIState
	{

		private int _relaxTimer;
		private float _rotation;
		private List<UIFriendBar> uIFriendBars;
		private UIList _onlinePlayerList;

		private UIPanel _onlinePlayerPanel;
		private UIButton refreshButton;
		private UIButton changeSortModeButton;

		private const float WINDOW_WIDTH = 600;
		private const float WINDOW_HEIGHT = 480;
		private const float FRIENDLIST_WIDTH = 320;
		private const float FRIENDLIST_HEIGHT = 360;
		private const float FRIENDLIST_OFFSET_RIGHT = 120;
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
			_onlinePlayerPanel.Left.Set(-FRIENDLIST_WIDTH / 2 + FRIENDLIST_OFFSET_RIGHT, 0.5f);
			_onlinePlayerPanel.Width.Set(FRIENDLIST_WIDTH, 0f);
			_onlinePlayerPanel.Height.Set(FRIENDLIST_HEIGHT, 0f);

			var onlinelabel = new UIText("好友列表");
			onlinelabel.Top.Set(-35, 0f);
			var texSize = Main.fontMouseText.MeasureString(onlinelabel.Text);
			onlinelabel.Left.Set(-texSize.X / 2, 0.5f);
			_onlinePlayerPanel.Append(onlinelabel);
			WindowPanel.Append(_onlinePlayerPanel);

			refreshButton = new UIButton(ServerSideCharacter2.ModTexturesTable["Refresh"], false);
			refreshButton.Top.Set(-50, 1f);
			refreshButton.Left.Set(FRIENDLIST_OFFSET_RIGHT - 35 / 2, 0.5f);
			refreshButton.Width.Set(35, 0f);
			refreshButton.Height.Set(35, 0f);
			refreshButton.ButtonDefaultColor = new Color(200, 200, 200);
			refreshButton.ButtonChangeColor = Color.White;
			refreshButton.UseRotation = true;
			refreshButton.TextureScale = 0.2f;
			refreshButton.Tooltip = "刷新";
			refreshButton.OnClick += RefreshButton_OnClick;
			WindowPanel.Append(refreshButton);

			_onlinePlayerList = new UIList();
			_onlinePlayerList.Width.Set(-25f, 1f);
			_onlinePlayerList.Height.Set(0f, 1f);
			_onlinePlayerList.ListPadding = 5f;
			_onlinePlayerPanel.Append(_onlinePlayerList);

			// ScrollBar设定
			var uiscrollbar = new UIScrollbar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			_onlinePlayerPanel.Append(uiscrollbar);
			_onlinePlayerList.SetScrollbar(uiscrollbar);
		}

		private void RefreshButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			RefreshFriends();
		}

		public void RefreshFriends()
		{
			uIFriendBars.Clear();
			_onlinePlayerList.Clear();

			if (Main.netMode == 1)
			{
				MessageSender.SendGetFriends();
			}
			else
			{
				for (var i = 0; i < 20; i++)
				{
					var testinfo = new JsonData.SimplifiedPlayerInfo
					{
						Name = ServerUtils.RandomGenString()
					};
					var bar = new UIFriendBar(testinfo);
					uIFriendBars.Add(bar);
					_onlinePlayerList.Add(bar);
				}
			}
			_relaxTimer = 180;
			_rotation = 0f;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (_relaxTimer > 0)
			{
				_relaxTimer--;
				_rotation += 0.1f;
				refreshButton.Enabled = false;
			}
			else
			{
				_rotation = 0f;
				refreshButton.Enabled = true;
			}
			refreshButton.Rotation = _rotation;
		}

		public void AppendFriends(JsonData.SimplifiedPlayerInfo info)
		{
			var bar = new UIFriendBar(info);
			uIFriendBars.Add(bar);
			_onlinePlayerList.Add(bar);
		}


		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionPage);
		}
	}
}
