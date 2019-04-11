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
	public class MailPageState : AdvWindowUIState
	{
		public static MailPageState Instance;
		private int _relaxTimer;
		private float _rotation;
		private UIAdvList _unionsList;

		private UIAdvPanel mailContentPanel;
		private UIAdvPanel outerContentPanel;
		private UIAdvPanel mailHeadPanel;
		private UIButton refreshButton;
		private UIButton createUnionButton;
		private UIMessageBox _mailContent;
		private UIText _uiTitle;
		private UIAdvGrid _uiItemGrid;


		private const float WINDOW_WIDTH = 860;
		private const float WINDOW_HEIGHT = 600;
		private const float UNIONLIST_WIDTH = 560;
		private const float UNIONLIST_HEIGHT = 400;
		private const float UNIONLIST_OFFSET_RIGHT = 110;
		private const float UNIONLIST_OFFSET_TOP = -20;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;

		public MailPageState()
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

			outerContentPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				Color = new Color(33, 43, 79) * 0.8f
			};
			outerContentPanel.Top.Set(-UNIONLIST_HEIGHT / 2 + UNIONLIST_OFFSET_TOP, 0.5f);
			outerContentPanel.Left.Set(-UNIONLIST_WIDTH / 2 + UNIONLIST_OFFSET_RIGHT, 0.5f);
			outerContentPanel.Width.Set(UNIONLIST_WIDTH, 0f);
			outerContentPanel.Height.Set(UNIONLIST_HEIGHT + 80, 0f);
			outerContentPanel.SetPadding(10f);
			WindowPanel.Append(outerContentPanel);


			mailContentPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				OverflowHidden = true,
				Color = new Color(33, 43, 79) * 0.8f
			};
			mailContentPanel.Top.Set(-UNIONLIST_HEIGHT / 2 + UNIONLIST_OFFSET_TOP, 0.5f);
			mailContentPanel.Left.Set(-UNIONLIST_WIDTH / 2 + UNIONLIST_OFFSET_RIGHT, 0.5f);
			mailContentPanel.Width.Set(UNIONLIST_WIDTH, 0f);
			mailContentPanel.Height.Set(UNIONLIST_HEIGHT, 0f);
			mailContentPanel.SetPadding(10f);
			mailContentPanel.Visible = false;

			WindowPanel.Append(mailContentPanel);

			_mailContent = new UIMessageBox("（空）");
			_mailContent.Width.Set(-25f, 1f);
			_mailContent.Height.Set(0f, 1f);
			mailContentPanel.Append(_mailContent);

			UIAdvScrollBar uiscrollbar = new UIAdvScrollBar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(-20f, 1f);
			uiscrollbar.VAlign = 0.5f;
			uiscrollbar.HAlign = 1f;
			mailContentPanel.Append(uiscrollbar);
			_mailContent.SetScrollbar(uiscrollbar);

			AddItemSlots();

			// 上方标题
			_uiTitle = new UIText("标题", 0.6f, true);
			_uiTitle.Top.Set(-70f, 0f);
			_uiTitle.SetPadding(15f);
			outerContentPanel.Append(_uiTitle);




			mailHeadPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				OverflowHidden = true,
				Color = new Color(33, 43, 79) * 0.8f
			};
			mailHeadPanel.Top.Set(-UNIONLIST_HEIGHT / 2 + UNIONLIST_OFFSET_TOP, 0.5f);
			mailHeadPanel.Left.Set(-UNIONLIST_WIDTH / 2 + UNIONLIST_OFFSET_RIGHT - 230, 0.5f);
			mailHeadPanel.Width.Set(200, 0f);
			mailHeadPanel.Height.Set(UNIONLIST_HEIGHT + 80, 0f);
			mailHeadPanel.SetPadding(10f);
			WindowPanel.Append(mailHeadPanel);




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

		
		}

		private void AddItemSlots()
		{
			throw new NotImplementedException();
		}

		internal void GetMailList()
		{
			var itemSlotPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				OverflowHidden = true,
				Color = new Color(33, 43, 79) * 0.8f
			};
			itemSlotPanel.Top.Set(-UNIONLIST_HEIGHT / 2 + UNIONLIST_OFFSET_TOP, 0.5f);
			itemSlotPanel.Left.Set(-UNIONLIST_WIDTH / 2 + UNIONLIST_OFFSET_RIGHT, 0.5f);
			itemSlotPanel.Width.Set(UNIONLIST_WIDTH, 0f);
			itemSlotPanel.Height.Set(UNIONLIST_HEIGHT, 0f);
			itemSlotPanel.SetPadding(10f);
			itemSlotPanel.Visible = false;

			_uiItemGrid = new UIAdvGrid();
			_uiItemGrid.Width.Set(-25f, 1f);
			_uiItemGrid.Height.Set(0f, 1f);
			_uiItemGrid.ListPadding = 5f;
			itemSlotPanel.Append(_uiItemGrid);
		}

		private void RefreshButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			throw new NotImplementedException();
		}

		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.MailPage);
		}
	}
}
