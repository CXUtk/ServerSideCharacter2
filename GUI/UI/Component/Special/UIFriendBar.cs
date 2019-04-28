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
	public class UIFriendBar : UINormalPlayerBar
	{
		private const float LABEL_MAX_WIDTH = 100;
		private const float GENDER_ICON_SIZE = 25;

		public UIFriendBar(SimplifiedPlayerInfo info) : base(info)
		{
			// nameLabel.TextColor = info.IsLogin ? Color.LimeGreen : new Color(180, 180, 180);
		}

		protected override void AddExtraButtons(List<UICDButton> buttons)
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
		}
		//public override void Click(UIMouseEvent evt)
		//{
		//	this.Width.Set(100, 0f);
		//	Recalculate();
		//	base.Click(evt);
		//}

		public override int CompareTo(object obj)
		{
			var other = obj as UIFriendBar;

			return string.CompareOrdinal(this.playerInfo.Name, other.playerInfo.Name);
		}


	}
}
