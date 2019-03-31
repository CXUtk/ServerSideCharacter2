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
		UnionPage,
		ItemPage,
		ProfilePage,
		NPCPage
	}
	public class GUIManager
	{
		private ServerSideCharacter2 _mod;

		private LoginWindowState _loginWindowState;
		private PlayerOnlineWindow _playerOnlineWindow;
		private HomePageState _homePageState;
		private UnionPageState _unionPageState;
		private ItemUIState _getitemState;
		private PlayerProfileState _playerProfileState;
		private NPCUIState _getnpcState;

		private UserInterface _toolBarInterface;
		private CDInterfaceManager _cdInterface;
		private MessageDisplayer _messageDisplayer;
		private ToolBarState _toolBarState;

		private Dictionary<SSCUIState, bool> _canShowUITable = new Dictionary<SSCUIState, bool>();
	

		public GUIManager(ServerSideCharacter2 mod)
		{
			_mod = mod;

			_messageDisplayer = new MessageDisplayer();

			_toolBarInterface = new UserInterface();
			_toolBarState = new ToolBarState();
			_toolBarInterface.SetState(_toolBarState);

			foreach (var type in typeof(SSCUIState).GetEnumValues())
			{
				_canShowUITable.Add((SSCUIState)type, false);
			}

			_cdInterface = new CDInterfaceManager();
			SetWindows();
		}

		internal void SetWindows()
		{
			_loginWindowState = new LoginWindowState();
			var loginWindow = new ConditionalInterface(() => { return _canShowUITable[SSCUIState.LoginWindow]; });
			loginWindow.SetState(_loginWindowState);
			_cdInterface.Add(loginWindow);

			_playerOnlineWindow = new PlayerOnlineWindow();
			var playerOnlineWindow = new ConditionalInterface(() => { return _canShowUITable[SSCUIState.PlayerOnlineWindow]; });
			playerOnlineWindow.SetState(_playerOnlineWindow);
			_cdInterface.Add(playerOnlineWindow);

			_homePageState = new HomePageState();
			var hompage = new ConditionalInterface(() => { return _canShowUITable[SSCUIState.HomePage]; });
			hompage.SetState(_homePageState);
			_cdInterface.Add(hompage);

			_unionPageState = new UnionPageState();
			var unionpage = new ConditionalInterface(() => { return _canShowUITable[SSCUIState.UnionPage]; });
			unionpage.SetState(_unionPageState);
			_cdInterface.Add(unionpage);

			_getitemState = new ItemUIState();
			var itempage = new ConditionalInterface(() => { return _canShowUITable[SSCUIState.ItemPage]; });
			itempage.SetState(_getitemState);
			_cdInterface.Add(itempage);

			_playerProfileState = new PlayerProfileState();
			var profileinterface = new ConditionalInterface(() => { return _canShowUITable[SSCUIState.ProfilePage]; });
			profileinterface.SetState(_playerProfileState);
			_cdInterface.Add(profileinterface);

			_getnpcState = new NPCUIState();
			var uinpcinterface = new ConditionalInterface(() => { return _canShowUITable[SSCUIState.NPCPage]; });
			uinpcinterface.SetState(_getnpcState);
			_cdInterface.Add(uinpcinterface);
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


		public void SetMyPlayerProfile(JsonData.SimplifiedPlayerInfo info)
		{
			_homePageState.SetProfile(info);
		}


		internal bool IsActive(SSCUIState state)
		{
			if (!_canShowUITable.ContainsKey(state)) throw new ArgumentException("不存在此UI状态");
			return _canShowUITable[state];
		}

		public void OpenProfile(JsonData.SimplifiedPlayerInfo info)
		{
			if (!_canShowUITable[SSCUIState.ProfilePage])
			{
				_canShowUITable[SSCUIState.ProfilePage] = true;
			}
			_playerProfileState.SetProfile(info);
		}



		internal void TurnOffAll()
		{
			foreach (var type in typeof(SSCUIState).GetEnumValues())
			{
				_canShowUITable[(SSCUIState)type] = false;
			}
		}

		internal void CheckGroup()
		{
			if (_canShowUITable[SSCUIState.PlayerOnlineWindow])
			{
				RefreshOnlinePlayers();
			}
			if (_canShowUITable[SSCUIState.HomePage])
			{
				RefreshFriends();
			}
			ServerSideCharacter2.ToolBarServiceManager.ResetDefault();
			_toolBarState.SetUpButtons();
			_toolBarState.ShowButtons();
		}
	}
}
