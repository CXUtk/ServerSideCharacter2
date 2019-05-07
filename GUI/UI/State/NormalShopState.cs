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

namespace ServerSideCharacter2.GUI.UI
{
	public class NormalShopState : AdvWindowUIState
	{
		public static NormalShopState Instance;
		private int _relaxTimer;
		private float _rotation;
		private UIAdvGrid _shopItemGrid;

		private UIAdvPanel unionsPanel;
		private UIButton refreshButton;
		private UIText currencyText;

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



		public NormalShopState()
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


			_shopItemGrid = new UIAdvGrid();
			_shopItemGrid.Width.Set(-25f, 1f);
			_shopItemGrid.Height.Set(0f, 1f);
			_shopItemGrid.ListPadding = 7f;
			_shopItemGrid.OverflowHidden = true;
			unionsPanel.Append(_shopItemGrid);


			// ScrollBar设定
			var uiscrollbar = new UIAdvScrollBar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			unionsPanel.Append(uiscrollbar);
			_shopItemGrid.SetScrollbar(uiscrollbar);

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

			var currencyPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				OverflowHidden = true
			};
			currencyPanel.Top.Set(UNIONLIST_OFFSET_TOP, 0f);
			currencyPanel.Left.Set(UNIONLIST_OFFSET_RIGHT + UNIONLIST_WIDTH + 15, 0f);
			currencyPanel.Width.Set(165, 0f);
			currencyPanel.Height.Set(80, 0f);
			currencyPanel.SetPadding(5f);
			var currencylabel = new UIText("当前货币");
			currencylabel.Top.Set(5, 0f);
			currencylabel.HAlign = 0.5f;
			currencyPanel.Append(currencylabel);

			currencyText = new UIText("1000");
			currencyText.TextColor = Color.Yellow;
			currencyText.MarginBottom = 10f;
			currencyText.PaddingLeft = 15f;
			currencyText.VAlign = 1f;
			currencyText.HAlign = 0.5f;
			currencyPanel.Append(currencyText);

			WindowPanel.Append(currencyPanel);
		}



		private void ReturnButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionPage);
			UnionPageState.Instance.RefreshUnions();
		}

		private void RefreshButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			RefreshItems();
		}

		public void RefreshItems()
		{
			_shopItemGrid.Clear();
			if (Main.netMode == 1)
			{
				MessageSender.SendGetNormalShop(-1);
			}
			else
			{
				for (var i = 0; i < 19; i++)
				{
					var testinfo = new JsonData.SimplifiedMarketItem
					{
						ItemID = Main.rand.Next(Main.itemTexture.Length),
						Price = Main.rand.Next(1, 10) * 10000,
						Discount = Main.rand.Next(0, 5) * 5,
					};
					var bar = new UIShopItem(testinfo);
					_shopItemGrid.Add(bar);
				}
			}
			_relaxTimer = 180;
			_rotation = 0f;
		}
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			var iconpos = currencyText.GetDimensions().Position() + new Vector2(5f, currencyText.GetDimensions().Height / 2);
			var tex = ServerSideCharacter2.ModTexturesTable["GuCoin"];
			spriteBatch.Draw(tex, iconpos, tex.Frame(1, 4, 0, frame), Color.White, 0f, new Vector2(tex.Width, tex.Height / 4f * 0.5f), 1f, SpriteEffects.None, 0f);
		}

		private int frameCounter = 0;
		private int frame = 0;
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
			frameCounter++;
			if (frameCounter == 7)
			{
				frameCounter = 0;
				frame = (frame + 1) % 4;
			}
		}
		public void ClearItems()
		{
			_shopItemGrid.Clear();
		}



		public void Apply(JsonData.MarketInfo info)
		{
			foreach(var item in info.Items)
			{
				_shopItemGrid.Add(new UIShopItem(item));
			}
			currencyText.SetText(info.PlayerCurrency.ToString());
		}


		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.NormalShopPage);
		}
	}
}
