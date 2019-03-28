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
using ServerSideCharacter2.RankingSystem;

namespace ServerSideCharacter2.GUI.UI.Component.Special
{
	public class UIPlayerProfileHead : UIElement
	{
		private UIText textName;
		private UISimpleBar rankBar;
		private UIImageResizable rankimage;
		private UIText rankLabel;

		private const float RANK_BAR_WIDTH = 192;
		private const float RANK_BAR_HEIGHT = 18;
		private const float RANK_LEFT_OFFSET = 60;
		private Vector2 center;

		public UIPlayerProfileHead()
		{
			textName = new UIText("");
			textName.Top.Set(0, 0f);
			textName.Left.Set(RANK_LEFT_OFFSET + 32, 0f);
			Append(textName);

			rankBar = new UISimpleBar();
			rankBar.Top.Set(20f, 0f);
			rankBar.Left.Set(RANK_LEFT_OFFSET, 0f);
			rankBar.Width.Set(RANK_BAR_WIDTH, 0f);
			rankBar.Height.Set(RANK_BAR_HEIGHT, 0f);
			rankBar.Value = 0.3f;
			Append(rankBar);


			rankimage = new UIImageResizable(ServerSideCharacter2.ModTexturesTable["Crown"]);
			rankimage.Top.Set(-10, 0f);
			rankimage.Left.Set(RANK_LEFT_OFFSET, 0f);
			rankimage.UsePosition = false;
			center = new Vector2(RANK_LEFT_OFFSET + 16f, 6);
			Append(rankimage);

			rankLabel = new UIText("300/1000");
			rankLabel.Top.Set(20 + RANK_BAR_HEIGHT + 2f, 0f);
			rankLabel.Left.Set(RANK_LEFT_OFFSET, 0f);
			Append(rankLabel);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			Main.instance.DrawPlayer(Main.LocalPlayer,
				GetDimensions().Position() + new Vector2(2, 2) + Main.screenPosition, 0f, Vector2.Zero, 0f);
		}

		public void SetPlayer(SimplifiedPlayerInfo info)
		{
			textName.SetText(info.Name);
			var type = Ranking.GetRankType(info.Rank);
			var range = Ranking.GetRankRange(type);
			rankLabel.SetText($"{info.Rank} / {range.Item2}");

			var percent = (info.Rank - range.Item1) / (float)(range.Item2 - range.Item1);
			rankBar.Value = percent;

			rankimage.SetImage(ServerSideCharacter2.ModTexturesTable[type.ToString()]);
			rankimage.Left.Set(center.X - rankimage.Width.Pixels / 2, 0);
			rankimage.Top.Set(center.Y - rankimage.Height.Pixels / 2, 0);
			rankimage.Tooltip = Ranking.GetName(type);
		}
	}
}
