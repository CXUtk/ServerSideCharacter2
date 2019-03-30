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
		private UIBar rankBar;
		private UIImageResizable rankimage;
		private UIText rankLabel;
		private UIList infoList;

		private const float RANK_BAR_WIDTH = 192;
		private const float RANK_BAR_HEIGHT = 18;
		private const float RANK_LEFT_OFFSET = 60;
		private Vector2 center;
		private SimplifiedPlayerInfo _info;

		public UIPlayerProfileHead()
		{
			textName = new UIText("");
			textName.Top.Set(0, 0f);
			textName.Left.Set(RANK_LEFT_OFFSET + 32, 0f);
			Append(textName);

			rankBar = new UIBar();
			rankBar.BarFrameTex = ServerSideCharacter2.ModTexturesTable["BarFrameRank"];
			rankBar.BarFillTex = Main.magicPixel;
			rankBar.BarFrameTexCornerSize = new Vector2(6, 6);
			rankBar.FillerDrawOffset = new Vector2(6, 6);
			rankBar.FillerSize = new Vector2(RANK_BAR_WIDTH - 12, RANK_BAR_HEIGHT - 12);
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

			var infopanel = new UIPanel();
			infopanel.Top.Set(20 + RANK_BAR_HEIGHT + 30f, 0f);
			infopanel.Left.Set(0f, 0f);
			infopanel.Width.Set(260, 0f);
			infopanel.Height.Set(340, 0f);
			infopanel.SetPadding(10f);

			infoList = new UIList();
			infoList.ListPadding = 5f;
			infoList.Width.Set(0f, 1f);
			infoList.Height.Set(0f, 1f);
			infopanel.Append(infoList);
			Append(infopanel);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			Player player = null;
			if (_info.PlayerID >= 0 && _info.PlayerID < 255)
			{
				player = Main.player[_info.PlayerID];
				Item item = player.inventory[player.selectedItem];
				player.inventory[player.selectedItem] = new Item();
				Main.instance.DrawPlayer(player,
					GetDimensions().Position() + new Vector2(2, 2) + Main.screenPosition, 0f, Vector2.Zero, 0f);
				player.inventory[player.selectedItem] = item;
			}
			else
			{
				player = new Player();
				Main.instance.DrawPlayer(player,
					GetDimensions().Position() + new Vector2(2, 2) + Main.screenPosition, 0f, Vector2.Zero, 0f);
			}
		}

		public void SetPlayer(SimplifiedPlayerInfo info)
		{
			_info = info;
			infoList.Clear();
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

			if (Main.netMode == 0 || ServerSideCharacter2.MainPlayerGroup.IsSuperAdmin)
			{
				UIText guidText = new UIText($"GUID：{_info.GUID}");
				infoList.Add(guidText);
			}

			UIText killcountText = new UIText($"击杀数：{_info.KillCount}");
			infoList.Add(killcountText);

			UIText grouptext = new UIText($"权限组：{_info.ChatPrefix}");
			infoList.Add(grouptext);
		}
	}
}
