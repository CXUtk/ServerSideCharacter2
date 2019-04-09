using Microsoft.Xna.Framework;
using ServerSideCharacter2.GUI.UI.Component;
using ServerSideCharacter2.GUI.UI.Component.Special;
using ServerSideCharacter2.Utils;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using System.Collections.Generic;

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


		private float windowWidth = 600;
		private float windowHeight = 500;
		private const float UNIONLIST_WIDTH = 400;
		private const float UNIONLIST_HEIGHT = 360;
		private const float UNIONLIST_OFFSET_RIGHT = 32;
		private const float UNIONLIST_OFFSET_TOP = 120;

		private const float CANDIDATE_OFFSET_RIGHT = 580;
		private const float CANDIDATE_OFFSET_TOP = 120;
		private const float CANDIDATE_WIDTH = 240;

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
			buttonPanel.Height.Set(UNIONLIST_HEIGHT, 0f);
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
			unionNameText.Top.Set(UNIONLIST_OFFSET_TOP - 40, 0f);
			unionNameText.Left.Set(UNIONLIST_OFFSET_RIGHT + 5, 0f);
			WindowPanel.Append(unionNameText);
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
					var bar = new UIUnionMemberBar(testinfo, false);
					_memberList.Add(bar);
				}
				var ownerinfo = new JsonData.SimplifiedPlayerInfo
				{
					Name = "Skirt",
					IsLogin = true,
				};
				_memberList.Add(new UIUnionMemberBar(ownerinfo, true));
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
			_memberList.Add(new UIUnionMemberBar(info.Owner, true));
			foreach (var member in info.Members)
			{
				if(member.Name != info.Owner.Name)
					_memberList.Add(new UIUnionMemberBar(member, false));
			}
			_memberList.Sort();
			unionNameText.SetText(info.Name);
			AdjustOwnerUI(info.Owner.Name == Main.LocalPlayer.name);
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
		}

		private void ExitButton_OnClick1(UIMouseEvent evt, UIElement listeningElement)
		{
			throw new NotImplementedException();
		}

		private void DonateButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			throw new NotImplementedException();
		}

		private void ExitButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			throw new NotImplementedException();
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
