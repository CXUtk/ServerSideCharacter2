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

namespace ServerSideCharacter2.GUI.UI.Component.Special
{
	public class UIUnionMemberBar : UINormalPlayerBar
	{
		private bool _isOwner;

		public UIUnionMemberBar(SimplifiedPlayerInfo info, bool owner) : base(info)
		{
			if (owner)
			{
				_defaultColor = Color.LimeGreen;
				this.Color = _defaultColor * 0.7f;
			}
			_isOwner = owner;

			var classText = new UIText(owner ? "会长" : "成员");
			classText.Top.Set(10, 0f);
			classText.Left.Set(165, 0);
			Append(classText);
		}

		protected override void AddExtraButtons(List<UICDButton> buttons)
		{
			AddFriendButton();
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


			if (Main.netMode == 0 || ServerSideCharacter2.ClientUnion.Owner == Main.LocalPlayer.name)
			{
				var kickButton = new UICDButton(null, true);
				kickButton.Width.Set(70f, 0f);
				kickButton.Height.Set(38f, 0f);
				kickButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack3"];
				kickButton.ButtonDefaultColor = new Color(200, 200, 200);
				kickButton.ButtonChangeColor = Color.White;
				kickButton.CornerSize = new Vector2(12, 12);
				kickButton.ButtonText = "踢出";
				kickButton.OnClick += KickButton_OnClick1;
				buttons.Add(kickButton);
			}

		}

		private void KickButton_OnClick1(UIMouseEvent evt, UIElement listeningElement)
		{
			MessageSender.SendUnionKick(playerInfo.Name);
		}

		public override int CompareTo(object obj)
		{
			var other = obj as UIUnionMemberBar;
			if (this._isOwner && !other._isOwner) return -1;
			else if (playerInfo.IsLogin && !other.playerInfo.IsLogin) return -1;
			else if (!playerInfo.IsLogin && other.playerInfo.IsLogin) return 1;
			else return 0;
		}


	}
}
