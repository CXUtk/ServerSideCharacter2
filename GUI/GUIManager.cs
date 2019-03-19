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
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.GUI
{
	public enum SSCUIState
	{
		LoginWindow,	
	}
	public class GUIManager
	{
		private ServerSideCharacter2 _mod;

		private UserInterface _mainInterface;
		private LoginWindowState _loginWindowState;

		private UserInterface _toolBarInterface;

		private CDInterfaceManager _cdInterface;

		private Dictionary<SSCUIState, bool> _canShowUITable = new Dictionary<SSCUIState, bool>();
	

		public GUIManager(ServerSideCharacter2 mod)
		{
			_mod = mod;
			_mainInterface = new UserInterface();
			_loginWindowState = new LoginWindowState();

			_toolBarInterface = new UserInterface();
			_toolBarInterface.SetState(new ToolBarState());


			_canShowUITable.Add(SSCUIState.LoginWindow, false);

			_cdInterface = new CDInterfaceManager();
			ConditionalInterface loginWindow = new ConditionalInterface(() => { return _canShowUITable[SSCUIState.LoginWindow]; });
			loginWindow.SetState(_loginWindowState);
			_cdInterface.Add(loginWindow);


		}
		
		public void RelaxGUI()
		{
			_loginWindowState.Relax();
		}

		public void UpdateUI(GameTime gameTime) 
		{
			try
			{
				_cdInterface.Update(gameTime);
				_toolBarInterface.Update(gameTime);
			}
			catch (Exception ex)
			{
				Main.NewText(ex);
			}
		}
		public void RunUI()
		{
			try
			{
				_cdInterface.Draw(Main.spriteBatch);
				_toolBarInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
			}
			catch(Exception ex)
			{
				Main.NewText(ex);
			}
		}

		internal void ToggleState(SSCUIState state)
		{
			if (!_canShowUITable.ContainsKey(state)) throw new ArgumentException("不存在此UI状态");
			_canShowUITable[state] ^= true;
		}

		internal void Reset()
		{

		}
	}
}
