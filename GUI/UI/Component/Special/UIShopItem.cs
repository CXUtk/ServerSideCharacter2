using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ReLogic.Graphics;
using Terraria.GameInput;
using ReLogic.OS;
using Microsoft.Xna.Framework.Input;
using Terraria.UI.Chat;
using ServerSideCharacter2.JsonData;
using System;
using System.Collections.Generic;
using ServerSideCharacter2.Mailing;
using Terraria.Graphics;

namespace ServerSideCharacter2.GUI.UI.Component.Special
{
	public class UIShopItem : UIAdvPanel
	{

		private readonly SimplifiedMarketItem marketItem;
		private int _innerCD = 0;
		private readonly UISimpleSlot itemslot;
		private readonly UIText gucoinText;
		private bool isExpanded = false;

		public UIShopItem(SimplifiedMarketItem info)
		{
			marketItem = info;
			this.Width.Set(140f, 0f);
			this.Height.Set(170f, 0f);
			this.CornerSize = new Vector2(8, 8);
			base.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			this.Color = Color.Cyan * 0.8f;
			base.SetPadding(6f);
			this.OverflowHidden = true;

			itemslot = new UISimpleSlot(info.ItemID);
			itemslot.CanPick = false;
			itemslot.Width.Set(50, 0f);
			itemslot.Height.Set(50, 0f);
			itemslot.Top.Set(20, 0f);
			itemslot.Left.Set(-25f, 0.5f);
			Append(itemslot);

			gucoinText = new UIText(info.Price.ToString());
			gucoinText.MarginBottom = 10f;
			gucoinText.PaddingLeft = 15f;
			gucoinText.VAlign = 1f;
			gucoinText.HAlign = 0.5f;
			Append(gucoinText);

			var buyButton = new UICDButton(null);
			buyButton.Top.Set(80f, 0f);
			buyButton.Left.Set(-40, 0.5f);
			buyButton.Width.Set(80f, 0f);
			buyButton.Height.Set(35f, 0f);
			buyButton.ButtonText = "购买";
			buyButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			buyButton.CornerSize = new Vector2(12, 12);
			buyButton.ButtonDefaultColor = new Color(200, 200, 200);
			buyButton.ButtonChangeColor = Color.White;
			buyButton.OnClick += BuyButton_OnClick;
			Append(buyButton);
		}

		private void BuyButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (!ServerSideCharacter2.GuiManager.IsActive(SSCUIState.NormalShopBuyPage))
			{
				ServerSideCharacter2.GuiManager.SetState(SSCUIState.NormalShopBuyPage, true);
			}
			NormalShopBuyState.Instance.SetItem(marketItem);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			Main.PlaySound(12, -1, -1, 1, 1f, 0f);
			this.Color = Color.Cyan * 1.2f;
			base.MouseOver(evt);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			this.Color = Color.Cyan * 0.8f;
			base.MouseOut(evt);
		}

		public override void Click(UIMouseEvent evt)
		{
			base.Click(evt);
			if (_innerCD > 0) return;
			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			var iconpos = gucoinText.GetDimensions().Position() + new Vector2(5f, gucoinText.GetDimensions().Height / 2);
			var tex = ServerSideCharacter2.ModTexturesTable["GuCoin"];
			spriteBatch.Draw(tex, iconpos, tex.Frame(1, 4, 0, frame), Color.White, 0f, new Vector2(tex.Width, tex.Height / 4f * 0.5f), 1f, SpriteEffects.None, 0f);
		}

		private int frameCounter = 0;
		private int frame = 0;
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (_innerCD > 0)
				_innerCD--;
			frameCounter++;
			if (frameCounter == 7)
			{
				frameCounter = 0;
				frame = (frame + 1) % 4;
			}
		}

	}
}
