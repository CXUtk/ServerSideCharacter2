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

namespace ServerSideCharacter2.Network
{
	public class SSCPacketHandler
	{
		public delegate bool SSCPacketHandlerDelegate(BinaryReader reader, int playerNumber);
		private Dictionary<SSCMessageType, SSCPacketHandlerDelegate> _packethandler;
		public SSCPacketHandler()
		{
			RegisterHandler();
		}

		public bool Handle(SSCMessageType msgType, BinaryReader reader, int playerNumber)
		{
			try
			{
				SSCPacketHandlerDelegate method;
				if (_packethandler.TryGetValue(msgType, out method))
				{
					return method(reader, playerNumber);
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
			_packethandler = new Dictionary<SSCMessageType, SSCPacketHandlerDelegate>()
			{
				{SSCMessageType.LoginPassword,  LoginMsg},
				{SSCMessageType.RSAPublic,  ReceiveRSA},
				{SSCMessageType.SuccessLogin,  SuccessLogin},
				{SSCMessageType.FailLogin,  FailedLogin},
				{SSCMessageType.WelcomeMessage,  FailedLogin},
			};
		}

		private void successLogin(ServerPlayer player)
		{
			player.IsLogin = true;
			player.ClearAllBuffs();
			NetMessage.SendData(MessageID.PlayerBuffs, -1, -1, NetworkText.Empty, player.PrototypePlayer.whoAmI, 0f, 0f, 0f, 0, 0, 0);
		}

		private bool LoginMsg(BinaryReader reader, int playerNumber)
		{
			if(Main.netMode == 2)
			{
				string encrypted = reader.ReadString();
				var info = CryptedUserInfo.GetDecrypted(encrypted);
				var serverPlayer = Main.player[playerNumber].GetServerPlayer();
				if (serverPlayer.HasPassword)
				{
					if (serverPlayer.CheckPassword(info))
					{
						successLogin(serverPlayer);
						MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "认证成功");
						CommandBoardcast.ConsoleMessage("玩家 " + serverPlayer.Name + " 认证成功");
					}
					else
					{
						MessageSender.SendLoginFailed(playerNumber, "密码错误！");
					}
				}
				else
				{
					serverPlayer.SetPassword(info);
					successLogin(serverPlayer);
					MessageSender.SendLoginSuccess(serverPlayer.PrototypePlayer.whoAmI, "注册成功");
				}
			}
			return false;
		}

		private bool ReceiveRSA(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				string publicKey = reader.ReadString();
				RSACrypto.SetPublicKey(publicKey);
			}
			return false;
		}

		private bool SuccessLogin(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				string msg = reader.ReadString();
				ServerSideCharacter2.Instance.ShowMessage(msg, 120, Color.Green);
				ServerSideCharacter2.Instance.RelaxButton();
			}
			return false;
		}

		private bool FailedLogin(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				string msg = reader.ReadString();
				ServerSideCharacter2.Instance.ShowMessage(msg, 120, Color.Red);
				ServerSideCharacter2.Instance.RelaxButton();
			}
			return false;
		}

		private bool WelcomeMessage(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				string msg = reader.ReadString();
				ServerSideCharacter2.Instance.ShowMessage(msg, 120, Color.White);
			}
			return false;
		}
	}
}
