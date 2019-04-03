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
	public class CommunicationState : AdvWindowUIState
	{
		public static CommunicationState Instance;
		private int _relaxTimer;
		private float _rotation;

		private UIPlayerProfileHead uIPlayerProfileHead;
		private UIAdvList _friendList;


		private const float WINDOW_WIDTH = 800;
		private const float WINDOW_HEIGHT = 700;
		private const float FRIENDLIST_WIDTH = 300;
		private const float FRIENDLIST_HEIGHT = 300;
		private const float FRIENDLIST_OFFSET_RIGHT = 170;
		private const float FRIENDLIST_OFFSET_TOP = 30;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;
		private const float PLAYER_IMAGE_OFFSET_X = 50;
		private const float PLAYER_IMAGE_OFFSET_Y = 65;

		public CommunicationState()
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

			var friendListPanel = new UIAdvPanel(Drawing.Box1);
			friendListPanel.Top.Set(95, 0f);
			friendListPanel.Left.Set(10f, 0f);
			friendListPanel.Width.Set(FRIENDLIST_WIDTH, 0f);
			friendListPanel.Height.Set(FRIENDLIST_HEIGHT, 0f);
			friendListPanel.SetPadding(10f);

			_friendList = new UIAdvList();
			_friendList.Width.Set(-25f, 1f);
			_friendList.Height.Set(0f, 1f);
			_friendList.ListPadding = 5f;
			friendListPanel.Append(_friendList);

			// ScrollBar设定
			var uiscrollbar = new UIAdvScrollBar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			friendListPanel.Append(uiscrollbar);
			_friendList.SetScrollbar(uiscrollbar);
			WindowPanel.Append(friendListPanel);
		}

		//private void RefreshButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		//{
		//	RefreshFriends();
		//}

		public void SetProfile(JsonData.SimplifiedPlayerInfo info)
		{
			uIPlayerProfileHead.SetPlayer(info);
		}

		public void RefreshFriends()
		{
			//uIFriendBars.Clear();
			_friendList.Clear();

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
					_friendList.Add(bar);
				}
			}
			_relaxTimer = 180;
			_rotation = 0f;
		}





		//public override void Update(GameTime gameTime)
		//{
		//	base.Update(gameTime);
		//	if (Main.netMode == 0)
		//	{
		//		JsonData.SimplifiedPlayerInfo info = new JsonData.SimplifiedPlayerInfo
		//		{
		//			Name = Main.LocalPlayer.name,
		//			IsFriend = true,
		//			IsLogin = true,
		//			Rank = 1500
		//		};
		//		uIPlayerProfileHead.SetPlayer(info);
		//	}
		//	if (_relaxTimer > 0)
		//	{
		//		_relaxTimer--;
		//		_rotation += 0.1f;
		//		refreshButton.Enabled = false;
		//	}
		//	else
		//	{
		//		_rotation = 0f;
		//		refreshButton.Enabled = true;
		//	}
		//	refreshButton.Rotation = _rotation;
		//}

		public void AppendFriends(JsonData.SimplifiedPlayerInfo info)
		{
			UIFriendBar bar = new UIFriendBar(info);
			_friendList.Add(bar);
		}




		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.CommunicationPage);
		}
	}
}
