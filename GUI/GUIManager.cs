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
		PlayerOnlineWindow,
		HomePage,

	}
	public class GUIManager
	{
		private ServerSideCharacter2 _mod;

		private LoginWindowState _loginWindowState;
		private PlayerOnlineWindow _playerOnlineWindow;
		private HomePageState _homePageState;

		private UserInterface _toolBarInterface;

		private CDInterfaceManager _cdInterface;

		private MessageDisplayer _messageDisplayer;

		private Dictionary<SSCUIState, bool> _canShowUITable = new Dictionary<SSCUIState, bool>();
	

		public GUIManager(ServerSideCharacter2 mod)
		{
			_mod = mod;

			_messageDisplayer = new MessageDisplayer();

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

			_homePageState = new HomePageState();
			ConditionalInterface hompage = new ConditionalInterface(() => { return _canShowUITable[SSCUIState.HomePage]; });
			hompage.SetState(_homePageState);
			_cdInterface.Add(hompage);
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
				_messageDisplayer.Update(gameTime);
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
				_messageDisplayer.Draw(Main.spriteBatch);
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

		public void ShowMessage(string msg, int time, Color color)
		{
			_messageDisplayer.ShowMessage(msg, time, color);
		}


		public void AppendFriends(JsonData.SimplifiedPlayerInfo info)
		{
			_homePageState.AppendFriends(info);
		}

		public void RefreshFriends()
		{
			_homePageState.RefreshFriends();
		}

		public void AppendOnlinePlayers(JsonData.SimplifiedPlayerInfo info)
		{
			_playerOnlineWindow.AppendPlayers(info);
		}


		public void RefreshOnlinePlayers()
		{
			_playerOnlineWindow.RefreshOnlinePlayer();
		}



		internal bool IsActive(SSCUIState state)
		{
			if (!_canShowUITable.ContainsKey(state)) throw new ArgumentException("不存在此UI状态");
			return _canShowUITable[state];
		}
	}
}
