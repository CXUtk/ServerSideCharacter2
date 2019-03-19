using Microsoft.Xna.Framework;
using ServerSideCharacter2.GUI.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Social;
using Terraria.UI;

namespace ServerSideCharacter2.GUI
{
	public class TooltipLayer : GameInterfaceLayer
	{
		public TooltipLayer(string name, InterfaceScaleType scaleType) : base(name, scaleType)
		{

		}

		protected override bool DrawSelf()
		{
			if (ServerSideCharacter2.ShowTooltip == null) return true;
			if (ServerSideCharacter2.ShowTooltip != "")
			{
				Vector2 size = Main.fontMouseText.MeasureString(ServerSideCharacter2.ShowTooltip);
				Vector2 drawPos = new Vector2(Main.mouseX, Main.mouseY) + new Vector2(25f, 25f);
				if (drawPos.Y > Main.screenHeight - 30f)
					drawPos.Y = Main.screenHeight - 30f;
				if (drawPos.X > Main.screenWidth - size.X)
					drawPos.X = Main.screenWidth - size.X - 30.0f;
				Drawing.DrawBox(Main.spriteBatch, new Rectangle((int)drawPos.X - 5, (int)drawPos.Y - 10, (int)size.X + 10, (int)size.Y + 10),
					Color.White * 0.75f, 0.8f, ServerSideCharacter2.ModTexturesTable["Box2"]);
				Drawing.DrawColorCodedString(Main.spriteBatch, Main.fontMouseText, ServerSideCharacter2.ShowTooltip, drawPos);
			}
			ServerSideCharacter2.ShowTooltip = "";

			return base.DrawSelf();
		}
	}
}
