using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using ServerSideCharacter2.GUI;
using ServerSideCharacter2.GUI.UI;
using ServerSideCharacter2.GUI.UI.Component;
using Terraria;
using Terraria.UI;

namespace ServerSideCharacter2.Services.Union
{
	public class UnionPageService : ISSCToolBarService
	{
		public Texture2D Texture => ServerSideCharacter2.ModTexturesTable["Home"];

		public string Tooltip => "公会";

		public string Name => "SSC: 公会";

		public bool Enabled { get; set; }

		public UIDrawEventHandler DrawEvent
		{
			get
			{
				return new UIDrawEventHandler((element, sb) =>
				{
					if (UnionCandidatePage.Instance.UnreadCount > 0)
					{
						var tex = ServerSideCharacter2.ModTexturesTable["RedDot"];
						Vector2 drawPos = element.GetDimensions().Position();
						drawPos.X += element.GetDimensions().Width;
						sb.Draw(tex, drawPos, null, Color.Red, 0f, tex.Size() * 0.5f, 1.2f, SpriteEffects.None, 0f);
						string num = UnionCandidatePage.Instance.UnreadCount.ToString();
						if (UnionCandidatePage.Instance.UnreadCount == 1)
						{
							return;
						}
						if (UnionCandidatePage.Instance.UnreadCount > 99)
						{
							num = "99+";
						}
						Vector2 size = Main.fontMouseText.MeasureString(num);
						sb.DrawString(Main.fontMouseText, num, drawPos - new Vector2(size.X * 0.5f, 8f), Color.White);
					}
				});
			}
		}

		public UnionPageService()
		{
			Enabled = true;
		}

		public void OnButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			if (Main.netMode != 0 && !ServerSideCharacter2.Instance.IsLoginClientSide)
			{
				Main.NewText("您还没有登录，请先登录", Color.Red);
				return;
			}
			if (ServerSideCharacter2.ClientUnion != null)
			{
				ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionPage2);
				if (ServerSideCharacter2.GuiManager.IsActive(SSCUIState.UnionPage2))
				{
					UnionPageState2.Instance.RefreshUnion();
				}
			}
			else
			{
				ServerSideCharacter2.Instance.ChangeState(SSCUIState.UnionPage);
				if (ServerSideCharacter2.GuiManager.IsActive(SSCUIState.UnionPage))
				{
					UnionPageState.Instance.RefreshUnions();
				}
			}
		}
	}
}
