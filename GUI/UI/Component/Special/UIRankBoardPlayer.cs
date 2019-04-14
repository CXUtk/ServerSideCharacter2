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
	public class UIRankBoardPlayerBar : UINormalPlayerBar
	{
		private readonly int rank;


		private Color GetColor(int rank)
		{
			if (rank == 1) return Color.Red;
			else if (rank <= 5) return Color.Orange;
			else if (rank <= 10) return Color.Yellow;
			return Color.White;
		}

		public UIRankBoardPlayerBar(SimplifiedPlayerInfo info, int rank) : base(info)
		{
			this.rank = rank;
			var orderText = new UIText(rank.ToString());
			orderText.Top.Set(10, 0f);
			orderText.Left.Set(5, 0);
			Append(orderText);
			nameLabel.Left.Set(30, 0f);
			nameLabel.TextColor = GetColor(rank);

			var rankText = new UIText("分数: " + info.Rank.ToString())
			{
				HAlign = 1f
			};
			rankText.Top.Set(10, 0f);
			rankText.PaddingRight = 5f;
			Append(rankText);
		}

		//protected override void AddExtraButtons(List<UICDButton> buttons)
		//{
		//	AddFriendButton();
		//	var profilebutton = new UICDButton(null, true);
		//	profilebutton.Width.Set(70f, 0f);
		//	profilebutton.Height.Set(38f, 0f);
		//	profilebutton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
		//	profilebutton.ButtonDefaultColor = new Color(200, 200, 200);
		//	profilebutton.ButtonChangeColor = Color.White;
		//	profilebutton.CornerSize = new Vector2(12, 12);
		//	profilebutton.ButtonText = "资料";
		//	profilebutton.OnClick += Profilebutton_OnClick;
		//	buttons.Add(profilebutton);

		//	if (Main.netMode == 0 || ServerSideCharacter2.MainPlayerGroup.HasPermission("tp"))
		//	{
		//		var tpbutton = new UICDButton(null, true);
		//		tpbutton.Width.Set(70f, 0f);
		//		tpbutton.Height.Set(38f, 0f);
		//		tpbutton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
		//		tpbutton.ButtonDefaultColor = new Color(200, 200, 200);
		//		tpbutton.ButtonChangeColor = Color.White;
		//		tpbutton.CornerSize = new Vector2(12, 12);
		//		tpbutton.ButtonText = "传送";
		//		tpbutton.OnClick += Tpbutton_OnClick;
		//		buttons.Add(tpbutton);
		//	}


		//	if (Main.netMode == 0 || ServerSideCharacter2.ClientUnion.Owner == Main.LocalPlayer.name)
		//	{
		//		var kickButton = new UICDButton(null, true);
		//		kickButton.Width.Set(70f, 0f);
		//		kickButton.Height.Set(38f, 0f);
		//		kickButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack3"];
		//		kickButton.ButtonDefaultColor = new Color(200, 200, 200);
		//		kickButton.ButtonChangeColor = Color.White;
		//		kickButton.CornerSize = new Vector2(12, 12);
		//		kickButton.ButtonText = "踢出";
		//		kickButton.OnClick += KickButton_OnClick1;
		//		buttons.Add(kickButton);
		//	}

		//}
	}
}
