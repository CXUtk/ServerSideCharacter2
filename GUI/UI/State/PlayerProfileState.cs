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
	public class PlayerProfileState : AdvWindowUIState
	{

		private int _relaxTimer;
		private float _rotation;

		private UIPlayerProfileHead uIPlayerProfileHead;


		private const float WINDOW_WIDTH = 360;
		private const float WINDOW_HEIGHT = 480;
		private const float FRIENDLIST_WIDTH = 320;
		private const float FRIENDLIST_HEIGHT = 360;
		private const float FRIENDLIST_OFFSET_RIGHT = 170;
		private const float FRIENDLIST_OFFSET_TOP = 30;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;
		private const float PLAYER_IMAGE_OFFSET_X = 50;
		private const float PLAYER_IMAGE_OFFSET_Y = 65;



		protected override void Initialize(UIAdvPanel WindowPanel)
		{
			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - WINDOW_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - WINDOW_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(WINDOW_WIDTH, 0f);
			WindowPanel.Height.Set(WINDOW_HEIGHT, 0f);
			WindowPanel.Color = Color.White * 0.8f;


			uIPlayerProfileHead = new UIPlayerProfileHead();
			uIPlayerProfileHead.Top.Set(PLAYER_IMAGE_OFFSET_Y, 0f);
			uIPlayerProfileHead.Left.Set(PLAYER_IMAGE_OFFSET_X, 0f);
			uIPlayerProfileHead.Width.Set(300, 0f);
			uIPlayerProfileHead.Height.Set(60, 0f);
			WindowPanel.Append(uIPlayerProfileHead);
		}

		//private void RefreshButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		//{
		//	RefreshFriends();
		//}

		public void SetProfile(JsonData.SimplifiedPlayerInfo info)
		{
			uIPlayerProfileHead.SetPlayer(info);
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

		//public void AppendFriends(JsonData.SimplifiedPlayerInfo info)
		//{
		//	UIFriendBar bar = new UIFriendBar(info);
		//	uIFriendBars.Add(bar);
		//	_friendList.Add(bar);
		//}




		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.ProfilePage);
		}
	}
}
