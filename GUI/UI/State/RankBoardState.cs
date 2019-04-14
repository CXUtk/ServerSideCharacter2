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
using ServerSideCharacter2.RankingSystem;

namespace ServerSideCharacter2.GUI.UI
{
	public class RankBoardState : AdvWindowUIState
	{
		public static RankBoardState Instance;
		private int _relaxTimer;
		private float _rotation;
		private UIAdvList _onlinePlayerList;

		private UIAdvPanel _onlinePlayerPanel;
		private UIButton refreshButton;
		private UIText onlinelabel;
		private UIText distanceToEnd;
		private DateTime seasonEndTime;

		private const float WINDOW_WIDTH = 660;
		private const float WINDOW_HEIGHT = 480;
		private const float FRIENDLIST_WIDTH = 400;
		private const float FRIENDLIST_HEIGHT = 360;
		private const float FRIENDLIST_OFFSET_RIGHT = -110;
		private const float FRIENDLIST_OFFSET_TOP = 40;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;

		public RankBoardState()
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

			_onlinePlayerPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				OverflowHidden = true
			};
			_onlinePlayerPanel.Top.Set(-FRIENDLIST_HEIGHT / 2 + FRIENDLIST_OFFSET_TOP, 0.5f);
			_onlinePlayerPanel.Left.Set(-FRIENDLIST_WIDTH / 2 + FRIENDLIST_OFFSET_RIGHT, 0.5f);
			_onlinePlayerPanel.Width.Set(FRIENDLIST_WIDTH, 0f);
			_onlinePlayerPanel.Height.Set(FRIENDLIST_HEIGHT, 0f);
			_onlinePlayerPanel.SetPadding(10f);

			onlinelabel = new UIText("排位积分榜（每天更新）");
			onlinelabel.Top.Set(-FRIENDLIST_HEIGHT / 2 + FRIENDLIST_OFFSET_TOP - 25f, 0.5f);
			var texSize = Main.fontMouseText.MeasureString(onlinelabel.Text);
			onlinelabel.Left.Set(-FRIENDLIST_WIDTH / 2 + FRIENDLIST_OFFSET_RIGHT, 0.5f);
			WindowPanel.Append(onlinelabel);
			WindowPanel.Append(_onlinePlayerPanel);

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

			_onlinePlayerList = new UIAdvList();
			_onlinePlayerList.Width.Set(-25f, 1f);
			_onlinePlayerList.Height.Set(0f, 1f);
			_onlinePlayerList.ListPadding = 5f;
			_onlinePlayerList.OverflowHidden = true;
			_onlinePlayerPanel.Append(_onlinePlayerList);

			// ScrollBar设定
			var uiscrollbar = new UIAdvScrollBar();
			// uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			_onlinePlayerPanel.Append(uiscrollbar);
			_onlinePlayerList.SetScrollbar(uiscrollbar);


			distanceToEnd = new UIText("00:00:00", 0.6f, true);
			distanceToEnd.Top.Set(20f, 0f);
			distanceToEnd.Left.Set(20f, 0f);
			WindowPanel.Append(distanceToEnd);

			UIAdvPanel infoPanel = new UIAdvPanel();
			infoPanel.Top.Set(-FRIENDLIST_HEIGHT / 2 + FRIENDLIST_OFFSET_TOP, 0.5f);
			infoPanel.Left.Set(440, 0f);
			infoPanel.Width.Set(200, 0f);
			infoPanel.Height.Set(FRIENDLIST_HEIGHT, 0f);
			WindowPanel.Append(infoPanel);

			var announcement = new UIMessageBox(GameLanguage.GetText("rankannouncement"));
			announcement.Width.Set(-25f, 1f);
			announcement.Height.Set(0f, 1f);
			infoPanel.Append(announcement);

			var uiscrollbar2 = new UIAdvScrollBar();
			uiscrollbar2.SetView(100f, 1000f);
			uiscrollbar2.Height.Set(-20f, 1f);
			uiscrollbar2.VAlign = 0.5f;
			uiscrollbar2.HAlign = 1f;
			infoPanel.Append(uiscrollbar2);
			announcement.SetScrollbar(uiscrollbar2);
		}


		private void RefreshButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			RefreshBoard();
		}


		public void RefreshBoard()
		{
			_onlinePlayerList.Clear();

			if (Main.netMode == 1)
			{
				MessageSender.SendRequestRankBoard();
			}
			else
			{
				List<SimplifiedPlayerInfo> list = new List<SimplifiedPlayerInfo>();
				for (var i = 0; i < 20; i++)
				{
					var testinfo = new SimplifiedPlayerInfo()
					{
						Name = ServerUtils.RandomGenString(),
						Rank = Main.rand.Next(1000) + 1000,
						KillCount = Main.rand.Next(100),
						IsFriend = true,
					};
					list.Add(testinfo);
				}
				list.Sort(SimplifiedPlayerInfo.CompareA);
				list.Reverse();
				int k = 1;
				foreach (var info in list)
				{
					var bar = new UIRankBoardPlayerBar(info, k);
					_onlinePlayerList.Add(bar);
					k++;
				}
				seasonEndTime = DateTime.Now.AddDays(5);
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
			TimeSpan ts = seasonEndTime - DateTime.Now;
			
			distanceToEnd.SetText($"距离赛季结束：{ts:dd\\:hh\\:mm\\:ss}");
		}


		public void Apply(RankData rankdata)
		{
			int k = 1;
			foreach (var player in rankdata.LastBoard)
			{
				var bar = new UIRankBoardPlayerBar(player, k++);
				_onlinePlayerList.Add(bar);
			}
			seasonEndTime = rankdata.RankSeasonEndTime;
		}


		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.RankBoard);
		}
	}
}
