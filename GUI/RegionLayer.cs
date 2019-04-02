using Microsoft.Xna.Framework;
using ServerSideCharacter2.GUI.UI.Component;
using ServerSideCharacter2.Items;
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
	public class RegionLayer : GameInterfaceLayer
	{
		public RegionLayer(string name, InterfaceScaleType scaleType) : base(name, scaleType)
		{

		}

		protected override bool DrawSelf()
		{
			var spriteBatch = Main.spriteBatch;
			if (Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == ServerSideCharacter2.Instance.ItemType<RegionItem>())
			{
				spriteBatch.Draw(Main.magicPixel, ServerSideCharacter2.RegionUpperLeft.ToVector2() * 16 - Main.screenPosition, new Rectangle(0, 0, 8, 8), Color.Purple);
				spriteBatch.Draw(Main.magicPixel, ServerSideCharacter2.RegionLowerRight.ToVector2() * 16 - Main.screenPosition, new Rectangle(0, 0, 8, 8), Color.Red);
				foreach (var region in ServerSideCharacter2.ClientRegions)
				{
					var rect = region.Value.Area;
					Drawing.DrawAdvBox(spriteBatch, new Rectangle((int)(rect.X * 16 - Main.screenPosition.X),
						(int)(rect.Y * 16 - Main.screenPosition.Y), rect.Width * 16, rect.Height * 16), Color.Yellow * 0.5f, Drawing.Box1, new Vector2(8, 8));
				}
				if(ServerSideCharacter2.RegionLowerRight.X - ServerSideCharacter2.RegionUpperLeft.X <= 0 || ServerSideCharacter2.RegionLowerRight.Y - ServerSideCharacter2.RegionUpperLeft.Y <= 0)
				{
					return true;
				}
				Rectangle targetRect = new Rectangle((int)((int)ServerSideCharacter2.RegionUpperLeft.X * 16 - Main.screenPosition.X), (int)((int)ServerSideCharacter2.RegionUpperLeft.Y * 16 - Main.screenPosition.Y),
					(int)(ServerSideCharacter2.RegionLowerRight.X - ServerSideCharacter2.RegionUpperLeft.X) * 16, (int)(ServerSideCharacter2.RegionLowerRight.Y - ServerSideCharacter2.RegionUpperLeft.Y) * 16);
				Drawing.DrawAdvBox(spriteBatch, targetRect, Color.Green * 0.5f, Drawing.Box1, new Vector2(8, 8));
			}

			return base.DrawSelf();
		}
	}
}
