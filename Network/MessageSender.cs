﻿using Microsoft.Xna.Framework;
using ServerSideCharacter2.Core;
using ServerSideCharacter2.Crypto;
using ServerSideCharacter2.Utils;
using Terraria;
using Terraria.ModLoader;
namespace ServerSideCharacter2
{
	public class MessageSender
	{

		public static void SyncPlayerHealth(int plr, int to, int from)
		{
			string name = Main.player[plr].name;
			ServerPlayer player = ServerSideCharacter2.PlayerCollection.Get(name);
			Main.player[plr].statLife = player.StatLife;
			Main.player[plr].statLifeMax = player.LifeMax;
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.SyncPlayerHealth);
			p.Write((byte)plr);
			p.Write(player.StatLife);
			p.Write(player.LifeMax);
			p.Send(to, from);
		}
		public static void SyncPlayerMana(int plr, int to, int from)
		{
			string name = Main.player[plr].name;
			ServerPlayer player = ServerSideCharacter2.PlayerCollection.Get(name);
			Main.player[plr].statMana = player.StatMana;
			Main.player[plr].statManaMax = player.ManaMax;
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.SyncPlayerMana);
			p.Write((byte)plr);
			p.Write(player.StatMana);
			p.Write(player.ManaMax);
			p.Send(to, from);
		}
		public static void SyncPlayerBanks(int plr, int to, int from)
		{
			string name = Main.player[plr].name;
			ServerPlayer player = ServerSideCharacter2.PlayerCollection.Get(name);
			Main.player[plr].bank = (Chest)player.bank.Clone();
			//Main.player[plr].bank2 = (Chest)player.bank2.Clone();
			//Main.player[plr].bank3 = (Chest)player.bank3.Clone();
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.SyncPlayerBank);
			p.Write((byte)plr);
			foreach (Item item in player.bank.item)
			{
				p.Write(item.type);
				p.Write((short)item.prefix);
				p.Write((short)item.stack);
			}
			//foreach (Item item in player.bank2.item)
			//{
			//	p.Write(item.type);
			//	p.Write((short)item.prefix);
			//	p.Write((short)item.stack);
			//}
			//foreach (Item item in player.bank3.item)
			//{
			//	p.Write(item.type);
			//	p.Write((short)item.prefix);
			//	p.Write((short)item.stack);
			//}
			p.Send(to, from);
		}

		public static void SendTeleport(int plr, Vector2 pos)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.TeleportPalyer);
			p.WriteVector2(pos);
			p.Send(plr, -1);
		}

		public static void SendRequestSave(int plr)
		{
			string name = Main.player[plr].name;
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RequestSaveData);
			p.Write((byte)plr);
			p.Send();
		}

		public static void SendTimeSet(double time, bool day)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.SendTimeSet);
			p.Write(time);
			p.Write(day);
			p.Write(Main.sunModY);
			p.Write(Main.moonModY);
			p.Send();
		}

		public static void SendSetPassword(int plr, string password)
		{
			string name = Main.player[plr].name;
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RequestRegister);
			p.Write((byte)plr);
			p.Write(password);
			p.Send();
		}

		public static void SendLoginPassword(CryptedUserInfo info)
		{
			if (Main.netMode != 1) return;
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.LoginPassword);
			p.Write(info.GetEncryptedData());
			p.Send();
		}

		public static void SendKillCommand(int plr, int target)
		{
			string name = Main.player[plr].name;
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.KillCommand);
			p.Write((byte)plr);
			p.Write((byte)target);
			p.Send();
		}

		public static void SendTimeCommand(int plr, bool set, int time, bool day)
		{
			string name = Main.player[plr].name;
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.TimeCommand);
			p.Write((byte)plr);
			p.Write(set);
			p.Write(time);
			p.Write(day);
			p.Send();
		}

		public static void SendLockCommand(int plr, int target, int time)
		{
			string name = Main.player[plr].name;
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.LockPlayer);
			p.Write((byte)plr);
			p.Write((byte)target);
			p.Write(time);
			p.Send();
		}

		public static void SendItemCommand(int type)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RequestItem);
			p.Write((byte)Main.myPlayer);
			p.Write(type);
			p.Send();
		}

		public static void SendBanItemCommand(int type)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.BanItemCommand);
			p.Write((byte)Main.myPlayer);
			p.Write(type);
			p.Send();
		}

		public static void SendTeleportCommand(int plr, int target)
		{
			string name = Main.player[plr].name;
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.TPCommand);
			p.Write((byte)plr);
			p.Write((byte)target);
			p.Send();
		}

		//public static void SendListCommand(int plr, ListType type, bool all)
		//{
		//	string name = Main.player[plr].name;
		//	ModPacket p = ServerSideCharacter.Instance.GetPacket();
		//	p.Write((int)SSCMessageType.ListCommand);
		//	p.Write((byte)plr);
		//	p.Write((byte)type);
		//	p.Write(all);
		//	p.Send();
		//}

		public static void SendHelpCommand(int plr)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.HelpCommand);
			p.Write((byte)plr);
			p.Send();
		}

		public static void SendButcherCommand(int plr)
		{
			string name = Main.player[plr].name;
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ButcherCommand);
			p.Write((byte)plr);
			p.Send();
		}

		public static void SendAuthRequest(int plr, string code)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RequestAuth);
			p.Write((byte)plr);
			p.Write(code);
			p.Send();
		}

		public static void SendSummonCommand(int plr, int type, int number)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.SummonCommand);
			p.Write((byte)plr);
			p.Write(type);
			p.Write(number);
			p.Send();
		}


		public static void SendSetGroup(int plr, int uuid, string group)
		{
			string name = Main.player[plr].name;
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RequestSetGroup);
			p.Write((byte)plr);
			p.Write(uuid);
			p.Write(group);
			p.Send();
		}

		//public static void SendRegionCreate(int plr, string name)
		//{
		//	ModPacket p = ServerSideCharacter.Instance.GetPacket();
		//	p.Write((int)SSCMessageType.RegionCreateCommand);
		//	p.Write((byte)plr);
		//	p.Write(name);
		//	p.WriteVector2(ServerSideCharacter.TilePos1);
		//	p.WriteVector2(ServerSideCharacter.TilePos2);
		//	p.Send();
		//}

		public static void SendRegionRemove(int plr, string name)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RegionRemoveCommand);
			p.Write((byte)plr);
			p.Write(name);
			p.Send();
		}

		public static void SendRegionShare(int plr, string name, int target)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RegionShareCommand);
			p.Write((byte)plr);
			p.Write((byte)target);
			p.Write(name);
			p.Send();
		}

		public static void SendSSC()
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ServerSideCharacter);
			p.Send();
		}

		public static void SendToggleExpert()
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ToggleExpert);
			p.Write((byte)Main.myPlayer);
			p.Send();
		}

		public static void SendToggleHardmode()
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ToggleHardMode);
			p.Write((byte)Main.myPlayer);
			p.Send();
		}

		public static void SendToggleXmas()
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ToggleHardMode);
			p.Write((byte)Main.myPlayer);
			p.Send();
		}

		public static void SendGeneration(GenerationType type)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.GenResources);
			p.Write((byte)Main.myPlayer);
			p.Write((byte)type);
			p.Send();
		}

		public static void SendTPProtect(int plr)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.TPProtect);
			p.Write((byte)Main.myPlayer);
			p.Send();
		}

		public static void SendRSAPublic()
		{
			if(Main.netMode == 2)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				// CommandBoardcast.ConsoleMessage("发送RSA公钥");
				p.Write((int)SSCMessageType.RSAPublic);
				p.Write(RSACrypto.PublicKey);
				p.Send();
			}
		}

		public static void SendLoginSuccess(int to, string msg)
		{
			if (Main.netMode == 2)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.SuccessLogin);
				p.Write(msg);
				p.Send(to);
			}
		}

		public static void SendLoginFailed(int to, string msg)
		{
			if (Main.netMode == 2)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.FailLogin);
				p.Write(msg);
				p.Send(to);
			}
		}

		public static void SendWelcomeMessage(int to, string msg)
		{
			if (Main.netMode == 2)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.WelcomeMessage);
				p.Write(msg);
				p.Send(to);
			}
		}

		public static void SendRequestOnlinePlayer()
		{
			if (Main.netMode == 1)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.RequestOnlinePlayers);
				p.Send();
			}
		}

		public static void SendOnlineInformation(int to, string data)
		{
			if (Main.netMode == 2)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.OnlinePlayersData);
				p.Write(data);
				p.Send(to);
			}
		}

		public static void SendFriendRequest(string name)
		{
			if (Main.netMode == 1)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.FriendRequest);
				p.Write(name);
				p.Send();
			}
		}

		public static void SendGetFriends()
		{
			if (Main.netMode == 1)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.GetFriends);
				p.Send();
			}
		}

		public static void SendFriendsData(int to, string data)
		{
			if (Main.netMode == 2)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.FriendsData);
				p.Write(data);
				p.Send(to);
			}
		}

		public static void SendErrorMessage(int to, string msg)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ErrorMessage);
			p.Write(msg);
			p.Send(to);
		}

		public static void SendInfoMessage(int to, string msg)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.InfoMessage);
			p.Write(msg);
			p.Send(to);
		}
		//public static void SendChestCommand(ChestManager.Pending pending, int plr, string friendName = null)
		//{
		//	ModPacket pack = ServerSideCharacter.Instance.GetPacket();
		//	pack.Write((int)SSCMessageType.ChestCommand);
		//	pack.Write((byte)plr);
		//	pack.Write((int)pending);
		//	if (pending.HasFlag(ChestManager.Pending.AddFriend) || pending.HasFlag(ChestManager.Pending.RemoveFriend))
		//	{
		//		Player friend = Utils.TryGetPlayer(friendName);
		//		if (friend == null || !friend.active)
		//		{
		//			Main.NewText("Player not found", Color.Red);
		//			return;
		//		}
		//		if (friend.whoAmI == plr)
		//		{
		//			Main.NewText("You cannot add yourself as a friend", Color.Red);
		//			return;
		//		}
		//		pack.Write((byte)friend.whoAmI);

		//	}
		//	pack.Send();
		//}
	}
}
