using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Terraria.ID;
using Terraria;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Chat;
using ServerSideCharacter2.Utils;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using ServerSideCharacter2.Core;
using ServerSideCharacter2.Crypto;
using System.Windows.Forms;
using ServerSideCharacter2.Services;

namespace ServerSideCharacter2.Network
{
	public class SSCPacketHandler
	{
		private Dictionary<SSCMessageType, ISSCNetService> _packethandler;
		public SSCPacketHandler()
		{
			RegisterHandler();
		}

		public bool Handle(SSCMessageType msgType, BinaryReader reader, int playerNumber)
		{
			try
			{
				ISSCNetService method;
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
			_packethandler = new Dictionary<SSCMessageType, ISSCNetService>()
			{
				{SSCMessageType.LoginPassword,  new Services.Login.Authorization()},
				{SSCMessageType.RSAPublic,  new ReceiveRSA()},
				{SSCMessageType.SuccessLogin,  new Services.Login.LoginMessage(Color.Green)},
				{SSCMessageType.FailLogin,  new Services.Login.LoginMessage(Color.Red)},
				{SSCMessageType.WelcomeMessage,  new NormalMessage()},
			};
		}
	}
}
