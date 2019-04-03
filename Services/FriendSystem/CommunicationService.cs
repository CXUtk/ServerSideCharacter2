using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ServerSideCharacter2.GUI;
using Terraria;
using Terraria.UI;

namespace ServerSideCharacter2.Services.FriendSystem
{
	public class CommunicationService : ISSCToolBarService
	{
		public Texture2D Texture => ServerSideCharacter2.ModTexturesTable["Friend"];

		public string Tooltip => "交流页面";

		public string Name => "SSC: 交流";

		public bool Enabled { get; set; }

		public CommunicationService()
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
			Main.NewText("这个功能还没完成，所以暂时不能使用", Color.Red);
			return;
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.CommunicationPage);
		}
	}
}
