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
		UnionPage2,
		UnionPage3,
		UnionCandidatePage,
		UnionDonationPage,
		ItemPage,
		ProfilePage,
		InventoryPage,
		InventoryPage2,
		NPCPage,
		CommunicationPage,
		GameCenterPage,
		MailPage,
		RankBoard,

	}
	public class GUIManager
	{
		private readonly ServerSideCharacter2 _mod;

		private LoginWindowState _loginWindowState;
		private PlayerOnlineWindow _playerOnlineWindow;
		private HomePageState _homePageState;
		private CommunicationState _communicationState;
		private GameCenterState _gameCenterState;
		private UnionPageState _unionPageState;
		private UnionPageState2 _unionPageState2;
		private UnionCreatePage _unionCreateState;
		private UnionCandidatePage _unionCandidateState;
		private MailPageState _mainPageState;
		private ItemUIState _getitemState;
		private PlayerProfileState _playerProfileState;
		private NPCUIState _getnpcState;
		private RankBoardState _rankBoardState;
		private PlayerInventoryState _playerInventoryState;
		private PlayerInventoryState2 _playerInventoryState2;

		private readonly UserInterface _toolBarInterface;
		private readonly CDInterfaceManager _cdInterface;
		private readonly MessageDisplayer _messageDisplayer;
		private readonly ToolBarState _toolBarState;

		private readonly Dictionary<SSCUIState, bool> _canShowUITable = new Dictionary<SSCUIState, bool>();
	

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
			var loginWindow = new ConditionalInterface(() => _canShowUITable[SSCUIState.LoginWindow]);
			loginWindow.SetState(_loginWindowState);
			_cdInterface.Add(loginWindow);

			_playerOnlineWindow = new PlayerOnlineWindow();
			var playerOnlineWindow = new ConditionalInterface(() => _canShowUITable[SSCUIState.PlayerOnlineWindow]);
			playerOnlineWindow.SetState(_playerOnlineWindow);
			_cdInterface.Add(playerOnlineWindow);

			_homePageState = new HomePageState();
			var hompage = new ConditionalInterface(() => _canShowUITable[SSCUIState.HomePage]);
			hompage.SetState(_homePageState);
			_cdInterface.Add(hompage);

			_communicationState = new CommunicationState();
			var commPage = new ConditionalInterface(() => _canShowUITable[SSCUIState.CommunicationPage]);
			commPage.SetState(_communicationState);
			_cdInterface.Add(commPage);


			_mainPageState = new MailPageState();
			var mailPage = new ConditionalInterface(() => _canShowUITable[SSCUIState.MailPage]);
			mailPage.SetState(_mainPageState);
			_cdInterface.Add(mailPage);

			_unionPageState = new UnionPageState();
			var unionpage = new ConditionalInterface(() => _canShowUITable[SSCUIState.UnionPage]);
			unionpage.SetState(_unionPageState);
			_cdInterface.Add(unionpage);

			_unionPageState2 = new UnionPageState2();
			var unionpage2 = new ConditionalInterface(() => _canShowUITable[SSCUIState.UnionPage2]);
			unionpage2.SetState(_unionPageState2);
			_cdInterface.Add(unionpage2);

			_unionCandidateState = new UnionCandidatePage();
			var cddpage = new ConditionalInterface(() => _canShowUITable[SSCUIState.UnionCandidatePage]);
			cddpage.SetState(_unionCandidateState);
			_cdInterface.Add(cddpage);

			_unionCreateState = new UnionCreatePage();
			var unioncreatepage = new ConditionalInterface(() => _canShowUITable[SSCUIState.UnionPage3]);
			unioncreatepage.SetState(_unionCreateState);
			_cdInterface.Add(unioncreatepage);


			_rankBoardState = new RankBoardState();
			var rankpage = new ConditionalInterface(() => _canShowUITable[SSCUIState.RankBoard]);
			rankpage.SetState(_rankBoardState);
			_cdInterface.Add(rankpage);

			_getitemState = new ItemUIState();
			var itempage = new ConditionalInterface(() => _canShowUITable[SSCUIState.ItemPage]);
			itempage.SetState(_getitemState);
			_cdInterface.Add(itempage);

			_getnpcState = new NPCUIState();
			var uinpcinterface = new ConditionalInterface(() => _canShowUITable[SSCUIState.NPCPage]);
			uinpcinterface.SetState(_getnpcState);
			_cdInterface.Add(uinpcinterface);


			_gameCenterState = new GameCenterState();
			var uiGameCenter = new ConditionalInterface(() => _canShowUITable[SSCUIState.GameCenterPage]);
			uiGameCenter.SetState(_gameCenterState);
			_cdInterface.Add(uiGameCenter);

			_playerInventoryState = new PlayerInventoryState();
			_playerInventoryState2 = new PlayerInventoryState2();
			AddState(_playerInventoryState, SSCUIState.InventoryPage);
			AddState(_playerInventoryState2, SSCUIState.InventoryPage2);


			var unionDonateState = new UnionDonationPage();
			var uniondonateInterface = new ConditionalInterface(() => _canShowUITable[SSCUIState.UnionDonationPage]);
			uniondonateInterface.SetState(unionDonateState);
			_cdInterface.Add(uniondonateInterface);

			// 置顶
			_playerProfileState = new PlayerProfileState();
			var profileinterface = new ConditionalInterface(() => _canShowUITable[SSCUIState.ProfilePage]);
			profileinterface.SetState(_playerProfileState);
			_cdInterface.Add(profileinterface);

		}


		private void AddState(UIState state, SSCUIState uistate)
		{
			var profileinterface = new ConditionalInterface(() => _canShowUITable[uistate]);
			profileinterface.SetState(state);
			_cdInterface.Add(profileinterface);
		}


		public void SetNPCDefaults()
		{
			_getnpcState.SetDefaults();
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

		internal void SetState(SSCUIState state, bool value)
		{
			if (!_canShowUITable.ContainsKey(state)) throw new ArgumentException("不存在此UI状态");
			_canShowUITable[state] = value;
		}

		public void ShowMessage(string msg, int time, Color color)
		{
			_messageDisplayer.ShowMessage(msg, time, color);
		}


		public void AppendFriends(JsonData.SimplifiedPlayerInfo info)
		{
			// _homePageState.AppendFriends(info);
		}

		public void RefreshFriends()
		{
			// _homePageState.RefreshFriends();
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
			if (Main.netMode == 0 || ServerSideCharacter2.MainPlayerGroup.HasPermission("see-inventory"))
			{
				OpenInventory(info);
			}
		}

		public void OpenInventory(JsonData.SimplifiedPlayerInfo info)
		{
			if (!_canShowUITable[SSCUIState.InventoryPage])
			{
				_canShowUITable[SSCUIState.InventoryPage] = true;
			}
			_playerInventoryState.GetInventory(info.PlayerID);
		}

		public void OpenInventory2()
		{
			if (!_canShowUITable[SSCUIState.InventoryPage2])
			{
				_canShowUITable[SSCUIState.InventoryPage2] = true;
			}
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
