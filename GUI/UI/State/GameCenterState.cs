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
using ServerSideCharacter2.JsonData;

namespace ServerSideCharacter2.GUI.UI
{
	public class GameCenterState : AdvWindowUIState
	{
		public static GameCenterState Instance;
		private int _relaxTimer;
		private float _rotation;
		private UIAdvList _matchedGameList;
		private UIButton refreshButton;
		private UIText onlinelabel;


		private const float WINDOW_WIDTH = 480;
		private const float WINDOW_HEIGHT = 480;
		private const float MATCH_LIST_WIDTH = 400;
		private const float MATCH_LIST_HEIGHT = 360;
		private const float MATCHLIST_OFFSET_RIGHT = 0;
		private const float MATCHLIST_OFFSET_TOP = 35;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;

		public GameCenterState()
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

			var matchlistPanel = new UIAdvPanel(Drawing.Box1);
			matchlistPanel.Top.Set(-MATCH_LIST_HEIGHT / 2 + MATCHLIST_OFFSET_TOP, 0.5f);
			matchlistPanel.Left.Set(-MATCH_LIST_WIDTH / 2 + MATCHLIST_OFFSET_RIGHT, 0.5f);
			matchlistPanel.Width.Set(MATCH_LIST_WIDTH, 0f);
			matchlistPanel.Height.Set(MATCH_LIST_HEIGHT, 0f);
			matchlistPanel.SetPadding(10f);

			_matchedGameList = new UIAdvList();
			_matchedGameList.Width.Set(-25f, 1f);
			_matchedGameList.Height.Set(0f, 1f);
			_matchedGameList.ListPadding = 5f;
			matchlistPanel.Append(_matchedGameList);

			// ScrollBar设定
			var uiscrollbar = new UIAdvScrollBar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			matchlistPanel.Append(uiscrollbar);
			_matchedGameList.SetScrollbar(uiscrollbar);
			WindowPanel.Append(matchlistPanel);

			refreshButton = new UIButton(ServerSideCharacter2.ModTexturesTable["Refresh"], false);
			refreshButton.Top.Set(55, 0f);
			refreshButton.Left.Set(-35 / 2 - 65, 1f);
			refreshButton.Width.Set(35, 0f);
			refreshButton.Height.Set(35, 0f);
			refreshButton.OnClick += RefreshButton_OnClick;
			refreshButton.ButtonDefaultColor = new Color(200, 200, 200);
			refreshButton.ButtonChangeColor = Color.White;
			refreshButton.UseRotation = true;
			refreshButton.TextureScale = 0.2f;
			refreshButton.Tooltip = "刷新";
			WindowPanel.Append(refreshButton);

			onlinelabel = new UIText("游戏大厅");
			onlinelabel.Top.Set(-MATCH_LIST_HEIGHT / 2 + MATCHLIST_OFFSET_TOP - 25f, 0.5f);
			var texSize = Main.fontMouseText.MeasureString(onlinelabel.Text);
			onlinelabel.Left.Set(-MATCH_LIST_WIDTH / 2 + MATCHLIST_OFFSET_RIGHT, 0.5f);
			WindowPanel.Append(onlinelabel);
		}

		private void RefreshButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			RefreshGames();
		}

		//private void RefreshButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		//{
		//	RefreshFriends();
		//}


		public void RefreshGames()
		{
			//uIFriendBars.Clear();
			_matchedGameList.Clear();

			if (Main.netMode == 1)
			{
				MessageSender.SendGetGames();
			}
			else
			{
				for (var i = 0; i < 20; i++)
				{
					var testinfo = new JsonData.SimplifiedMatchInfo
					{
						Name = ServerUtils.RandomGenString(),
						IsMatching = Main.rand.Next(2) == 0 ? true : false,
						IsGameStarted = Main.rand.Next(2) == 0 ? true : false,
						MatchedPlayers = 5,
						MaxPlayers = 10
					};
					var bar = new UIMatchGameBar(testinfo);
					_matchedGameList.Add(bar);
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

		public void AppendMatches(MatchInfo info)
		{
			foreach(var match in info.Matches)
			{
				UIMatchGameBar gameBar = new UIMatchGameBar(match);
				_matchedGameList.Add(gameBar);
			}
		}

		public void ClearMatches()
		{
			_matchedGameList.Clear();
		}





		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.GameCenterPage);
		}
	}
}
