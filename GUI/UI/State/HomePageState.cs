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
	public class HomePageState : AdvWindowUIState
	{

		private int _relaxTimer;
		private float _rotation;
		private List<UIFriendBar> uIFriendBars;
		private UIAdvList _friendList;

		private UIAdvPanel _onlinePlayerPanel;
		private UIButton refreshButton;
		private UIButton changeSortModeButton;
		private UIPlayerProfileHead uIPlayerProfileHead;


		private const float WINDOW_WIDTH = 720;
		private const float WINDOW_HEIGHT = 480;
		private const float FRIENDLIST_WIDTH = 320;
		private const float FRIENDLIST_HEIGHT = 360;
		private const float FRIENDLIST_OFFSET_RIGHT = 170;
		private const float FRIENDLIST_OFFSET_TOP = 30;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;
		private const float PLAYER_IMAGE_OFFSET_X = 64;
		private const float PLAYER_IMAGE_OFFSET_Y = 82;



		protected override void Initialize(UIAdvPanel WindowPanel)
		{
			uIFriendBars = new List<UIFriendBar>();
			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - WINDOW_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - WINDOW_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(WINDOW_WIDTH, 0f);
			WindowPanel.Height.Set(WINDOW_HEIGHT, 0f);
			WindowPanel.Color = Color.White * 0.8f;

			_onlinePlayerPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"]);
			_onlinePlayerPanel.CornerSize = new Vector2(8, 8);
			_onlinePlayerPanel.OverflowHidden = true;
			_onlinePlayerPanel.SetPadding(10f);
			_onlinePlayerPanel.Top.Set(-FRIENDLIST_HEIGHT / 2 + FRIENDLIST_OFFSET_TOP, 0.5f);
			_onlinePlayerPanel.Left.Set(-FRIENDLIST_WIDTH / 2 + FRIENDLIST_OFFSET_RIGHT, 0.5f);
			_onlinePlayerPanel.Width.Set(FRIENDLIST_WIDTH, 0f);
			_onlinePlayerPanel.Height.Set(FRIENDLIST_HEIGHT, 0f);

			UIText onlinelabel = new UIText("好友列表");
			onlinelabel.Top.Set(35 + FRIENDLIST_OFFSET_TOP, 0f);
			Vector2 texSize = Main.fontMouseText.MeasureString(onlinelabel.Text);
			onlinelabel.Left.Set(-FRIENDLIST_WIDTH / 2 + FRIENDLIST_OFFSET_RIGHT, 0.5f);
			WindowPanel.Append(onlinelabel);
			WindowPanel.Append(_onlinePlayerPanel);

			refreshButton = new UIButton(ServerSideCharacter2.ModTexturesTable["Refresh"], false);
			refreshButton.Top.Set(47f, 0f);
			refreshButton.Left.Set(FRIENDLIST_OFFSET_RIGHT + FRIENDLIST_WIDTH / 2 - 35, 0.5f);
			refreshButton.Width.Set(35, 0f);
			refreshButton.Height.Set(35, 0f);
			refreshButton.ButtonDefaultColor = new Color(200, 200, 200);
			refreshButton.ButtonChangeColor = Color.White;
			refreshButton.UseRotation = true;
			refreshButton.TextureScale = 0.2f;
			refreshButton.Tooltip = "刷新";
			refreshButton.OnClick += RefreshButton_OnClick;
			WindowPanel.Append(refreshButton);

			_friendList = new UIAdvList();
			_friendList.Width.Set(-25f, 1f);
			_friendList.Height.Set(0f, 1f);
			_friendList.ListPadding = 5f;
			_onlinePlayerPanel.Append(_friendList);

			// ScrollBar设定
			UIAdvScrollBar uiscrollbar = new UIAdvScrollBar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			_onlinePlayerPanel.Append(uiscrollbar);
			_friendList.SetScrollbar(uiscrollbar);

			uIPlayerProfileHead = new UIPlayerProfileHead();
			uIPlayerProfileHead.Top.Set(PLAYER_IMAGE_OFFSET_Y, 0f);
			uIPlayerProfileHead.Left.Set(PLAYER_IMAGE_OFFSET_X, 0f);
			uIPlayerProfileHead.Width.Set(300, 0f);
			uIPlayerProfileHead.Height.Set(60, 0f);
			WindowPanel.Append(uIPlayerProfileHead);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			
		}

		private void RefreshButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			RefreshFriends();
		}

		public void SetProfile(JsonData.SimplifiedPlayerInfo info)
		{
			uIPlayerProfileHead.SetPlayer(info);
		}

		

		public void RefreshFriends()
		{
			uIFriendBars.Clear();
			_friendList.Clear();

			if (Main.netMode == 1)
			{
				MessageSender.SendGetFriends();
			}
			else
			{
				for (int i = 0; i < 20; i++)
				{
					JsonData.SimplifiedPlayerInfo testinfo = new JsonData.SimplifiedPlayerInfo
					{
						Name = ServerUtils.RandomGenString()
					};
					var bar = new UIFriendBar(testinfo);
					uIFriendBars.Add(bar);
					_friendList.Add(bar);
				}
			}
			_relaxTimer = 180;
			_rotation = 0f;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (Main.netMode == 0)
			{
				JsonData.SimplifiedPlayerInfo info = new JsonData.SimplifiedPlayerInfo
				{
					Name = Main.LocalPlayer.name,
					IsFriend = true,
					IsLogin = true,
					Rank = 1500
				};
				uIPlayerProfileHead.SetPlayer(info);
			}
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
			UIFriendBar bar = new UIFriendBar(info);
			uIFriendBars.Add(bar);
			_friendList.Add(bar);
		}




		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.HomePage);
		}
	}
}
