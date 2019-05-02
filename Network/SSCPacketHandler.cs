using Microsoft.Xna.Framework;
using ServerSideCharacter2.Services;
using ServerSideCharacter2.Services.Misc;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;

namespace ServerSideCharacter2.Network
{
	public class SSCPacketHandler
	{
		private Dictionary<SSCMessageType, ISSCNetHandler> _packethandler;
		public SSCPacketHandler()
		{
			RegisterHandler();
		}

		public void Handle(SSCMessageType msgType, BinaryReader reader, int playerNumber)
		{
			try
			{
				ISSCNetHandler method;
				if (_packethandler.TryGetValue(msgType, out method))
				{
					method.Handle(reader, playerNumber);
				}
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}



		private void RegisterHandler()
		{
			_packethandler = new Dictionary<SSCMessageType, ISSCNetHandler>()
			{
				{SSCMessageType.LoginPassword,  new Services.Login.Authorization()},
				{SSCMessageType.RSAPublic,  new ReceiveRSA()},
				{SSCMessageType.LockPlayer,  new LockHandler()},
				{SSCMessageType.NotifyLogin,  new Services.Login.NotifyLoginClient()},
				{SSCMessageType.SuccessLogin,  new Services.Login.LoginMessage(Color.Green)},
				{SSCMessageType.FailLogin,  new Services.Login.LoginMessage(Color.Red)},
				{SSCMessageType.WelcomeMessage,  new NormalMessage()},
				{SSCMessageType.RequestRankBoard, new Services.Rank.GetRankBoardHandler() },
				{SSCMessageType.RequestOnlinePlayers, new Services.OnlinePlayer.RequestPlayersHandler() },
				{SSCMessageType.OnlinePlayersData, new Services.OnlinePlayer.OnlinePlayerHandler() },
				{SSCMessageType.FriendRequest, new Services.FriendSystem.FriendRequestHandler() },
				{SSCMessageType.FriendsData, new Services.FriendSystem.FriendsDataHandler() },
				{SSCMessageType.GetFriends, new Services.FriendSystem.GetFriendsHandler() },
				{SSCMessageType.ErrorMessage, new ErrorMessage() },
				{SSCMessageType.InfoMessage, new InfoMessage() },
				{SSCMessageType.ButcherCommand, new ButcherHandler() },
				{SSCMessageType.ToggleExpert, new ExpertModeHandler() },
				{SSCMessageType.ToggleHardMode, new HardmodeHandler() },
				{SSCMessageType.SummonCommand, new SummonHandler() },
				{SSCMessageType.ToggleGodMode, new GodModeHandler() },
				{SSCMessageType.ModPlayerInfo, new ModPlayerInfoHandler() },
				{SSCMessageType.TPCommand, new TPHandler() },
				{SSCMessageType.RequestItem, new ItemHandler() },
				{SSCMessageType.SyncGroupInfoToClient, new SyncGroupHandler() },
				{SSCMessageType.ForcePVP, new ForcePVPHandler() },
				{SSCMessageType.ChatText, new ChatTextHandler() },
				{SSCMessageType.SpawnRate, new SpawnControlHandler1() },
				{SSCMessageType.MaxSpawnCount, new SpawnControlHandler2() },
				{SSCMessageType.KickCommand, new KickHandler() },
				{SSCMessageType.TPHereCommand, new TPHereHandler() },
				{SSCMessageType.RegionCreateCommand, new Services.Regions.RegionCreateHandler() },
				{SSCMessageType.SyncRegionsToClient, new Services.Regions.RegionSyncHandler() },
				{SSCMessageType.RegionPVPCommand, new Services.Regions.RegionPVPHandler() },
				{SSCMessageType.RegionRemoveCommand, new Services.Regions.RegionRemoveHandler() },
				{SSCMessageType.ClearCommand, new ClearHandler() },
				{SSCMessageType.PigPlayer, new PigHandler() },
				{SSCMessageType.RegionOwnerCommand, new Services.Regions.RegionOwnerHandler() },
				{SSCMessageType.RegionUnionCommand, new Services.Regions.RegionUnionHandler() },
				{SSCMessageType.NewMatchCommand, new Services.Matches.NewMatchHandler() },
				{SSCMessageType.JoinMatchCommand, new Services.Matches.JoinMatchHandler() },
				{SSCMessageType.KillCommand, new Services.Misc.KillHandler() },
				{SSCMessageType.GetMatches, new Services.Matches.GetMatchesHandler() },
				{SSCMessageType.SafeTeleport, new SafeTeleportHandler() },
				{SSCMessageType.BanCommand, new BanHandler() },
				{SSCMessageType.CreateUnion, new Services.Union.UnionCreateHandler() },
				{SSCMessageType.UnionsInfo, new Services.Union.GetUnionsHandler() },
				{SSCMessageType.NotifyClientUnion, new Services.Union.ClientUnionHandler() },
				{SSCMessageType.UnionInfoComplex, new Services.Union.GetUnionComplexHandler() },
				{SSCMessageType.RequestJoinUnion, new Services.Union.UnionJoinHandler() },
				{SSCMessageType.UnionCandidateOp, new Services.Union.UnionCandidateHandler() },
				{SSCMessageType.UnionRemoveCommand, new Services.Union.UnionRemoveHandler() },
				{SSCMessageType.UnionDonate, new Services.Union.UnionDonateHandler() },
				{SSCMessageType.UnionKick, new Services.Union.UnionKickHandler() },
				{SSCMessageType.MailGetHeads, new Services.Mails.RequestMailListHandler() },
				{SSCMessageType.MailGetContent, new Services.Mails.RequestMailContentHandler() },
				{SSCMessageType.MailPickItem, new Services.Mails.PickMailSlotHandler() },
				{SSCMessageType.Invasion, new Services.Misc.InvasionHandler() },
				{SSCMessageType.SyncSingleEquip, new Services.Misc.SyncEquipHandler() },
				{SSCMessageType.SyncSingleEquip2, new Services.Misc.SyncEquipHandler2() },
				{SSCMessageType.GetEquipsOffline, new Services.Misc.PlayerInventoryHandler() }
			};
		}
	}
}
