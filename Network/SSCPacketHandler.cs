using Microsoft.Xna.Framework;
using ServerSideCharacter2.Services;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace ServerSideCharacter2.Network
{
	public class SSCPacketHandler
	{
		private Dictionary<SSCMessageType, ISSCNetHandler> _packethandler;
		public SSCPacketHandler()
		{
			RegisterHandler();
		}

		public bool Handle(SSCMessageType msgType, BinaryReader reader, int playerNumber)
		{
			try
			{
				ISSCNetHandler method;
				if (_packethandler.TryGetValue(msgType, out method))
				{
					return method.Handle(reader, playerNumber);
				}
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
			return false;
		}



		private void RegisterHandler()
		{
			_packethandler = new Dictionary<SSCMessageType, ISSCNetHandler>()
			{
				{SSCMessageType.LoginPassword,  new Services.Login.Authorization()},
				{SSCMessageType.RSAPublic,  new ReceiveRSA()},
				{SSCMessageType.NotifyLogin,  new Services.Login.LoginMessage(Color.Green)},
				{SSCMessageType.SuccessLogin,  new Services.Login.LoginMessage(Color.Green)},
				{SSCMessageType.FailLogin,  new Services.Login.LoginMessage(Color.Red)},
				{SSCMessageType.WelcomeMessage,  new NormalMessage(240)},
				{SSCMessageType.RequestOnlinePlayers, new Services.OnlinePlayer.RequestPlayersHandler() },
				{SSCMessageType.OnlinePlayersData, new Services.OnlinePlayer.OnlinePlayerHandler() },
				{SSCMessageType.FriendRequest, new Services.FriendSystem.FriendRequestHandler() },
				{SSCMessageType.FriendsData, new Services.FriendSystem.FriendsDataHandler() },
				{SSCMessageType.GetFriends, new Services.FriendSystem.GetFriendsHandler() },
				{SSCMessageType.SyncPlayerBank, new Services.OnlinePlayer.PlayerBankHandler() },
				{SSCMessageType.ErrorMessage, new ErrorMessage() },
				{SSCMessageType.InfoMessage, new InfoMessage() },
			};
		}
	}
}
