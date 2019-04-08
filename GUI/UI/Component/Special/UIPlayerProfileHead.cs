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
		private UIAdvList infoList;

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

			rankBar = new UIBar
			{
				BarFrameTex = ServerSideCharacter2.ModTexturesTable["BarFrameRank"],
				BarFillTex = Main.magicPixel,
				BarFrameTexCornerSize = new Vector2(6, 6),
				FillerDrawOffset = new Vector2(6, 6),
				FillerSize = new Vector2(RANK_BAR_WIDTH - 12, RANK_BAR_HEIGHT - 12)
			};
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
			infopanel.Width.Set(300f, 0f);
			infopanel.Height.Set(500f, 0f);
			infopanel.SetPadding(10f);

			infoList = new UIAdvList {StartPadding = 5f, ListPadding = 10f};
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
				var item = player.inventory[player.selectedItem];
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

		private string addColor(string str, Color c)
		{
			return $"[c/{c.Hex3()}:{str}]";
		}

		public void SetPlayer(SimplifiedPlayerInfo info)
		{
			_info = info;
			infoList.Clear();
            textName.SetText((info.CustomChatPrefix == "" ? "" : ("【" + info.CustomChatPrefix + "】")) + info.Name);
            var type = Ranking.GetRankType(info.Rank);
			var range = Ranking.GetRankRange(type);
			rankLabel.SetText($"{info.Rank} / {range.Item2}");

			var percent = (info.Rank - range.Item1) / (float)(range.Item2 - range.Item1);
			rankBar.Value = percent;

			rankimage.SetImage(ServerSideCharacter2.ModTexturesTable[type.ToString()]);
			rankimage.Left.Set(center.X - rankimage.Width.Pixels / 2, 0);
			rankimage.Top.Set(center.Y - rankimage.Height.Pixels / 2, 0);
			rankimage.Tooltip = Ranking.GetName(type);

			var stateText = new UIText("");
			infoList.Add(stateText);
			if(_info.CurrentMatch == "")
			{
				stateText.SetText($"状态：{addColor("空闲", Color.Green)}");
			}
			else
			{
				stateText.SetText($"状态：{addColor(_info.CurrentMatch + " 游戏中", Color.Yellow)}");
			}

			if (Main.netMode == 0 || ServerSideCharacter2.MainPlayerGroup.IsSuperAdmin)
			{
				var guidText = new UIText($"GUID：{_info.GUID}");
				infoList.Add(guidText);
				var qqNumberText = new UIText($"QQ：{_info.QQNumber}");
				infoList.Add(qqNumberText);
			}
			var playerIDText = new UIText($"玩家ID：{_info.PlayerID}");
			infoList.Add(playerIDText);

			var killcountText = new UIText($"击杀数：{_info.KillCount}");
			infoList.Add(killcountText);

			var grouptext = new UIText($"权限组：[c/{_info.ChatColor.Hex3()}:{_info.ChatPrefix}]");
			infoList.Add(grouptext);

			var sexText = new UIText($"性别：{((Main.player[_info.PlayerID].Male) ? "男" : "女")}");
			infoList.Add(sexText);

			var regTimeText = new UIText($"注册时间：{_info.RegistedTime.ToString("g")}");
			infoList.Add(regTimeText);
        }
    }
}
