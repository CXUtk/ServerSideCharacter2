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
	public class UIUnionCandidateBar : UINormalPlayerBar
	{
		public UIUnionCandidateBar(SimplifiedPlayerInfo info) : base(info)
		{
			collapsedHeight = expandedHeight = 50f;
			var acceptCandidateButton = new UICDButton(null, true);
			acceptCandidateButton.Top.Set(0f, 0f);
			acceptCandidateButton.Left.Set(-70f, 1f);
			acceptCandidateButton.Width.Set(70f, 0f);
			acceptCandidateButton.Height.Set(38f, 0f);
			acceptCandidateButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack3"];
			acceptCandidateButton.ButtonDefaultColor = new Color(200, 200, 200);
			acceptCandidateButton.ButtonChangeColor = Color.White;
			acceptCandidateButton.CornerSize = new Vector2(12, 12);
			acceptCandidateButton.ButtonText = "接受";
			acceptCandidateButton.OnClick += AcceptCandidateButton_OnClick;
			Append(acceptCandidateButton);

			var rejectButton = new UICDButton(null, true);
			rejectButton.Top.Set(0f, 0f);
			rejectButton.Left.Set(-155f, 1f);
			rejectButton.Width.Set(70f, 0f);
			rejectButton.Height.Set(38f, 0f);
			rejectButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBackRej"];
			rejectButton.ButtonDefaultColor = new Color(200, 200, 200);
			rejectButton.ButtonChangeColor = Color.White;
			rejectButton.CornerSize = new Vector2(12, 12);
			rejectButton.ButtonText = "拒绝";
			rejectButton.OnClick += RejectButton_OnClick;
			Append(rejectButton);
		}

		private void RejectButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			MessageSender.SendCandidateOperation(playerInfo.Name, false);
		}

		private void AcceptCandidateButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			MessageSender.SendCandidateOperation(playerInfo.Name, true);
		}

		protected override void AddExtraButtons(List<UICDButton> buttons)
		{
			// 删掉所有ExtraButtons
		}


	}
}
