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

namespace ServerSideCharacter2.GUI.UI.Component.Special
{
	public sealed class UIMatchGameBar : UIAdvPanel
	{
		private const float LABEL_MAX_WIDTH = 100;
		private const float GENDER_ICON_SIZE = 25;
		private const float EXTRA_BUTTON_MARGIN_LEFT = 5f;
		private const float EXTRA_BUTTON_MARGIN_RIGHT = 10f;

		internal static Color DefaultUIBlue = new Color(73, 94, 171);
		private readonly Texture2D dividerTexture;
		private readonly UIText matchTimeRem;

		private readonly List<UICDButton> extraButtons = new List<UICDButton>();

		private UIText nameLabel;
		private SimplifiedMatchInfo matchInfo;

		public UIMatchGameBar(SimplifiedMatchInfo info)
		{
			matchInfo = info;
			this.dividerTexture = TextureManager.Load("Images/UI/Divider");
			this.Width.Set(0, 1f);
			this.Height.Set(140f, 0f);
			this.CornerSize = new Vector2(8, 8);
			base.MainTexture = ServerSideCharacter2.ModTexturesTable["Box"];
			base.SetPadding(6f);
			this.OverflowHidden = true;


			nameLabel = new UIText(matchInfo.Name);
			nameLabel.Top.Set(10, 0f);
			nameLabel.Left.Set(5, 0);
			Append(nameLabel);

			var matchingStateText = new UIText("");
			matchingStateText.Top.Set(10f, 0f);
			matchingStateText.Left.Set(-100f, 1f);
			if (!info.IsMatching)
			{
				matchingStateText.SetText("匹配未开始");
				matchingStateText.TextColor = Color.Red;
			}
			else if(!info.IsGameStarted)
			{
				matchingStateText.SetText("匹配中……");
				matchingStateText.TextColor = Color.Lime;
			}
			else
			{
				matchingStateText.SetText("游戏中……");
				matchingStateText.TextColor = Color.Yellow;
			}
			Append(matchingStateText);

			//bool male = Main.player[playerInfo.PlayerID].Male;
			//UIImage _genderImage = new UIImage(ServerSideCharacter2.ModTexturesTable[male ? "Male" : "Female"]);
			//_genderImage.Top.Set(-GENDER_ICON_SIZE / 2, 0.5f);
			//_genderImage.Left.Set(LABEL_MAX_WIDTH + 10, 0);
			//_genderImage.Width.Set(GENDER_ICON_SIZE, 0);
			//_genderImage.Height.Set(GENDER_ICON_SIZE, 0);
			//_onlinePlayerPanel.Append(_genderImage);

			//if (!info.IsFriend)
			//{
			//	addFriendButton = new UICDButton(null, true);
			//	addFriendButton.Top.Set(0f, 0f);
			//	addFriendButton.Left.Set(-70f, 1f);
			//	addFriendButton.Width.Set(70f, 0f);
			//	addFriendButton.Height.Set(38f, 0f);
			//	addFriendButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack3"];
			//	addFriendButton.ButtonDefaultColor = new Color(200, 200, 200);
			//	addFriendButton.ButtonChangeColor = Color.White;
			//	addFriendButton.CornerSize = new Vector2(12, 12);
			//	addFriendButton.ButtonText = "+好友";
			//	addFriendButton.OnClick += AddFriendButton_OnClick;
			//	Append(addFriendButton);
			//}
			var matchedPlayerText = new UIText("");
			matchedPlayerText.Top.Set(50f, 0f);
			matchedPlayerText.Left.Set(5, 0f);
			if (!info.IsMatching) info.MatchedPlayers = 0;
			matchedPlayerText.SetText($"匹配人数：{info.MatchedPlayers} / {info.MaxPlayers}");
			Append(matchedPlayerText);

			matchTimeRem = new UIText("");
			matchTimeRem.Top.Set(50f, 0f);
			matchTimeRem.Left.Set(-150, 1f);
			matchTimeRem.SetText($"剩余时间：{info.TimeRem / 60}s");
			Append(matchTimeRem);

			AddExtraButtons(extraButtons);

			SetUpExtraButtons();

		}


		private void AddExtraButtons(List<UICDButton> buttons)
		{
			if (Main.netMode == 0 || ServerSideCharacter2.MainPlayerGroup.HasPermission("match-join"))
			{
				var matchJoinButton = new UICDButton(null, true);
				matchJoinButton.Width.Set(70f, 0f);
				matchJoinButton.Height.Set(38f, 0f);
				matchJoinButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
				matchJoinButton.ButtonDefaultColor = new Color(200, 200, 200);
				matchJoinButton.ButtonChangeColor = Color.White;
				matchJoinButton.CornerSize = new Vector2(12, 12);
				matchJoinButton.ButtonText = "加入";
				matchJoinButton.OnClick += MatchJoinButton_OnClick;
				buttons.Add(matchJoinButton);
			}
			if (Main.netMode == 0 || ServerSideCharacter2.MainPlayerGroup.HasPermission("match-new") && !matchInfo.IsMatching)
			{
				var matchnewButton = new UICDButton(null, true);
				matchnewButton.Width.Set(100f, 0f);
				matchnewButton.Height.Set(38f, 0f);
				matchnewButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
				matchnewButton.ButtonDefaultColor = new Color(200, 200, 200);
				matchnewButton.ButtonChangeColor = Color.White;
				matchnewButton.CornerSize = new Vector2(12, 12);
				matchnewButton.ButtonText = "开启匹配";
				matchnewButton.OnClick += MatchnewButton_OnClick; ;
				buttons.Add(matchnewButton);
			}
		}

		private void MatchnewButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			MessageSender.SendNewMatchCommand(matchInfo.Name);
		}

		private void MatchJoinButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			MessageSender.SendJoinMatchCommand(matchInfo.Name);
		}

		private void SetUpExtraButtons()
		{
			if (extraButtons.Count == 0) return;
			var currentLeft = EXTRA_BUTTON_MARGIN_LEFT;
			foreach (var but in extraButtons)
			{
				but.Top.Set(80, 0f);
				but.Left.Set(currentLeft, 0f);
				Append(but);
				currentLeft += but.Width.Pixels + EXTRA_BUTTON_MARGIN_RIGHT;
			}
		}

		public override int CompareTo(object obj)
		{
			var other = obj as UIMatchGameBar;
			return string.Compare(matchInfo.Name, other.matchInfo.Name, StringComparison.Ordinal);
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
		


		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (matchInfo.IsMatching && !matchInfo.IsGameStarted)
			{
				if (matchInfo.TimeRem > 0)
					matchInfo.TimeRem--;
				matchTimeRem.SetText($"剩余时间：{matchInfo.TimeRem / 60}s");
			}
		}

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
