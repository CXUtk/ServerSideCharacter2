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
using ServerSideCharacter2.GUI.UI;

namespace ServerSideCharacter2.GUI
{
	public class GUIManager
	{
		private ServerSideCharacter2 _mod;
		private UserInterface _userInterface;
		private UIToolButtonState _mainButtonState;
		private WindowUIState _mainWindow;

		public GUIManager(ServerSideCharacter2 mod)
		{
			_mod = mod;
			_mainButtonState = new UIToolButtonState();
			_userInterface = new UserInterface();
			_mainWindow = new WindowUIState();
			//_mainWindow.setAnimation(new Animate.MoveIn(60, new Microsoft.Xna.Framework.Vector2(500, 500)));
			_userInterface.SetState(_mainButtonState);
		}
		
		private void Update_Main()
		{
			_userInterface.CurrentState.Update(Main._drawInterfaceGameTime);
		}

		private void Draw_Main()
		{
			_userInterface.CurrentState.Draw(Main.spriteBatch);
		}

		public void RunUI()
		{
			try
			{
				Update_Main();
				Draw_Main();
			}
			catch(Exception ex)
			{
				Main.NewText(ex);
			}
		}

		internal void SwitchState()
		{
			_userInterface.SetState(_mainWindow);
		}
	}
}
