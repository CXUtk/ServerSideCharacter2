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
using Terraria.Graphics;

namespace ServerSideCharacter2.GUI.UI.Component.Special
{
	public class UIMailHead : UIAdvPanel
	{

		public ulong MailID
		{
			get
			{
				return head.MailID;
			}
		}
		private MailHead head;
		private int _innerCD = 0;
		private UIText titleText;
		private bool isExpanded = false;

		public UIMailHead(MailHead info)
		{
			head = info;
			this.Width.Set(0, 1f);
			this.Height.Set(60f, 0f);
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
			titleText = new UIText(sb.ToString())
			{
				VAlign = 0f
			};
			titleText.PaddingTop = 5f;

			if (!info.IsRead)
			{
				titleText.TextColor = Color.Yellow;
			}
			this.Append(titleText);

			var senddateText = new UIText($"发件人：{info.Sender}");
			senddateText.VAlign = 1f;
			this.Append(senddateText);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			Main.PlaySound(12, -1, -1, 1, 1f, 0f);
			this.Color = Drawing.DefaultBoxColor * 1.2f;
			base.MouseOver(evt);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			this.Color = Drawing.DefaultBoxColor * 0.7f;
			base.MouseOut(evt);
		}

		public override void Click(UIMouseEvent evt)
		{
			base.Click(evt);
			if (_innerCD > 0) return;
			MailPageState.Instance.ClearContent();
			MailPageState.Instance.SelectedMailItem = this;
			MailPageState.Instance.GetContent(head.Title);
			_innerCD = 60;
			if (!head.IsRead)
			{
				head.IsRead = true;
				titleText.TextColor = Color.White;
				ServerSideCharacter2.UnreadCount--;
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (_innerCD > 0)
				_innerCD--;
		}

	}
}
