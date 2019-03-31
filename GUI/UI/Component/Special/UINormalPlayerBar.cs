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
	public class UINormalPlayerBar : UIAdvPanel
	{
		private bool _expanded = false;

		private const float LABEL_MAX_WIDTH = 100;
		private const float GENDER_ICON_SIZE = 25;
		private const float EXTRA_BUTTON_MARGIN_LEFT = 5f;
		private const float EXTRA_BUTTON_MARGIN_RIGHT = 10f;

		internal static Color DefaultUIBlue = new Color(73, 94, 171);
		private Texture2D dividerTexture;
		private UICDButton addFriendButton;

		private List<UICDButton> extraButtons = new List<UICDButton>();

		protected UIText nameLabel;
		protected SimplifiedPlayerInfo playerInfo;

		public UINormalPlayerBar(SimplifiedPlayerInfo info)
		{
			playerInfo = info;
			this.dividerTexture = TextureManager.Load("Images/UI/Divider");
			this.Width.Set(0, 1f);
			this.Height.Set(50f, 0f);
			this.CornerSize = new Vector2(8, 8);
			base.MainTexture = ServerSideCharacter2.ModTexturesTable["Box"];
			base.SetPadding(6f);
			this.OverflowHidden = true;


			nameLabel = new UIText(playerInfo.Name);
			nameLabel.Top.Set(10, 0f);
			nameLabel.Left.Set(5, 0);
			Append(nameLabel);

			//bool male = Main.player[playerInfo.PlayerID].Male;
			//UIImage _genderImage = new UIImage(ServerSideCharacter2.ModTexturesTable[male ? "Male" : "Female"]);
			//_genderImage.Top.Set(-GENDER_ICON_SIZE / 2, 0.5f);
			//_genderImage.Left.Set(LABEL_MAX_WIDTH + 10, 0);
			//_genderImage.Width.Set(GENDER_ICON_SIZE, 0);
			//_genderImage.Height.Set(GENDER_ICON_SIZE, 0);
			//_onlinePlayerPanel.Append(_genderImage);

			if (!info.IsFriend)
			{
				addFriendButton = new UICDButton(null, true);
				addFriendButton.Top.Set(0f, 0f);
				addFriendButton.Left.Set(-70f, 1f);
				addFriendButton.Width.Set(70f, 0f);
				addFriendButton.Height.Set(38f, 0f);
				addFriendButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack3"];
				addFriendButton.ButtonDefaultColor = new Color(200, 200, 200);
				addFriendButton.ButtonChangeColor = Color.White;
				addFriendButton.CornerSize = new Vector2(12, 12);
				addFriendButton.ButtonText = "+好友";
				addFriendButton.OnClick += AddFriendButton_OnClick;
				Append(addFriendButton);
			}

			AddExtraButtons(extraButtons);

			SetUpExtraButtons();

		}

		protected virtual void AddExtraButtons(List<UICDButton> buttons)
		{

			var profilebutton = new UICDButton(null, true);
			profilebutton.Width.Set(70f, 0f);
			profilebutton.Height.Set(38f, 0f);
			profilebutton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
			profilebutton.ButtonDefaultColor = new Color(200, 200, 200);
			profilebutton.ButtonChangeColor = Color.White;
			profilebutton.CornerSize = new Vector2(12, 12);
			profilebutton.ButtonText = "资料";
			profilebutton.OnClick += Profilebutton_OnClick;
			buttons.Add(profilebutton);



			if (Main.netMode == 0 || ServerSideCharacter2.MainPlayerGroup.HasPermission("tp"))
			{
				var tpbutton = new UICDButton(null, true);
				tpbutton.Width.Set(70f, 0f);
				tpbutton.Height.Set(38f, 0f);
				tpbutton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
				tpbutton.ButtonDefaultColor = new Color(200, 200, 200);
				tpbutton.ButtonChangeColor = Color.White;
				tpbutton.CornerSize = new Vector2(12, 12);
				tpbutton.ButtonText = "传送";
				tpbutton.OnClick += Tpbutton_OnClick;
				buttons.Add(tpbutton);
			}


			if (Main.netMode == 0 || ServerSideCharacter2.MainPlayerGroup.HasPermission("lock"))
			{
				var lockButton = new UICDButton(null, true);
				lockButton.Width.Set(70f, 0f);
				lockButton.Height.Set(38f, 0f);
				lockButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
				lockButton.ButtonDefaultColor = new Color(200, 200, 200);
				lockButton.ButtonChangeColor = Color.White;
				lockButton.CornerSize = new Vector2(12, 12);
				lockButton.ButtonText = "锁住";
				lockButton.OnClick += LockButton_OnClick;
				buttons.Add(lockButton);
			}

			if (Main.netMode == 0 || ServerSideCharacter2.MainPlayerGroup.HasPermission("kick"))
			{
				var kickButton = new UICDButton(null, true);
				kickButton.Width.Set(70f, 0f);
				kickButton.Height.Set(38f, 0f);
				kickButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
				kickButton.ButtonDefaultColor = new Color(200, 200, 200);
				kickButton.ButtonChangeColor = Color.White;
				kickButton.CornerSize = new Vector2(12, 12);
				kickButton.ButtonText = "踢掉";
				kickButton.OnClick += KickButton_OnClick;
				buttons.Add(kickButton);
			}
		}

		private void KickButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (Main.netMode == 0) return;
			if (Main.player[playerInfo.PlayerID].active)
			{
				MessageSender.SendKickCommand(playerInfo.PlayerID);
			}
		}

		private void LockButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (Main.netMode == 0) return;
			if (Main.player[playerInfo.PlayerID].active)
			{
				MessageSender.SendLockCommand(Main.myPlayer, playerInfo.PlayerID, 3600);
			}
		}

		private void Profilebutton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.GuiManager.OpenProfile(playerInfo);
		}

		private void SetUpExtraButtons()
		{
			if (extraButtons.Count == 0) return;
			float currentLeft = EXTRA_BUTTON_MARGIN_LEFT;
			for (int i = 0; i < extraButtons.Count; i++)
			{
				var but = extraButtons[i];
				but.Top.Set(50, 0f);
				but.Left.Set(currentLeft, 0f);
				Append(but);
				currentLeft += but.Width.Pixels + EXTRA_BUTTON_MARGIN_RIGHT;
			}
		}

		private void Tpbutton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (Main.netMode == 0) return;
			if (Main.player[playerInfo.PlayerID].active)
			{
				MessageSender.SendTeleportCommand(playerInfo.PlayerID);
			}
		}


		public override int CompareTo(object obj)
		{
			UINormalPlayerBar other = obj as UINormalPlayerBar;
			return this.playerInfo.Rank.CompareTo(other.playerInfo.Rank);
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
		

		public override void Click(UIMouseEvent evt)
		{
			_expanded ^= true;
			if (_expanded)
			{
				this.Height.Set(100f, 0f);
			}
			else
			{
				this.Height.Set(50f, 0f);
			}
			Recalculate();
			
			base.Click(evt);
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			// spriteBatch.Draw(Main.magicPixel, spriteBatch.GraphicsDevice.ScissorRectangle, Color.Blue * 0.4f);
			base.DrawSelf(spriteBatch);
			if (_expanded)
			{
				CalculatedStyle innerDimensions = base.GetInnerDimensions();
				Vector2 position = new Vector2(innerDimensions.X + 5f, innerDimensions.Y + 40);
				spriteBatch.Draw(this.dividerTexture, position, null, Color.White, 0f, Vector2.Zero,
					new Vector2((innerDimensions.Width - 10f) / 8f, 1f), SpriteEffects.None, 0f);
			}
			
		}


		//public override void Draw(SpriteBatch spriteBatch)
		//{
		//	// 傻逼原版程序员不好好写剪裁效果，连矩形相交都不判
		//	bool overflowHidden = this.OverflowHidden;
		//	bool useImmediateMode = this._useImmediateMode;
		//	RasterizerState rasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
		//	Rectangle scissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
		//	SamplerState anisotropicClamp = SamplerState.AnisotropicClamp;

		//	var mystate = new RasterizerState
		//	{
		//		CullMode = CullMode.None,
		//		ScissorTestEnable = true
		//	};
		//	if (useImmediateMode)
		//	{
		//		spriteBatch.End();
		//		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, mystate, null, Main.UIScaleMatrix);
		//		this.DrawSelf(spriteBatch);
		//		spriteBatch.End();
		//		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, mystate, null, Main.UIScaleMatrix);
		//	}
		//	else
		//	{
		//		this.DrawSelf(spriteBatch);
		//	}
		//	if (overflowHidden)
		//	{
		//		spriteBatch.End();
		//		Rectangle clippingRectangle = this.GetClippingRectangle(spriteBatch);
		//		spriteBatch.GraphicsDevice.ScissorRectangle = GetRectIntersections(scissorRectangle, clippingRectangle);
		//		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, mystate, null, Main.UIScaleMatrix);
		//	}
		//	this.DrawChildren(spriteBatch);
		//	if (overflowHidden)
		//	{
		//		spriteBatch.End();
		//		spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
		//		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, rasterizerState, null, Main.UIScaleMatrix);
		//	}
		//}



		private void AddFriendButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			//ServerSideCharacter2.Instance.ShowMessage("目前没有实现，等裙子有时间写", 120, Color.White);
			MessageSender.SendFriendRequest(this.playerInfo.Name);
			Main.NewText("Send");
		}
	}
}
