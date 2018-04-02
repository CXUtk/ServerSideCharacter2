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
		private ToolButtonState _mainButtonState;
		private MainWindowState _mainWindow;

		public GUIManager(ServerSideCharacter2 mod)
		{
			_mod = mod;
			_mainButtonState = new ToolButtonState();
			_userInterface = new UserInterface();
			_mainWindow = new MainWindowState();
			//_mainWindow.setAnimation(new Animate.MoveIn(60, new Microsoft.Xna.Framework.Vector2(500, 0)));
			_userInterface.SetState(_mainButtonState);
		}
		
		private void Update_Main()
		{
			_userInterface.Update(Main._drawInterfaceGameTime);
		}

		private void Draw_Main()
		{
			_userInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
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
			if (_userInterface.CurrentState.Equals(_mainButtonState))
				_userInterface.SetState(_mainWindow);
			else
				_userInterface.SetState(_mainButtonState);
		}

		internal void Reset()
		{
			_userInterface.SetState(_mainButtonState);
		}
	}
}
