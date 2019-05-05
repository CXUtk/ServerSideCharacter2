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
using System.Diagnostics;
using ServerSideCharacter2.Unions;

namespace ServerSideCharacter2.GUI.UI.Component.Special
{
	public class UIUnionMemberBar : UINormalPlayerBar
	{
		public UnionPosition _position;

		public UIUnionMemberBar(SimplifiedPlayerInfo info, UnionPosition position, long donation) : base(info)
		{
			_position = position;
			if (position == UnionPosition.会长)
			{
				_defaultColor = Color.LimeGreen;
				this.Color = _defaultColor * 0.7f;
			}
			expandedHeight = 135;

			var classText = new UIText(position.ToString());
			classText.Top.Set(10, 0f);
			classText.Left.Set(165, 0);
			Append(classText);

			var donationText = new UIText($"贡献：{donation}");
			donationText.Top.Set(50, 0f);
			donationText.Left.Set(5f, 0);
			Append(donationText);

			AddExtraButton();

			SetUpExtraButtons();
		}

		private void AddExtraButton()
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
			extraButtons.Add(profilebutton);

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
				extraButtons.Add(tpbutton);
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
				extraButtons.Add(kickButton);
			}

			if (_position != UnionPosition.会长 && (Main.netMode == 0 || ServerSideCharacter2.ClientUnion.Owner == Main.LocalPlayer.name))
			{
				var builderButton = new UICDButton(null, true);
				builderButton.Width.Set(125f, 0f);
				builderButton.Height.Set(38f, 0f);
				builderButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack3"];
				builderButton.ButtonDefaultColor = new Color(200, 200, 200);
				builderButton.ButtonChangeColor = Color.White;
				builderButton.CornerSize = new Vector2(12, 12);
				builderButton.ButtonText = (_position == UnionPosition.建筑师 ? "取消" : "任命") + "建筑师";
				builderButton.OnClick += BuilderButton_OnClick;
				extraButtons.Add(builderButton);
			}
			buttonTopOffset = 75f;
		}

		protected override void AddExtraButtons(List<UICDButton> buttons)
		{
			
		}

		private void BuilderButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			MessageSender.SendUnionToggleBuilder(playerInfo.Name);
		}

		private void KickButton_OnClick1(UIMouseEvent evt, UIElement listeningElement)
		{
			MessageSender.SendUnionKick(playerInfo.Name);
		}

		public override int CompareTo(object obj)
		{
			var other = obj as UIUnionMemberBar;
			Debug.Assert(other != null, nameof(other) + " != null");
			return _position.CompareTo(other._position);
		}


	}
}
