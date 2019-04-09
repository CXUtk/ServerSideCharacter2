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
using Terraria.Graphics;
using System.Collections.Generic;
using ServerSideCharacter2.Unions;

namespace ServerSideCharacter2.GUI.UI.Component.Special
{
	public class UIUnionBar : UIAdvPanel
	{
		private const float LABEL_MAX_WIDTH = 100;
		private const float GENDER_ICON_SIZE = 25;
		private const float EXTRA_BUTTON_MARGIN_LEFT = 5f;
		private const float EXTRA_BUTTON_MARGIN_RIGHT = 10f;

		internal static Color DefaultUIBlue = new Color(73, 94, 171);
		private Texture2D dividerTexture;
		private UIText levelText;
		private UIText matchTimeRem;

		private List<UICDButton> extraButtons = new List<UICDButton>();

		protected UIText nameLabel;
		protected SimplifiedUnionInfo unionInfo;

		public UIUnionBar(SimplifiedUnionInfo info)
		{
			unionInfo = info;
			this.dividerTexture = TextureManager.Load("Images/UI/Divider");
			this.Width.Set(0, 1f);
			this.Height.Set(100f, 0f);
			this.CornerSize = new Vector2(8, 8);
			base.MainTexture = ServerSideCharacter2.ModTexturesTable["Box"];
			base.SetPadding(6f);
			this.OverflowHidden = true;


			nameLabel = new UIText(unionInfo.Name);
			nameLabel.Top.Set(10, 0f);
			nameLabel.Left.Set(5, 0);
			Append(nameLabel);

			levelText = new UIText("");
			levelText.Top.Set(10f, 0f);
			levelText.Left.Set(-220f, 1f);
			levelText.SetText($"等级：{info.Level}");
			Append(levelText);

			var memberNumText = new UIText("");
			memberNumText.Top.Set(10f, 0f);
			memberNumText.Left.Set(-120f, 1f);
			memberNumText.SetText($"人数：{info.NumMember} / {Union.GetMaxMembers(info.Level)}");
			Append(memberNumText);


			var ownerText = new UIText("");
			ownerText.Top.Set(60f, 0f);
			ownerText.Left.Set(-220f, 1f);
			ownerText.SetText($"会长：{info.OwnerName}");
			Append(ownerText);

			AddExtraButtons(extraButtons);
			SetUpExtraButtons();

		}


		protected virtual void AddExtraButtons(List<UICDButton> buttons)
		{
			var unionjoinButton = new UICDButton(null, true);
			unionjoinButton.Width.Set(70f, 0f);
			unionjoinButton.Height.Set(38f, 0f);
			unionjoinButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
			unionjoinButton.ButtonDefaultColor = new Color(200, 200, 200);
			unionjoinButton.ButtonChangeColor = Color.White;
			unionjoinButton.CornerSize = new Vector2(12, 12);
			unionjoinButton.ButtonText = "申请";
			unionjoinButton.OnClick += UnionjoinButton_OnClick;
			buttons.Add(unionjoinButton);
		}

		private void UnionjoinButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			MessageSender.SendRequestJoinUnion(unionInfo.Name);
		}

		private void SetUpExtraButtons()
		{
			if (extraButtons.Count == 0) return;
			var currentLeft = EXTRA_BUTTON_MARGIN_LEFT;
			for (var i = 0; i < extraButtons.Count; i++)
			{
				var but = extraButtons[i];
				but.Top.Set(50, 0f);
				but.Left.Set(currentLeft, 0f);
				Append(but);
				currentLeft += but.Width.Pixels + EXTRA_BUTTON_MARGIN_RIGHT;
			}
		}

		public override int CompareTo(object obj)
		{
			var other = obj as UIUnionBar;
			return this.unionInfo.Name.CompareTo(other.unionInfo.Name);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			Main.PlaySound(12, -1, -1, 1, 1f, 0f);
			this.Color = DefaultUIBlue;
			base.MouseOver(evt);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			this.Color = DefaultUIBlue * 0.7f;
			base.MouseOut(evt);
		}
		

		//public override void Click(UIMouseEvent evt)
		//{
		//	_expanded ^= true;
		//	this.Height.Set(_expanded ? 100f : 50f, 0f);
		//	Recalculate();
			
		//	base.Click(evt);
		//}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			// spriteBatch.Draw(Main.magicPixel, spriteBatch.GraphicsDevice.ScissorRectangle, Color.Blue * 0.4f);
			base.DrawSelf(spriteBatch);
			var innerDimensions = base.GetInnerDimensions();
			var position = new Vector2(innerDimensions.X + 5f, innerDimensions.Y + 40);
			spriteBatch.Draw(this.dividerTexture, position, null, Color.White, 0f, Vector2.Zero,
				new Vector2((innerDimensions.Width - 10f) / 8f, 1f), SpriteEffects.None, 0f);

		}


	}
}
