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
		public static HomePageState Instance;

		private int _relaxTimer;
		private float _rotation;
		
		private UIButton refreshButton;
		private UIPlayerProfileHead uIPlayerProfileHead;
		private UIAdvPanel settingPanel;
		private UISwitch aboveHeadSwitch;


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

		public HomePageState()
		{
			Instance = this;
		}



		protected override void Initialize(UIAdvPanel WindowPanel)
		{
		// uIFriendBars = new List<UIFriendBar>();
			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - WINDOW_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - WINDOW_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(WINDOW_WIDTH, 0f);
			WindowPanel.Height.Set(WINDOW_HEIGHT, 0f);
			WindowPanel.Color = Color.White * 0.8f;

			settingPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				OverflowHidden = true
			};
			settingPanel.SetPadding(10f);
			settingPanel.Top.Set(-FRIENDLIST_HEIGHT / 2 + FRIENDLIST_OFFSET_TOP, 0.5f);
			settingPanel.Left.Set(-FRIENDLIST_WIDTH / 2 + FRIENDLIST_OFFSET_RIGHT, 0.5f);
			settingPanel.Width.Set(FRIENDLIST_WIDTH, 0f);
			settingPanel.Height.Set(FRIENDLIST_HEIGHT, 0f);
			WindowPanel.Append(settingPanel);
			GenerateSetting(settingPanel);

			//var onlinelabel = new UIText("好友列表");
			//onlinelabel.Top.Set(35 + FRIENDLIST_OFFSET_TOP, 0f);
			//var texSize = Main.fontMouseText.MeasureString(onlinelabel.Text);
			//onlinelabel.Left.Set(-FRIENDLIST_WIDTH / 2 + FRIENDLIST_OFFSET_RIGHT, 0.5f);
			//WindowPanel.Append(onlinelabel);
			//WindowPanel.Append(_onlinePlayerPanel);

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


			//_friendList = new UIAdvList();
			//_friendList.Width.Set(-25f, 1f);
			//_friendList.Height.Set(0f, 1f);
			//_friendList.ListPadding = 5f;
			//_onlinePlayerPanel.Append(_friendList);

			//// ScrollBar设定
			//var uiscrollbar = new UIAdvScrollBar();
			//uiscrollbar.SetView(100f, 1000f);
			//uiscrollbar.Height.Set(0f, 1f);
			//uiscrollbar.HAlign = 1f;
			//_onlinePlayerPanel.Append(uiscrollbar);
			//_friendList.SetScrollbar(uiscrollbar);

			uIPlayerProfileHead = new UIPlayerProfileHead();
			uIPlayerProfileHead.Top.Set(PLAYER_IMAGE_OFFSET_Y, 0f);
			uIPlayerProfileHead.Left.Set(PLAYER_IMAGE_OFFSET_X - 20, 0f);
			uIPlayerProfileHead.Width.Set(280, 0f);
			uIPlayerProfileHead.Height.Set(300, 0f);
			WindowPanel.Append(uIPlayerProfileHead);
		}

		private void GenerateSetting(UIAdvPanel panel)
		{
			UIText label = new UIText("设置");
			label.Top.Set(-20, 0f);
			label.Left.Set(0, 0f);
			panel.Append(label);

			UIText labelDisplayAboveHead = new UIText("显示段位标记");
			labelDisplayAboveHead.Top.Set(10, 0f);
			labelDisplayAboveHead.Left.Set(10, 0f);
			panel.Append(labelDisplayAboveHead);

			aboveHeadSwitch = new UISwitch();
			aboveHeadSwitch.Top.Set(0, 0f);
			aboveHeadSwitch.Left.Set(-80, 1f);
			aboveHeadSwitch.Width.Set(60f, 0f);
			aboveHeadSwitch.Height.Set(30f, 0f);
			aboveHeadSwitch.OnSwitch += AboveHeadSwitch_OnSwitch;
			panel.Append(aboveHeadSwitch);
		}

		private void AboveHeadSwitch_OnSwitch(UIElement element, bool state)
		{
			Main.LocalPlayer.GetModPlayer<MPlayer>().ShowOverHead = state;
			if (Main.netMode == 1)
				MessageSender.SyncModPlayerInfo(-1, -1, Main.LocalPlayer.GetModPlayer<MPlayer>());
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			
		}

		private void RefreshButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (Main.netMode == 1)
			{
				MessageSender.SendGetFriends();
			}
			_relaxTimer = 180;
			_rotation = 0f;
		}

		public void SetProfile(JsonData.SimplifiedPlayerInfo info)
		{
			uIPlayerProfileHead.SetPlayer(info);
		}



		public void RefreshFriends()
		{
			//uIFriendBars.Clear();
			//_friendList.Clear();

			if (Main.netMode == 1)
			{
				MessageSender.SendGetFriends();
			}
			_relaxTimer = 180;
			_rotation = 0f;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (Main.netMode == 0)
			{
				var info = new JsonData.SimplifiedPlayerInfo
				{
					Name = Main.LocalPlayer.name,
					IsFriend = true,
					IsLogin = true,
					Rank = 1500,
					KillCount = 50,
					ChatPrefix = "公民",
					ChatColor = Color.Red,
					RegistedTime = DateTime.Now
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

		//public void AppendFriends(JsonData.SimplifiedPlayerInfo info)
		//{
		//	var bar = new UIFriendBar(info);
		//	uIFriendBars.Add(bar);
		//	_friendList.Add(bar);
		//}




		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.HomePage);
		}
	}
}
