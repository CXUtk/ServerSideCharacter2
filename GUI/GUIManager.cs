using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ServerSideCharacter2.GUI
{
	public class GUIManager
	{
		private ServerSideCharacter2 _mod;
		private UserInterface _userInterface;

		public GUIManager(ServerSideCharacter2 mod)
		{
			_mod = mod;
			_userInterface = new UserInterface();
		}
		
		private void Update_Main()
		{

		}

		private void Draw_Main()
		{

		}

		public void RunUI()
		{
			Update_Main();
			Draw_Main();
		}
	}
}
