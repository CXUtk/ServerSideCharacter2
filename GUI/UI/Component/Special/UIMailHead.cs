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
using ServerSideCharacter2.Mailing;

namespace ServerSideCharacter2.GUI.UI.Component.Special
{
	public class UIMailHead : UIAdvPanel
	{
		public UIMailHead(MailHead info)
		{
			this.Width.Set(0, 1f);
			this.Height.Set(50f, 0f);
			this.CornerSize = new Vector2(8, 8);
			base.MainTexture = ServerSideCharacter2.ModTexturesTable["Box"];
			base.SetPadding(6f);
			this.OverflowHidden = true;

			string origText = info.Title;
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < origText.Length; i++)
			{
				sb.Append(origText[i]);
				Vector2 size = Main.fontMouseText.MeasureString(sb.ToString());
				if (size.X > 200)
				{
					sb.Append("…");
					break;
				}
			}
			UIText titleText = new UIText(sb.ToString());
			titleText.VAlign = 0.5f;
			if (!info.IsRead)
			{
				titleText.TextColor = Color.Yellow;
			}
			this.Append(titleText);
		}

	}
}
