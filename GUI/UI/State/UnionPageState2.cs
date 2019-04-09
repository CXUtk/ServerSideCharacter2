using Microsoft.Xna.Framework;
using ServerSideCharacter2.GUI.UI.Component;
using ServerSideCharacter2.GUI.UI.Component.Special;
using ServerSideCharacter2.Utils;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;

namespace ServerSideCharacter2.GUI.UI
{
	public class UnionPageState2 : AdvWindowUIState
	{
		public static UnionPageState2 Instance;
		private int _relaxTimer;
		private float _rotation;
		private UIAdvList _memberList;

		private UIPanel unionsPanel;
		private UIButton refreshButton;
		private UIButton createUnionButton;


		private const float WINDOW_WIDTH = 640;
		private const float WINDOW_HEIGHT = 500;
		private const float UNIONLIST_WIDTH = 400;
		private const float UNIONLIST_HEIGHT = 360;
		private const float UNIONLIST_OFFSET_RIGHT = -80;
		private const float UNIONLIST_OFFSET_TOP = 50;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;


		public UnionPageState2()
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

			unionsPanel = new UIPanel();
			unionsPanel.Top.Set(-UNIONLIST_HEIGHT / 2 + UNIONLIST_OFFSET_TOP, 0.5f);
			unionsPanel.Left.Set(-UNIONLIST_WIDTH / 2 + UNIONLIST_OFFSET_RIGHT, 0.5f);
			unionsPanel.Width.Set(UNIONLIST_WIDTH, 0f);
			unionsPanel.Height.Set(UNIONLIST_HEIGHT, 0f);

			WindowPanel.Append(unionsPanel);

			refreshButton = new UIButton(ServerSideCharacter2.ModTexturesTable["Refresh"], false);
			refreshButton.Top.Set(-UNIONLIST_HEIGHT / 2 + UNIONLIST_OFFSET_TOP - 50, 0.5f);
			refreshButton.Left.Set(UNIONLIST_OFFSET_RIGHT + UNIONLIST_WIDTH / 2 - 35, 0.5f);
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
			unionsPanel.Append(_memberList);

			// ScrollBar设定
			var uiscrollbar = new UIAdvScrollBar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			unionsPanel.Append(uiscrollbar);
			_memberList.SetScrollbar(uiscrollbar);

			createUnionButton = new UICDButton(null, true);
			createUnionButton.Top.Set(-UNIONLIST_HEIGHT / 2 + UNIONLIST_OFFSET_TOP, 0.5f);
			createUnionButton.Left.Set(-150, 1f);
			createUnionButton.Width.Set(100, 0f);
			createUnionButton.Height.Set(35, 0f);
			createUnionButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
			createUnionButton.ButtonDefaultColor = new Color(200, 200, 200);
			createUnionButton.ButtonChangeColor = Color.White;
			createUnionButton.CornerSize = new Vector2(12, 12);
			createUnionButton.ButtonText = "创建";
			createUnionButton.OnClick += CreateUnionButton_OnClick;
			WindowPanel.Append(createUnionButton);
		}

		private void CreateUnionButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if(ServerSideCharacter2.ClientUnion != null)
			{
				ServerSideCharacter2.Instance.ShowMessage("你已经加入公会了", 180, Color.OrangeRed);
				return;
			}
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionPage2);
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
				for (var i = 0; i < 20; i++)
				{
					var testinfo = new JsonData.SimplifiedPlayerInfo
					{
						Name = ServerUtils.RandomGenString(),
						IsLogin = Main.rand.NextBool(),
					};
					var bar = new UINormalPlayerBar(testinfo);
					_memberList.Add(bar);
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
		public void ClearMembers()
		{
			_memberList.Clear();
		}
		public void Apply(JsonData.ComplexUnionInfo info)
		{
			_memberList.Add(new UINormalPlayerBar(info.Owner));
			foreach (var member in info.Members)
			{
				if(member.Name != info.Owner.Name)
					_memberList.Add(new UINormalPlayerBar(member));
			}
		}


		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionPage2);
		}
	}
}
