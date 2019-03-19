﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ServerSideCharacter2.GUI;
using Terraria.UI;

namespace ServerSideCharacter2.Services.Login
{
	public class LoginService : ISSCToolBarService
	{
		public Texture2D Texture => ServerSideCharacter2.ModTexturesTable["Cog"];

		public string Tooltip => "登录窗口";

		public string Name => "登录";

		public bool Enabled { get; set; }

		public LoginService()
		{
			Enabled = true;
		}

		public void OnButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.LoginWindow);
		}
	}
}
