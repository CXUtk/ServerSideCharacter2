using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Login
{
	public class LoginMessage : ISSCNetHandler
	{
		private Color textColor; 
		public LoginMessage(Color color)
		{
			textColor = color;
		}
		public void Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				var msg = reader.ReadString();
				ServerSideCharacter2.Instance.ShowMessage(msg, 120, textColor);
				ServerSideCharacter2.Instance.RelaxButton();
                if (msg == "请先绑定QQ！")
                { System.Diagnostics.Process.Start("http://steamcity.terrariaserver.cn/SteamCityRegister.aspx"); }
            }
		}
	}
}
