using Microsoft.Xna.Framework;
using ServerSideCharacter2.GUI.UI.Component;
using ServerSideCharacter2.GUI.UI.Component.Special;
using ServerSideCharacter2.Utils;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using System.Collections.Generic;
using ServerSideCharacter2.Unions;
using Microsoft.Xna.Framework.Graphics;
using ServerSideCharacter2.UI.Coroutines;
using Terraria.ID;

namespace ServerSideCharacter2.GUI.UI
{
	public class LotteryState : AdvWindowUIState
	{
		public static LotteryState Instance;
		private int _relaxTimer;
		private float _rotation;

		private UIAdvPanel unionsPanel;
		private UIAdvGrid _itemGrid;
		private UIImageResizable chestImage;
		private Coroutine uiCorotine;

		private float windowWidth = 730;
		private float windowHeight = 520;
		private const float UNIONLIST_WIDTH = 500;
		private const float UNIONLIST_HEIGHT = 400;
		private const float UNIONLIST_OFFSET_RIGHT = 32;
		private const float UNIONLIST_OFFSET_TOP = 100;

		private const float CANDIDATE_OFFSET_RIGHT = 580;
		private const float CANDIDATE_OFFSET_TOP = 100;
		private const float CANDIDATE_WIDTH = 240;
		private const float BAR_WIDTH = 280;
		private const float BAR_HEIGHT = 16;



		public LotteryState()
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

			chestImage = new UIImageResizable(ServerSideCharacter2.ModTexturesTable["GoldChest"]);
			chestImage.FrameCount = 3;
			chestImage.UsePosition = false;
			chestImage.VAlign = 1f;
			chestImage.HAlign = 0.5f;
			chestImage.MarginBottom = 10f;
			unionsPanel.Append(chestImage);

			//_itemGrid = new UIAdvGrid();
			//_itemGrid.Width.Set(-25f, 1f);
			//_itemGrid.Height.Set(0f, 1f);
			//_itemGrid.ListPadding = 5f;
			//unionsPanel.Append(_itemGrid);
		}

		public void StartAnimation(List<Item> lotteryItems)
		{
			uiCorotine = new Coroutine(_lotteryTask(lotteryItems));
		}

		private List<UISlot> listItems = new List<UISlot>();

		private IEnumerator<ICoroutineInstruction> _lotteryTask(List<Item> items)
		{
			listItems.Clear();
			for (int i = 0; i < 3; i++)
			{
				chestImage.Frame = i;
				yield return new WaitForFrames(20);
			}
			for(int i = 0; i < items.Count; i++)
			{
				var dim = chestImage.GetDimensions().Center();
				UISlot slot = new UISlot();
				slot.Width.Set(50, 0f);
				slot.Height.Set(50, 0f);
				slot.Top.Pixels = dim.Y - 25f;
				slot.Left.Pixels = dim.X - 25f;
				slot.ContainedItem = items[i].Clone();
				unionsPanel.Append(slot);
				listItems.Add(slot);
				Vector2 targetpos = new Vector2(5 + 55 * i, 5);
				Vector2 startPos = new Vector2(dim.X - 25f - unionsPanel.GetDimensions().X, dim.Y - 25f - unionsPanel.GetDimensions().Y);
				for(int j = 0; j <= 30; j++)
				{
					float factor = j / 30f;
					Vector2 pos = Vector2.Lerp(startPos, targetpos, factor);
					slot.Top.Pixels = pos.Y;
					slot.Left.Pixels = pos.X;
					slot.Opacity = factor;
					slot.Recalculate();
					yield return new SkipFrame();
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			
		}


		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (uiCorotine == null) return;
			uiCorotine.Update();
		}





		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.LotteryPage);
			foreach(var item in listItems)
			{
				unionsPanel.RemoveChild(item);
			}
		}
	}
}
