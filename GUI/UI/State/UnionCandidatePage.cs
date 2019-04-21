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
	public class UnionCandidatePage : AdvWindowUIState
	{
		public static UnionCandidatePage Instance;
		public int UnreadCount;
		private int _relaxTimer;
		private float _rotation;
		private UIAdvList _candidateList;

		private UIAdvPanel candidatesPanel;
		private UIButton refreshButton;
		private UIButton createUnionButton;


		private const float WINDOW_WIDTH = 500;
		private const float WINDOW_HEIGHT = 500;
		private const float UNIONLIST_WIDTH = 400;
		private const float UNIONLIST_HEIGHT = 360;
		private const float UNIONLIST_OFFSET_RIGHT = 0;
		private const float UNIONLIST_OFFSET_TOP = 50;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;


		public UnionCandidatePage()
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

			candidatesPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				OverflowHidden = true
			};
			candidatesPanel.Top.Set(-UNIONLIST_HEIGHT / 2 + UNIONLIST_OFFSET_TOP, 0.5f);
			candidatesPanel.Left.Set(-UNIONLIST_WIDTH / 2 + UNIONLIST_OFFSET_RIGHT, 0.5f);
			candidatesPanel.Width.Set(UNIONLIST_WIDTH, 0f);
			candidatesPanel.Height.Set(UNIONLIST_HEIGHT, 0f);
			candidatesPanel.SetPadding(10f);

			WindowPanel.Append(candidatesPanel);

			_candidateList = new UIAdvList();
			_candidateList.Width.Set(-25f, 1f);
			_candidateList.Height.Set(0f, 1f);
			_candidateList.ListPadding = 5f;
			_candidateList.OverflowHidden = true;
			candidatesPanel.Append(_candidateList);

			// ScrollBar设定
			var uiscrollbar = new UIAdvScrollBar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			candidatesPanel.Append(uiscrollbar);
			_candidateList.SetScrollbar(uiscrollbar);


			var label = new UIText("公会申请信息", 0.7f, true);
			label.Top.Set(50, 0f);
			label.Left.Set(-UNIONLIST_WIDTH / 2 + UNIONLIST_OFFSET_RIGHT + 5, 0.5f);
			WindowPanel.Append(label);
		}

		

		public void AppendCandidates(JsonData.ComplexUnionInfo info)
		{
			_candidateList.Clear();
			foreach (var player in info.Requests)
			{
				_candidateList.Add(new UIUnionCandidateBar(player));
			}
			UnreadCount = _candidateList.Count;
		}


		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionCandidatePage);
		}
	}
}
