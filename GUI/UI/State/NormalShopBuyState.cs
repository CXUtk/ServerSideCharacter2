using Microsoft.Xna.Framework;
using ServerSideCharacter2.GUI.UI.Component;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ServerSideCharacter2.GUI.UI
{
	public class NormalShopBuyState : AdvWindowUIState
	{
		public static NormalShopBuyState Instance;


		private const float WINDOW_WIDTH = 380;
		private const float WINDOW_HEIGHT = 160;
		private readonly int TEXTBOX_HEIGHT = 35;
		private readonly int TEXTBOX_WIDTH = 200;
		private readonly int Y_OFFSET = 50;
		private readonly int X_OFFSET = -40;
		private UIAdvTextBox _donationValueText;
		private UICDButton _submitButton;
		private JsonData.SimplifiedMarketItem currentBuying;

		public NormalShopBuyState()
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

			_donationValueText = new UIAdvTextBox();
			_donationValueText.Top.Set(-TEXTBOX_HEIGHT + Y_OFFSET, 0.5f);
			_donationValueText.Left.Set(-TEXTBOX_WIDTH / 2 + X_OFFSET, 0.5f);
			_donationValueText.Width.Set(TEXTBOX_WIDTH, 0f);
			_donationValueText.Height.Set(TEXTBOX_HEIGHT, 0f);
			WindowPanel.Append(_donationValueText);

			var label = new UIText("输入需要购买的数量");
			label.Top.Set(60, 0f);
			label.Left.Set(-TEXTBOX_WIDTH / 2 + X_OFFSET, 0.5f);
			WindowPanel.Append(label);

			var submitButton = new UICDButton(null, true);
			submitButton.Top.Set(-TEXTBOX_HEIGHT + Y_OFFSET - 5, 0.5f);
			submitButton.Left.Set(TEXTBOX_WIDTH / 2 + X_OFFSET + 20, 0.5f);
			submitButton.Width.Set(70f, 0f);
			submitButton.Height.Set(38f, 0f);
			submitButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
			submitButton.ButtonDefaultColor = new Color(200, 200, 200);
			submitButton.ButtonChangeColor = Color.White;
			submitButton.CornerSize = new Vector2(12, 12);
			submitButton.ButtonText = "确认";
			submitButton.OnClick += SubmitButton_OnClick;
			WindowPanel.Append(submitButton);
		}

		public void SetItem(JsonData.SimplifiedMarketItem item)
		{
			currentBuying = item;
		}

		private void SubmitButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			int res;
			if (!int.TryParse(_donationValueText.Text, out res))
			{
				ServerSideCharacter2.Instance.ShowMessage("输入的数字不合法，应为整数", 120, Color.Red);
				return;
			}
			if(res <= 0)
			{
				ServerSideCharacter2.Instance.ShowMessage("购买的物品必须是正整数", 120, Color.Red);
				return;
			}
			Item item = new Item();
			item.netDefaults(currentBuying.ItemID);
			if (res > item.maxStack)
			{
				ServerSideCharacter2.Instance.ShowMessage($"此物品一次最多购买 {item.maxStack} 个", 120, Color.Red);
				return;
			}
			Main.PlaySound(7, -1, -1, 1, 1f, 0.0f);
			MessageSender.SendShopBuy(item.netID, res);
		}

		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.NormalShopBuyPage);
		}
	}
}
