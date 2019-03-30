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
			nameLabel.TextColor = info.IsLogin ? Color.LimeGreen : new Color(180, 180, 180);
		}

		protected override void AddExtraButtons(List<UICDButton> buttons)
		{
			base.AddExtraButtons(buttons);
		}
		//public override void Click(UIMouseEvent evt)
		//{
		//	this.Width.Set(100, 0f);
		//	Recalculate();
		//	base.Click(evt);
		//}

		public override int CompareTo(object obj)
		{
			UIFriendBar other = obj as UIFriendBar;

			return string.Compare(this.playerInfo.Name, other.playerInfo.Name);
		}


	}
}
