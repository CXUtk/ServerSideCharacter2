﻿using Microsoft.Xna.Framework;
using ServerSideCharacter2.GUI.UI.Component;
using ServerSideCharacter2.GUI.UI.Component.Special;
using ServerSideCharacter2.Utils;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using System.Collections.Generic;
using ServerSideCharacter2.Unions;

namespace ServerSideCharacter2.GUI.UI
{
	public class UnionPageState2 : AdvWindowUIState
	{
		public static UnionPageState2 Instance;
		private int _relaxTimer;
		private float _rotation;
		private UIAdvList _memberList;

		private UIAdvPanel unionsPanel;
		private UIButton refreshButton;
		private UIButton createUnionButton;
		private UIText unionNameText;
		private UIAdvList _buttonList;
		private UISlot uiSlot;
		private UIBar expBar;
		private UIText unionLevelText;
		private UIText unionWealthText;


		private float windowWidth = 640;
		private float windowHeight = 500;
		private const float UNIONLIST_WIDTH = 460;
		private const float UNIONLIST_HEIGHT = 360;
		private const float UNIONLIST_OFFSET_RIGHT = 32;
		private const float UNIONLIST_OFFSET_TOP = 120;

		private const float CANDIDATE_OFFSET_RIGHT = 580;
		private const float CANDIDATE_OFFSET_TOP = 120;
		private const float CANDIDATE_WIDTH = 240;
		private const float BAR_WIDTH = 280;
		private const float BAR_HEIGHT = 16;



		public UnionPageState2()
		{
			Instance = this;
		}


		protected override void Initialize(UIAdvPanel WindowPanel)
		{
			WindowPanel.OverflowHidden = true;
			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - windowWidth / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - windowHeight / 2, 0f);
			WindowPanel.Width.Set(windowWidth, 0f);
			WindowPanel.Height.Set(windowHeight, 0f);
			WindowPanel.Color = Color.White * 0.8f;

			unionsPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				OverflowHidden = true
			};
			unionsPanel.Top.Set(UNIONLIST_OFFSET_TOP, 0f);
			unionsPanel.Left.Set(UNIONLIST_OFFSET_RIGHT, 0f);
			unionsPanel.Width.Set(UNIONLIST_WIDTH, 0f);
			unionsPanel.Height.Set(UNIONLIST_HEIGHT, 0f);
			unionsPanel.SetPadding(10f);

			WindowPanel.Append(unionsPanel);


			refreshButton = new UIButton(ServerSideCharacter2.ModTexturesTable["Refresh"], false);
			refreshButton.Top.Set(UNIONLIST_OFFSET_TOP - 50, 0f);
			refreshButton.Left.Set(UNIONLIST_OFFSET_RIGHT + UNIONLIST_WIDTH - 35, 0f);
			refreshButton.Width.Set(35, 0f);
			refreshButton.Height.Set(35, 0f);
			refreshButton.ButtonDefaultColor = new Color(200, 200, 200);
			refreshButton.ButtonChangeColor = Color.White;
			refreshButton.UseRotation = true;
			refreshButton.TextureScale = 0.2f;
			refreshButton.Tooltip = "刷新";
			refreshButton.OnClick += RefreshButton_OnClick;
			WindowPanel.Append(refreshButton);

			var returnButton = new UICDButton(null, true);
			returnButton.Top.Set(UNIONLIST_OFFSET_TOP - 50, 0f);
			returnButton.Left.Set(UNIONLIST_OFFSET_RIGHT + UNIONLIST_WIDTH + 20, 0f);
			returnButton.Width.Set(105f, 0f);
			returnButton.Height.Set(50f, 0f);
			returnButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
			returnButton.ButtonDefaultColor = new Color(200, 200, 200);
			returnButton.ButtonChangeColor = Color.White;
			returnButton.CornerSize = new Vector2(12, 12);
			returnButton.ButtonText = "返回大厅";
			returnButton.OnClick += ReturnButton_OnClick;
			WindowPanel.Append(returnButton);

			_memberList = new UIAdvList();
			_memberList.Width.Set(-25f, 1f);
			_memberList.Height.Set(0f, 1f);
			_memberList.ListPadding = 5f;
			_memberList.OverflowHidden = true;
			unionsPanel.Append(_memberList);



			var buttonPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				OverflowHidden = true
			};
			buttonPanel.Top.Set(UNIONLIST_OFFSET_TOP, 0f);
			buttonPanel.Left.Set(UNIONLIST_OFFSET_RIGHT + UNIONLIST_WIDTH + 10, 0f);
			buttonPanel.Width.Set(150, 0f);
			buttonPanel.Height.Set(240, 0f);
			buttonPanel.SetPadding(10f);
			buttonPanel.Visible = false;
			WindowPanel.Append(buttonPanel);

			_buttonList = new UIAdvList();
			_buttonList.Width.Set(-25f, 1f);
			_buttonList.Height.Set(0f, 1f);
			_buttonList.ListPadding = 5f;
			buttonPanel.Append(_buttonList);

			// ScrollBar设定
			var uiscrollbar = new UIAdvScrollBar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			unionsPanel.Append(uiscrollbar);
			_memberList.SetScrollbar(uiscrollbar);


			unionNameText = new UIText("", 0.7f, true);
			unionNameText.Top.Set(UNIONLIST_OFFSET_TOP - 100, 0f);
			unionNameText.Left.Set(UNIONLIST_OFFSET_RIGHT + 5, 0f);
			WindowPanel.Append(unionNameText);

			//expBar = new UIBar
			//{
			//	BarFrameTex = ServerSideCharacter2.ModTexturesTable["ExpBarFrame"],
			//	BarFillTex = Main.magicPixel,
			//	FillerColor = Color.Yellow,
			//	BackGroundColor = Color.Transparent,
			//	BarFrameTexCornerSize = new Vector2(6, 6),
			//	FillerDrawOffset = new Vector2(6, 6),
			//	FillerSize = new Vector2(BAR_WIDTH - 12, BAR_HEIGHT - 12)
			//};
			//expBar.Top.Set(80f, 0f);
			//expBar.Left.Set(40, 0f);
			//expBar.Width.Set(BAR_WIDTH, 0f);
			//expBar.Height.Set(BAR_HEIGHT, 0f);
			//expBar.Value = 0.3f;
			//WindowPanel.Append(expBar);

			unionLevelText = new UIText("");
			unionLevelText.Top.Set(60f, 0f);
			unionLevelText.Left.Set(40, 0f);
			unionLevelText.SetText($"等级: {8} EXP: {10000000} / {30000000}");
			WindowPanel.Append(unionLevelText);

			unionWealthText = new UIText("");
			unionWealthText.Top.Set(90f, 0f);
			unionWealthText.Left.Set(40, 0f);
			unionWealthText.SetText($"财富值: 1000000");
			WindowPanel.Append(unionWealthText);

			//uiSlot = new UISlot(ServerSideCharacter2.ModTexturesTable["AdvInvBack1"]);
			//uiSlot.Left.Set(475, 0f);
			//uiSlot.Top.Set(340, 0f);
			//uiSlot.Width.Set(60, 0f);
			//uiSlot.Height.Set(60, 0f);
			//uiSlot.CanPutInSlot = new CheckPutSlotCondition((item) =>
			//{
			//	return item.type == UnionManager.CurrencyType;
			//});
			//uiSlot.Tooltip = "在这放置咕币来捐献";
			//uiSlot.DrawColor = Color.White;
			//WindowPanel.Append(uiSlot);

		}

		private void ReturnButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionPage);
			UnionPageState.Instance.RefreshUnions();
		}

		private void RefreshButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			RefreshUnion();
		}

		public void RefreshUnion()
		{
			_memberList.Clear();
			if (Main.netMode == 1)
			{
				MessageSender.GetComplexUnionData();
			}
			else
			{
				for (var i = 0; i < 19; i++)
				{
					var testinfo = new JsonData.SimplifiedPlayerInfo
					{
						Name = ServerUtils.RandomGenString(),
						IsLogin = Main.rand.NextBool(),
					};
					var bar = new UIUnionMemberBar(testinfo, Main.rand.Next(2) == 0 ? UnionPosition.会员 : UnionPosition.建筑师, Main.rand.Next(10000000));
					_memberList.Add(bar);
				}
				var ownerinfo = new JsonData.SimplifiedPlayerInfo
				{
					Name = "Skirt",
					IsLogin = true,
				};
				_memberList.Add(new UIUnionMemberBar(ownerinfo, UnionPosition.会长, Main.rand.Next(10000000)));
				_memberList.Sort();
				unionNameText.SetText("裙中世界");
				AdjustOwnerUI(true);
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
		public void ClearMembers()
		{
			_memberList.Clear();
		}



		public void Apply(JsonData.ComplexUnionInfo info)
		{
			_memberList.Add(new UIUnionMemberBar(info.Owner, UnionPosition.会长, info.Donation[info.Owner.Name]));
			foreach (var member in info.Members)
			{
				if (member.Name != info.Owner.Name)
				{
					var pos = UnionPosition.会员;
					if (info.Builders.Contains(member.Name))
					{
						pos = UnionPosition.建筑师;
					}
					_memberList.Add(new UIUnionMemberBar(member, pos, info.Donation[member.Name]));
				}
			}
			_memberList.Sort();
			unionNameText.SetText(info.Name);
			AdjustOwnerUI(info.Owner.Name == Main.LocalPlayer.name);
			// expBar.Value = (float)(info.CurrentEXP / (double)info.EXPToNext);
			unionLevelText.SetText($"等级: {info.Level} EXP: {info.CurrentEXP} / {info.EXPToNext}");
			unionWealthText.SetText($"财富值：{info.Wealth}");
		}


		private void AdjustOwnerUI(bool owner)
		{
			_buttonList.Clear();
			if (owner)
			{
				var candidateButton = new UICDButton(null, true);
				candidateButton.Width.Set(0, 1f);
				candidateButton.Height.Set(50f, 0f);
				candidateButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
				candidateButton.ButtonDefaultColor = new Color(200, 200, 200);
				candidateButton.ButtonChangeColor = Color.White;
				candidateButton.CornerSize = new Vector2(12, 12);
				candidateButton.ButtonText = "申请信息";
				candidateButton.OnClick += CandidateButton_OnClick;
				_buttonList.Add(candidateButton);

				var exitButton = new UICDButton(null, true);
				exitButton.Width.Set(0, 1f);
				exitButton.Height.Set(50f, 0f);
				exitButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
				exitButton.ButtonDefaultColor = new Color(200, 200, 200);
				exitButton.ButtonChangeColor = Color.White;
				exitButton.CornerSize = new Vector2(12, 12);
				exitButton.ButtonText = "解散";
				exitButton.OnClick += ExitButton_OnClick1;
				_buttonList.Add(exitButton);
			}
			else
			{
				var exitButton = new UICDButton(null, true);
				exitButton.Width.Set(0, 1f);
				exitButton.Height.Set(50f, 0f);
				exitButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
				exitButton.ButtonDefaultColor = new Color(200, 200, 200);
				exitButton.ButtonChangeColor = Color.White;
				exitButton.CornerSize = new Vector2(12, 12);
				exitButton.ButtonText = "退出";
				exitButton.OnClick += ExitButton_OnClick;
				_buttonList.Add(exitButton);
			}
			var donateButton = new UICDButton(null, true);
			donateButton.Width.Set(0, 1f);
			donateButton.Height.Set(50, 0f);
			donateButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
			donateButton.ButtonDefaultColor = new Color(200, 200, 200);
			donateButton.ButtonChangeColor = Color.White;
			donateButton.CornerSize = new Vector2(12, 12);
			donateButton.ButtonText = "捐献";
			donateButton.OnClick += DonateButton_OnClick;
			_buttonList.Add(donateButton);

			var tpRegionButton = new UICDButton(null, true);
			tpRegionButton.Width.Set(0, 1f);
			tpRegionButton.Height.Set(50, 0f);
			tpRegionButton.CoolDownCount = 60;
			tpRegionButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
			tpRegionButton.ButtonDefaultColor = new Color(200, 200, 200);
			tpRegionButton.ButtonChangeColor = Color.White;
			tpRegionButton.CornerSize = new Vector2(12, 12);
			tpRegionButton.ButtonText = "传送领地";
			tpRegionButton.OnClick += TpRegionButton_OnClick;
			_buttonList.Add(tpRegionButton);
		}

		private void TpRegionButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			MessageSender.SendTPUnionRegion();
		}

		private void ExitButton_OnClick1(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ShowMessage("请使用/union remove <公会名字> 进行公会解散", 360, Color.Yellow);
		}

		private void DonateButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionDonationPage);
		}

		private void ExitButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ShowMessage("请使用/union exit <公会名字> 来退出公会", 360, Color.Yellow);
		}

		private void CandidateButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionCandidatePage);
		}


		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionPage2);
		}
	}
}
