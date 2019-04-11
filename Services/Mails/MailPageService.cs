using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ServerSideCharacter2.GUI;
using ServerSideCharacter2.GUI.UI;
using Terraria;
using Terraria.UI;

namespace ServerSideCharacter2.Services.Mails
{
	public class MailPageService : ISSCToolBarService
	{
		public Texture2D Texture => ServerSideCharacter2.ModTexturesTable["Home"];

		public string Tooltip => "邮件";

		public string Name => "SSC: 邮件";

		public bool Enabled { get; set; }

		public MailPageService()
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
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.MailPage);
			if (ServerSideCharacter2.GuiManager.IsActive(SSCUIState.MailPage))
			{
				MailPageState.Instance.GetMailList();
			}
		}
	}
}
