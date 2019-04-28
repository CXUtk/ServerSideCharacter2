using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.Core;
using ServerSideCharacter2.Crypto;
using ServerSideCharacter2.Unions;
using ServerSideCharacter2.Utils;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ServerSideCharacter2
{
	public class MessageSender
	{


		public static void SendTeleport(int plr, Vector2 pos)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.TeleportPalyer);
			p.WriteVector2(pos);
			p.Send(plr);
		}

		public static void SendRequestSave(int plr)
		{
			var name = Main.player[plr].name;
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RequestSaveData);
			p.Write((byte)plr);
			p.Send();
		}

		public static void SendTimeSet(double time, bool day)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.SendTimeSet);
			p.Write(time);
			p.Write(day);
			p.Write(Main.sunModY);
			p.Write(Main.moonModY);
			p.Send();
		}

		public static void SendSetPassword(int plr, string password)
		{
			var name = Main.player[plr].name;
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RequestRegister);
			p.Write((byte)plr);
			p.Write(password);
			p.Send();
		}

		public static void SendLoginPassword(CryptedUserInfo info)
		{
			if (Main.netMode != 1) return;
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.LoginPassword);
			p.Write(info.GetEncryptedData());
			p.Send();
		}

		public static void SyncSingleEquip(int target, int index, Item item)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.SyncSingleEquip);
			p.Write((byte)target);
			p.Write((short)index);
			p.Write((short)item.netID);
			p.Write((byte)item.prefix);
			p.Write((short)item.stack);
			ItemIO.SendModData(item, p);
			p.Send();
		}

		public static void SyncSingleEquip2(string name, int index, Item item)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.SyncSingleEquip2);
			p.Write(name);
			p.Write((byte)index);
			p.Write((short)item.netID);
			p.Write((byte)item.prefix);
			p.Write((short)item.stack);
			ItemIO.SendModData(item, p);
			p.Send();
		}

		public static void SendOfflineInventory(int target)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.GetEquipsOffline);
			p.Write(target);
			p.Send();
		}

		public static void SendKickCommand(int target)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.KickCommand);
			p.Write((byte)target);
			p.Send();
		}

		public static void SendKillCommand(int target)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.KillCommand);
			p.Write((byte)target);
			p.Send();
		}


		public static void SendBanCommand(int target, string reason)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.BanCommand);
			p.Write(target);
			p.Write(reason);
			p.Send();
		}

		public static void SendTimeCommand(int plr, bool set, int time, bool day)
		{
			var name = Main.player[plr].name;
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.TimeCommand);
			p.Write((byte)plr);
			p.Write(set);
			p.Write(time);
			p.Write(day);
			p.Send();
		}

		public static void SendLockCommand(int plr, int target, int time)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.LockPlayer);
			p.Write((byte)plr);
			p.Write((byte)target);
			p.Write(time);
			p.Send();
		}

		public static void SendPigCommand(int target)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.PigPlayer);
			p.Write((byte)target);
			p.Send();
		}


		public static void SendItemCommand(int type)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RequestItem);
			p.Write(type);
			p.Send();
		}

		public static void SendBanItemCommand(int type)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.BanItemCommand);
			p.Write((byte)Main.myPlayer);
			p.Write(type);
			p.Send();
		}

		public static void SendTeleportCommand(int target)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.TPCommand);
			p.Write((byte)target);
			p.Send();
		}


		public static void SendTeleportHereCommand(int target)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.TPHereCommand);
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
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.HelpCommand);
			p.Write((byte)plr);
			p.Send();
		}

		public static void SendButcherCommand()
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ButcherCommand);
			p.Send();
		}

		public static void SendSafeTeleport(int plr, Vector2 pos)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.SafeTeleport);
			p.Write((byte)plr);
			p.WriteVector2(pos);
			p.Send();
		}

		public static void SendSummonCommand(int type, int number)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.SummonCommand);
			p.Write(type);
			p.Write(number);
			p.Send();
		}

		public static void SendClearCommand(int type)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ClearCommand);
			p.Write((byte)type);
			p.Send();
		}


		public static void SendSetGroup(string id, string group)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RequestSetGroup);
			p.Write(id);
			p.Write(group);
			p.Send();
		}

		public static void SendInvasion(int type)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.Invasion);
			p.Write((byte)type);
			p.Send();
		}

		public static void SendRegionCreate(string name)
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RegionCreateCommand);
			p.Write(name);
			p.Write(ServerSideCharacter2.RegionUpperLeft.X);
			p.Write(ServerSideCharacter2.RegionUpperLeft.Y);
			p.Write(ServerSideCharacter2.RegionLowerRight.X);
			p.Write(ServerSideCharacter2.RegionLowerRight.Y);
			p.Send();
		}

		public static void SendUnionCreate(string name)
		{
			if (Main.netMode != 1) return;
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.CreateUnion);
			p.Write(name);
			p.Send();
		}

		public static void SendGetUnionsData()
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.UnionsInfo);
			p.Send();
		}
		public static void GetComplexUnionData()
		{
			if (Main.netMode == 1)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.UnionInfoComplex);
				p.Send();
			}
		}

		public static void SendComplexUnionData(Union union, int to)
		{
			if (Main.netMode == 2)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.UnionInfoComplex);
				var tmp = JsonConvert.SerializeObject(union.GetVerbose(to), Formatting.None);
				p.Write(tmp);
				p.Send(to);
			}
		}

		public static void SendCandidateOperation(string name, bool accept)
		{
			if (Main.netMode == 1)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.UnionCandidateOp);
				p.Write(name);
				p.Write(accept);
				p.Send();
			}
		}

		public static void SendRequestJoinUnion(string name)
		{
			if (Main.netMode == 1)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.RequestJoinUnion);
				p.Write(name);
				p.Send();
			}
		}

		public static void NotifyClientUnion(int to)
		{
			if (Main.netMode == 2)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.NotifyClientUnion);
				var splayer = Main.player[to].GetServerPlayer();
				p.Write(splayer.Union == null ? "无": splayer.Union.Name);
				if (splayer.Union != null)
				{
					p.Write(splayer.Union.Owner);
				}
				p.Send(to);
			}
		}

		public static void SendUnionRemove(string name)
		{
			if (Main.netMode == 1)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.UnionRemoveCommand);
				p.Write(name);
				p.Send();
			}
		}

		public static void SendUnionKick(string name)
		{
			if (Main.netMode == 1)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.UnionKick);
				p.Write(name);
				p.Send();
			}
		}

		public static void SendDonateUnion(int amount)
		{
			if (Main.netMode == 1)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.UnionDonate);
				p.Write(RSACrypto.EncryptWithTag(amount.ToString(), "ddl"));
				p.Send();
			}
		}
		public static void SendGetMailsHead()
		{
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.MailGetHeads);
			p.Send();
		}

		public static void SendGetMail(ulong ID)
		{
			if (Main.netMode == 1)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.MailGetContent);
				p.Write(ID);
				p.Send();
			}
		}

		public static void SendPickMailItem(ulong ID, byte pos)
		{
			if (Main.netMode == 1)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.MailPickItem);
				p.Write(ID);
				p.Write(pos);
				p.Send();
			}
		}


		public static void SendRegionRemove(string name)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RegionRemoveCommand);
			p.Write(name);
			p.Send();
		}

		public static void SendRegionPVP(string name, int mode)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RegionPVPCommand);
			p.Write(name);
			p.Write((byte)mode);
			p.Send();
		}

		public static void SendRegionForbid(string name)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RegionForbidCommand);
			p.Write(name);
			p.Send();
		}

		public static void SendRegionOwner(string name, int GUID)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RegionOwnerCommand);
			p.Write(name);
			p.Write(GUID);
			p.Send();
		}

		public static void SendRegionUnion(string name, string union)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RegionUnionCommand);
			p.Write(name);
			p.Write(union);
			p.Send();
		}

		public static void SendRegionShare(int plr, string name, int target)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.RegionShareCommand);
			p.Write((byte)plr);
			p.Write((byte)target);
			p.Write(name);
			p.Send();
		}

		public static void SendLoginIn(int to)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.NotifyLogin);
			p.Send(to);
		}

		public static void SendToggleExpert()
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ToggleExpert);
			p.Send();
		}

		public static void SendToggleHardmode()
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ToggleHardMode);
			p.Send();
		}


		public static void SendToggleForcePVP(int mode)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ForcePVP);
			p.Write((byte)mode);
			p.Send();
		}

		public static void SendToggleXmas()
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ToggleHardMode);
			p.Write((byte)Main.myPlayer);
			p.Send();
		}

		public static void SendGeneration(GenerationType type)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.GenResources);
			p.Write((byte)Main.myPlayer);
			p.Write((byte)type);
			p.Send();
		}

		public static void SendTPProtect(int plr)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.TPProtect);
			p.Write((byte)Main.myPlayer);
			p.Send();
		}

		public static void SendRSAPublic()
		{
			if(Main.netMode == 2)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
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
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.SuccessLogin);
				p.Write(msg);
				p.Send(to);
			}
		}

		public static void SendLoginFailed(int to, string msg)
		{
			if (Main.netMode == 2)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.FailLogin);
				p.Write(msg);
				p.Send(to);
			}
		}

		public static void SendWelcomeMessage(int to, string msg)
		{
			if (Main.netMode == 2)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.WelcomeMessage);
				p.Write(msg);
				p.WriteRGB(Color.White);
				p.Write(240);
				p.Send(to);
			}
		}

		public static void SendBoxMessage(int to, string msg, int time, Color c)
		{
			if (Main.netMode == 2)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.WelcomeMessage);
				p.Write(msg);
				p.WriteRGB(c);
				p.Write(time);
				p.Send(to);
			}
		}


		public static void SendRequestRankBoard()
		{
			if (Main.netMode == 1)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.RequestRankBoard);
				p.Send();
			}
		}

		public static void SendRequestOnlinePlayer()
		{
			if (Main.netMode == 1)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.RequestOnlinePlayers);
				p.Send();
			}
		}

		public static void SendOnlineInformation(int to, string data)
		{
			if (Main.netMode == 2)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.OnlinePlayersData);
				p.Write(data);
				p.Send(to);
			}
		}

		public static void SendFriendRequest(string name)
		{
			if (Main.netMode == 1)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.FriendRequest);
				p.Write(name);
				p.Send();
			}
		}

		public static void SendGetFriends()
		{
			if (Main.netMode == 1)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.GetFriends);
				p.Send();
			}
		}

		public static void SendGetGames()
		{
			if (Main.netMode == 1)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.GetMatches);
				p.Send();
			}
		}

		public static void SendMatchesData(int plr)
		{
			var data = JsonConvert.SerializeObject(ServerSideCharacter2.MatchingSystem.GetMatchInfo(), Formatting.None);
			ModPacket p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.GetMatches);
			p.Write(data);
			p.Send(plr);
		}

		public static void SendFriendsData(int to, string data)
		{
			if (Main.netMode == 2)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.FriendsData);
				p.Write(data);
				p.Send(to);
			}
		}

		public static void SendErrorMessage(int to, string msg)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.ErrorMessage);
			p.Write(msg);
			p.Send(to);
		}

		public static void SendInfoMessage(int to, string msg, Color c)
		{
			try
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.InfoMessage);
				p.Write(msg);
				p.WriteRGB(c);
				p.Send(to);
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}

		public static void SyncModPlayerInfo(int to, int from, MPlayer mPlayer)
		{
			var pack = ServerSideCharacter2.Instance.GetPacket();
			pack.Write((int)SSCMessageType.ModPlayerInfo);
			pack.Write((byte)mPlayer.player.whoAmI);
			BitsByte bits = new BitsByte();
			bits[0] = mPlayer.GodMode;
			bits[1] = mPlayer.Piggify;
			bits[2] = mPlayer.ShowRank;
			bits[3] = mPlayer.ShowCrown;
			pack.Write(bits);
			pack.Write(mPlayer.Rank);
			pack.Send(to, from);
		}

		public static void SendChatMessageToClient(int plr, string playername, string msg, Groups.Group group)
		{
			var pack = ServerSideCharacter2.Instance.GetPacket();
			pack.Write((int)SSCMessageType.ChatText);
			pack.Write((byte)plr);
			pack.Write(playername);
			pack.Write(msg);
			pack.Write(group.ChatPrefix);
			pack.WriteRGB(group.ChatColor);
			pack.Send();
		}
        public static void SendChatMessageToClient(int plr, string playername, string msg, Groups.Group group, string CustomChatPrefix)
        {
            var pack = ServerSideCharacter2.Instance.GetPacket();
            pack.Write((int)SSCMessageType.ChatText);
            pack.Write((byte)plr);
            pack.Write(playername);
            pack.Write(msg);
            pack.Write(group.ChatPrefix + (CustomChatPrefix == "" ? "" : "·" + CustomChatPrefix));
            pack.WriteRGB(group.ChatColor);
            pack.Send();
        }

        public static void SendSpawnRate(int val)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.SpawnRate);
			p.Write(val);
			p.Send();
		}

		public static void SendMaxSpawnCount(int val)
		{
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.MaxSpawnCount);
			p.Write(val);
			p.Send();
		}


		public static void SyncRegionsToClient(int plr)
		{
			if (Main.netMode == 2)
			{
				var p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.SyncRegionsToClient);
				ServerSideCharacter2.RegionManager.WriteRegions(p);
				p.Send(plr);
			}
		}

		public static void SendNewMatchCommand(string name)
		{
			if (Main.netMode != 1) return;
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.NewMatchCommand);
			p.Write(name);
			p.Send();
		}

		public static void SendJoinMatchCommand(string name)
		{
			if (Main.netMode != 1) return;
			var p = ServerSideCharacter2.Instance.GetPacket();
			p.Write((int)SSCMessageType.JoinMatchCommand);
			p.Write(name);
			p.Send();
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
