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
		PlayerOnlineWindow
	}
	public class GUIManager
	{
		private ServerSideCharacter2 _mod;

		private LoginWindowState _loginWindowState;
		private PlayerOnlineWindow _playerOnlineWindow;

		private UserInterface _toolBarInterface;

		private CDInterfaceManager _cdInterface;

		private Dictionary<SSCUIState, bool> _canShowUITable = new Dictionary<SSCUIState, bool>();
	

		public GUIManager(ServerSideCharacter2 mod)
		{
			_mod = mod;

			

			_toolBarInterface = new UserInterface();
			_toolBarInterface.SetState(new ToolBarState());

			foreach (var type in typeof(SSCUIState).GetEnumValues())
			{
				_canShowUITable.Add((SSCUIState)type, false);
			}

			_cdInterface = new CDInterfaceManager();

			SetWindows();

		}

		private void SetWindows()
		{
			_loginWindowState = new LoginWindowState();
			ConditionalInterface loginWindow = new ConditionalInterface(() => { return _canShowUITable[SSCUIState.LoginWindow]; });
			loginWindow.SetState(_loginWindowState);
			_cdInterface.Add(loginWindow);

			_playerOnlineWindow = new PlayerOnlineWindow();
			ConditionalInterface playerOnlineWindow = new ConditionalInterface(() => { return _canShowUITable[SSCUIState.PlayerOnlineWindow]; });
			playerOnlineWindow.SetState(_playerOnlineWindow);
			_cdInterface.Add(playerOnlineWindow);
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
