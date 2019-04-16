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
using ServerSideCharacter2.Groups;
using Terraria.GameContent.NetModules;
using Terraria.Net;
using Terraria.GameContent.UI.Chat;

namespace ServerSideCharacter2.Network
{
	public delegate bool PacketHandlerDelegate(ref BinaryReader reader, int playerNumber);
	public class PacketHandler
	{
		private Dictionary<int, PacketHandlerDelegate> _packethandler;
		public PacketHandler()
		{
			RegisterHandler();
		}

		public bool Handle(int msgType, ref BinaryReader reader, int playerNumber)
		{
			try
			{
				PacketHandlerDelegate method;
				if (_packethandler.TryGetValue(msgType, out method))
				{
					return method(ref reader, playerNumber);
				}
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
			return false;
		}

		private bool HandleNetModules(ref BinaryReader reader, int playernumber)
		{
			var moduleId = reader.ReadUInt16();
			//LoadNetModule is now used for sending chat text.
			//Read the module ID to determine if this is in fact the text module
			if (Main.netMode == 2)
			{
				if (moduleId == Terraria.Net.NetManager.Instance.GetId<Terraria.GameContent.NetModules.NetTextModule>())
				{
					//Then deserialize the message from the reader
					var msg = ChatMessage.Deserialize(reader);
					var splayer = Main.player[playernumber].GetServerPlayer();

					if (msg.Text.StartsWith("/", StringComparison.Ordinal)) return true;
					if (!splayer.IsLogin)
					{
						MessageSender.SendErrorMessage(playernumber, "你还没有登录，不能说话哦");
						return true;
					}
					var text = msg.Text;/*$"<{splayer.Group.ChatPrefix}>{Main.player[(int)playernumber].name}: {msg.Text}";*/
                                        //NetPacket packet = NetTextModule.SerializeServerMessage(NetworkText.FromLiteral(text),
                                        //	splayer.Group.ChatColor, (byte)playernumber);
                                        //NetManager.Instance.Broadcast(packet, -1);
                                        //MessageSender.SendChatMessageToClient(playernumber, Main.player[(int)playernumber].name, msg.Text, splayer.Group);
                    MessageSender.SendChatMessageToClient(playernumber, Main.player[(int)playernumber].name, msg.Text, splayer.Group, splayer.qqAuth.CustomChatPrefix);
                    CommandBoardcast.ConsoleNormalText($"<{splayer.Name}> : {text}");
					return true;
				}
			}
			//else if(Main.netMode == 1)
			//{
			//	if (moduleId == Terraria.Net.NetManager.Instance.GetId<Terraria.GameContent.NetModules.NetTextModule>())
			//	{
			//		//Then deserialize the message from the reader
			//		byte id = reader.ReadByte();
			//		string text = NetworkText.Deserialize(reader).ToString();
			//		Color c = reader.ReadRGB();
			//		if (id < 255 && text.Contains(':'))
			//		{
			//			Main.player[id].chatOverhead.NewMessage(text.Substring(text.IndexOf(':') + 1), Main.chatLength / 2);
			//			// text = NameTagHandler.GenerateTag(Main.player[(int)b].name) + " " + text;
			//		}
			//		Main.NewTextMultiline(text, false, c, -1);
			//		return true;
			//	}
			//}
			return false;
		}

		//private bool RequestChestOpen(ref BinaryReader reader, int playerNumber)
		//{
		//	if (!ServerSideCharacter.Config.EnableChestProtection)
		//		return false;
		//	if (Main.netMode == 2)
		//	{
		//		int x = reader.ReadInt16();
		//		int y = reader.ReadInt16();
		//		int id = Chest.FindChest(x, y);
		//		Player player = Main.player[playerNumber];
		//		ServerPlayer sPlayer = player.GetServerPlayer();
		//		ChestManager.Pending pending = ServerSideCharacter.ChestManager.GetPendings(sPlayer);
		//		switch (pending)
		//		{
		//			case ChestManager.Pending.AddFriend:
		//				if (ServerSideCharacter.ChestManager.IsOwner(id, sPlayer))
		//				{
		//					ServerPlayer friend = ServerSideCharacter.ChestManager.GetFriendP(sPlayer);
		//					ServerSideCharacter.ChestManager.AddFriend(id, friend);
		//					sPlayer.SendSuccessInfo($"{friend.Name} can open this chest now");
		//				}
		//				else
		//					sPlayer.SendErrorInfo("You are not the owner of this chest");
		//				break;
		//			case ChestManager.Pending.RemoveFriend:
		//				if (ServerSideCharacter.ChestManager.IsOwner(id, sPlayer))
		//				{
		//					ServerPlayer friend = ServerSideCharacter.ChestManager.GetFriendP(sPlayer);
		//					ServerSideCharacter.ChestManager.RemoveFriend(id, friend);
		//					sPlayer.SendSuccessInfo($"{friend.Name} can't open this chest now");
		//				}
		//				else
		//					sPlayer.SendErrorInfo("You are not the owner of this chest");
		//				break;
		//			case ChestManager.Pending.Public:
		//				if (ServerSideCharacter.ChestManager.IsOwner(id, sPlayer))
		//				{
		//					if (!ServerSideCharacter.ChestManager.IsPublic(id))
		//					{
		//						ServerSideCharacter.ChestManager.SetOwner(id, sPlayer.UUID, true);
		//						sPlayer.SendSuccessInfo("This chest is now Public");
		//					}
		//					else
		//						sPlayer.SendErrorInfo("This chest is already public");

		//				}
		//				else
		//					sPlayer.SendErrorInfo("You are not the owner of this chest");
		//				break;
		//			case ChestManager.Pending.UnPublic:
		//				if (ServerSideCharacter.ChestManager.IsOwner(id, sPlayer))
		//				{
		//					if (ServerSideCharacter.ChestManager.IsPublic(id))
		//					{
		//						ServerSideCharacter.ChestManager.SetOwner(id, sPlayer.UUID, false);
		//						sPlayer.SendSuccessInfo("This chest is not Public anymore");
		//					}
		//					else
		//						sPlayer.SendErrorInfo("This chest is not public");
		//				}
		//				else
		//					sPlayer.SendErrorInfo("You are not the owner of this chest");
		//				break;
		//			case ChestManager.Pending.Protect:
		//				if (ServerSideCharacter.ChestManager.IsNull(id))
		//				{
		//					ServerSideCharacter.ChestManager.SetOwner(id, sPlayer.UUID, false);
		//					sPlayer.SendSuccessInfo("You now own this chest");
		//				}
		//				else if (ServerSideCharacter.ChestManager.IsOwner(id, sPlayer))
		//					sPlayer.SendErrorInfo("You already protected this chest");
		//				else
		//					sPlayer.SendErrorInfo("This chest as already been protected by other player");
		//				break;
		//			case ChestManager.Pending.DeProtect:
		//				if (ServerSideCharacter.ChestManager.IsOwner(id, sPlayer))
		//				{
		//					ServerSideCharacter.ChestManager.SetOwner(id, -1, false);
		//					sPlayer.SendSuccessInfo("This chest is no longer yours");
		//				}
		//				else if (ServerSideCharacter.ChestManager.IsNull(id))
		//					sPlayer.SendErrorInfo("This chest don't have a owner");
		//				else
		//					sPlayer.SendErrorInfo("You are not the owner of this chest");
		//				break;
		//			case ChestManager.Pending.Info:
		//				if (ServerSideCharacter.ChestManager.IsOwner(id, sPlayer))
		//				{
		//					ChestInfo chest = ServerSideCharacter.ChestManager.ChestInfo[id];
		//					StringBuilder info = new StringBuilder();
		//					if (sPlayer.PermissionGroup.HasPermission("chest"))
		//						info.AppendLine($"Owner: {ServerPlayer.FindPlayer(chest.OwnerID).Name}"); //For Admins
		//					info.AppendLine($"Public Chest: {chest.IsPublic.ToString().ToUpper()}");
		//					info.AppendLine($"Friends ({chest.Friends.Count.ToString()}): {string.Join(", ", chest.Friends.ToArray().Take(10).Select(uuid => ServerPlayer.FindPlayer(uuid).Name))}");
		//					sPlayer.SendInfo(info.ToString());
		//				}
		//				else if (ServerSideCharacter.ChestManager.IsNull(id))
		//					sPlayer.SendErrorInfo("This chest don't have a owner");
		//				else
		//					sPlayer.SendErrorInfo("You are not the owner of this chest");
		//				break;
		//			default:
		//				if (ServerSideCharacter.ChestManager.IsNull(id))
		//				{
		//					if (ServerSideCharacter.Config.AutoProtectChests)
		//					{
		//						ServerSideCharacter.ChestManager.SetOwner(id, sPlayer.UUID, false);
		//						sPlayer.SendSuccessInfo("You now own this chest");
		//					}
		//					else
		//						sPlayer.SendErrorInfo("Use '/chest protect' to become the owner of this chest");
		//					return false;
		//				}
		//				else if (ServerSideCharacter.ChestManager.CanOpen(id, sPlayer))
		//				{
		//					return false;
		//				}
		//				else
		//				{
		//					sPlayer.SendErrorInfo("You cannot open this chest");
		//				}
		//				break;
		//		}
		//		ServerSideCharacter.ChestManager.RemovePending(sPlayer, pending);

		//	}
		//	return true;
		//}

		private bool PlayerControls(ref BinaryReader reader, int playerNumber)
		{
			//if (Main.netMode == 2)
			//{
			//	if (Main.player[playerNumber].GetServerPlayer().posLock.LockedPos())
			//	{
			//		return true;
			//	}
			//}
			return false;
		}

		//private bool TileChange(ref BinaryReader reader, int playerNumber)
		//{
		//	if (Main.netMode == 2)
		//	{
		//		try
		//		{
		//			Player p = Main.player[playerNumber];
		//			ServerPlayer player = p.GetServerPlayer();
		//			int action = reader.ReadByte();
		//			short X = reader.ReadInt16();
		//			short Y = reader.ReadInt16();
		//			short type = reader.ReadInt16();
		//			int style = reader.ReadByte();
		//			if (ServerSideCharacter.CheckSpawn(X, Y) && player.PermissionGroup.Name != "spadmin")
		//			{
		//				player.SendErrorInfo("Warning: Spawn is protected from change");
		//				NetMessage.SendTileSquare(-1, X, Y, 4);
		//				return true;
		//			}
		//			else if (ServerSideCharacter.RegionManager.CheckRegion(X, Y, player))
		//			{
		//				player.SendErrorInfo("Warning: You don't have permission to change this tile");
		//				NetMessage.SendTileSquare(-1, X, Y, 4);
		//				return true;
		//			}
		//			else if (player.PermissionGroup.Name == "criminal")
		//			{
		//				player.SendErrorInfo("Warning: Criminals cannot change tiles");
		//				NetMessage.SendTileSquare(-1, X, Y, 4);
		//				return true;
		//			}
		//		}
		//		catch (Exception ex)
		//		{
		//			CommandBoardcast.ConsoleError(ex);
		//		}
		//	}
		//	return false;
		//}

		//private bool ChatText(ref BinaryReader reader, int playerNumber)
		//{
		//	int playerID = reader.ReadByte();
		//	if (Main.netMode == 2)
		//	{
		//		playerID = playerNumber;
		//	}
		//	Color c = reader.ReadRGB();
		//	if (Main.netMode == 2)
		//	{
		//		c = new Color(255, 255, 255);
		//	}
		//	string text = reader.ReadString();
		//	if (Main.netMode == 1)
		//	{
		//		string text2 = text.Substring(text.IndexOf('>') + 1);
		//		if (playerID < 255)
		//		{
		//			Main.player[playerID].chatOverhead.NewMessage(text2, Main.chatLength / 2);
		//		}
		//		Main.NewTextMultiline(text, false, c, -1);
		//	}
		//	else
		//	{
		//		Player p = Main.player[playerID];
		//		ServerPlayer player = p.GetServerPlayer();
		//		Group group = player.PermissionGroup;
		//		string prefix = "[" + group.ChatPrefix + "] ";
		//		c = group.ChatColor;
		//		NetMessage.SendData(25, -1, -1, NetworkText.FromLiteral(prefix + "<" + p.name + "> " + text), playerID, (float)c.R, (float)c.G, (float)c.B, 0, 0, 0);
		//		if (Main.dedServ)
		//		{
		//			Console.WriteLine("{0}<" + Main.player[playerID].name + "> " + text, prefix);
		//		}
		//	}
		//	return true;
		//}
		bool newflag = false;
		private bool PlayerSpawn(ref BinaryReader reader, int playerNumber)
		{
			int id = reader.ReadByte();
			if (Main.netMode == 2)
			{
				id = playerNumber;
			}
			var player = Main.player[id];
			player.SpawnX = reader.ReadInt16();
			player.SpawnY = reader.ReadInt16();
			player.Spawn();
			if (id == Main.myPlayer && Main.netMode != 2)
			{
				Main.ActivePlayerFileData.StartPlayTimer();
				Player.Hooks.EnterWorld(Main.myPlayer);
			}
			if (Main.netMode != 2 || Netplay.Clients[playerNumber].State < 3)
			{
				return true;
			}
			if(ServerSideCharacter2.Config.MediumcoreOnly && player.difficulty < 1)
			{
				NetMessage.SendData(2, id, -1,
					NetworkText.FromLiteral("你必须使用中核难度的人物"), 0, 0f, 0f, 0f, 0, 0, 0);
				return true;
			}
			else if(!ServerSideCharacter2.Config.MediumcoreOnly && player.difficulty >= 1)
			{
				NetMessage.SendData(2, id, -1,
					NetworkText.FromLiteral("请不要使用中核难度的人物"), 0, 0f, 0f, 0f, 0, 0, 0);
				return true;
			}
			//如果数据中没有玩家的信息
			if (!ServerSideCharacter2.PlayerCollection.ContainsKey(Main.player[playerNumber].name))
			{
				try
				{
					newflag = true;
					CommandBoardcast.ConsoleMessage(string.Format(GameLanguage.GetText("newPlayer"), Main.player[playerNumber].name));
					ServerSideCharacter2.PlayerCollection.AddNewPlayer(Main.player[playerNumber]);
				}
				catch (Exception ex)
				{
					CommandBoardcast.ConsoleError(ex);
				}
			}
			else
			{
				CommandBoardcast.ConsoleMessage(string.Format(GameLanguage.GetText("recognizedPlayer"), Main.player[playerNumber].name));
			}
			

			if (Netplay.Clients[playerNumber].State == 3)
			{
				Netplay.Clients[playerNumber].State = 10;
				NetMessage.greetPlayer(playerNumber);
				NetMessage.buffer[playerNumber].broadcast = true;
				MessageSender.SendRSAPublic();
				SyncConnectedPlayer(playerNumber);

				NetMessage.SendData(MessageID.SpawnPlayer, -1, playerNumber, NetworkText.Empty, playerNumber, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(MessageID.AnglerQuest, playerNumber, -1, NetworkText.FromLiteral(Main.player[playerNumber].name), Main.anglerQuest, 0f, 0f, 0f, 0, 0, 0);
				return true;
			}
			NetMessage.SendData(MessageID.SpawnPlayer, -1, playerNumber, NetworkText.Empty, playerNumber, 0f, 0f, 0f, 0, 0, 0);
			return true;
		}

		public static void SyncConnectedPlayer(int plr)
		{
			SyncOnePlayer(plr, -1, -1);
			for (var i = 0; i < 255; i++)
			{
				if (plr != i && Main.player[i].active)
				{
					SyncOnePlayer(i, plr, -1);
				}
			}
			SendNPCHousesAndTravelShop(plr);
			SendAnglerQuest(plr);
			EnsureLocalPlayerIsPresent();
		}

		private static void SyncOnePlayer(int plr, int toWho, int fromWho)
		{
			var active = 0;
			if (Main.player[plr].active)
			{
				active = 1;
			}
			if (Netplay.Clients[plr].State == 10)
			{
				var name = Main.player[plr].name;
				var player = ServerSideCharacter2.PlayerCollection.Get(name);
				player.SetID(plr);
				player.ApplyToPlayer();
				Main.player[plr].trashItem = new Item();

				NetMessage.SendData(MessageID.PlayerActive, toWho, fromWho, null, plr, active, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(MessageID.SyncPlayer, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(MessageID.PlayerControls, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(MessageID.PlayerHealth, toWho, fromWho, null, plr);
				NetMessage.SendData(MessageID.PlayerPvP, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(MessageID.PlayerTeam, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(MessageID.PlayerMana, toWho, fromWho, null, plr);
				NetMessage.SendData(MessageID.PlayerBuffs, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				if (toWho == -1)
				{
					player.IsLogin = false;
					player.ClearAllBuffs();
					player.Lock();
					player.SyncUnionInfo();
					player.SendMailList();

					MessageSender.SendWelcomeMessage(plr,
						player.HasPassword ? GameLanguage.GetText("welcomeold") : GameLanguage.GetText("welcomenew"));
					MessageSender.SyncRegionsToClient(plr);
					if (!player.InRegion || player.CurrentRegion.PVP == JsonData.PVPMode.Normal)
					{
						if (ServerSideCharacter2.Config.PvpMode == JsonData.PVPMode.Always)
						{
							player.PrototypePlayer.hostile = true;
							NetMessage.SendData(MessageID.PlayerPvP, -1, -1, NetworkText.FromLiteral(""), player.PrototypePlayer.whoAmI);
						}
					}
				}


				for (var i = 0; i < 59; i++)
				{
					NetMessage.SendData(MessageID.SyncEquipment, toWho, fromWho, NetworkText.FromLiteral(Main.player[plr].inventory[i].Name), plr, i, Main.player[plr].inventory[i].prefix, 0f, 0, 0, 0);
				}
				for (var j = 0; j < Main.player[plr].armor.Length; j++)
				{
					NetMessage.SendData(MessageID.SyncEquipment, toWho, fromWho, NetworkText.FromLiteral(Main.player[plr].armor[j].Name), plr, (59 + j), Main.player[plr].armor[j].prefix, 0f, 0, 0, 0);
				}
				for (var k = 0; k < Main.player[plr].dye.Length; k++)
				{
					NetMessage.SendData(MessageID.SyncEquipment, toWho, fromWho, NetworkText.FromLiteral(Main.player[plr].dye[k].Name), plr, (58 + Main.player[plr].armor.Length + 1 + k), Main.player[plr].dye[k].prefix, 0f, 0, 0, 0);
				}
				for (var l = 0; l < Main.player[plr].miscEquips.Length; l++)
				{
					NetMessage.SendData(MessageID.SyncEquipment, toWho, fromWho, NetworkText.Empty, plr, 58 + Main.player[plr].armor.Length + Main.player[plr].dye.Length + 1 + l, Main.player[plr].miscEquips[l].prefix, 0f, 0, 0, 0);
				}
				for (var m = 0; m < Main.player[plr].miscDyes.Length; m++)
				{
					NetMessage.SendData(MessageID.SyncEquipment, toWho, fromWho, NetworkText.Empty, plr, 58 + Main.player[plr].armor.Length + Main.player[plr].dye.Length + Main.player[plr].miscEquips.Length + 1 + m, Main.player[plr].miscDyes[m].prefix, 0f, 0, 0, 0);
				}
				for (var i = 0; i < Main.player[plr].bank.item.Length; i++)
				{
					NetMessage.SendData(MessageID.SyncEquipment, toWho, fromWho, null, plr,
						58 + Main.player[plr].armor.Length + Main.player[plr].dye.Length + Main.player[plr].miscEquips.Length + Main.player[plr].miscDyes.Length + 1 + i, Main.player[plr].bank.item[i].prefix, 0f, 0, 0, 0);
				}
				for (var i = 0; i < Main.player[plr].bank2.item.Length; i++)
				{
					NetMessage.SendData(MessageID.SyncEquipment, toWho, fromWho, null, plr,
						58 + Main.player[plr].armor.Length + Main.player[plr].dye.Length + Main.player[plr].miscEquips.Length + Main.player[plr].miscDyes.Length + Main.player[plr].bank.item.Length + 1 + i, Main.player[plr].bank2.item[i].prefix, 0f, 0, 0, 0);
				}
				NetMessage.SendData(MessageID.SyncEquipment, toWho, fromWho, null,
					plr, 58 + Main.player[plr].armor.Length + Main.player[plr].dye.Length +
					Main.player[plr].miscEquips.Length + Main.player[plr].bank.item.Length + Main.player[plr].bank2.item.Length + 1, Main.player[plr].trashItem.prefix);

				for (var i = 0; i < Main.player[plr].bank3.item.Length; i++)
				{
					NetMessage.SendData(MessageID.SyncEquipment, toWho, fromWho, null, plr,
						58 + Main.player[plr].armor.Length + Main.player[plr].dye.Length +
					Main.player[plr].miscEquips.Length + Main.player[plr].bank.item.Length + Main.player[plr].bank2.item.Length + 2 + i, Main.player[plr].bank2.item[i].prefix, 0f, 0, 0, 0);
				}
				PlayerHooks.SyncPlayer(Main.player[plr], toWho, fromWho, false);
				if (!Netplay.Clients[plr].IsAnnouncementCompleted)
				{
					Netplay.Clients[plr].IsAnnouncementCompleted = true;
					if (Main.dedServ)
					{
						Console.WriteLine(Main.player[plr].name + GameLanguage.GetText("entergame"));
					}
				}
			}
			else
			{
				active = 0;
				NetMessage.SendData(MessageID.PlayerActive, -1, plr, NetworkText.Empty, plr, active, 0f, 0f, 0, 0, 0);
				if (Netplay.Clients[plr].IsAnnouncementCompleted)
				{
					Netplay.Clients[plr].IsAnnouncementCompleted = false;
					NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(Netplay.Clients[plr].Name + GameLanguage.GetText("leavegame")), new Color(255, 255, 240, 20), plr);
					if (Main.dedServ)
					{
						Console.WriteLine(Netplay.Clients[plr].Name + GameLanguage.GetText("leavegame"));
					}
					Netplay.Clients[plr].Name = "Anonymous";
				}
			}
		}

		private static void SendNPCHousesAndTravelShop(int plr)
		{
			var flag = false;
			for (var i = 0; i < 200; i++)
			{
				if (Main.npc[i].active && Main.npc[i].townNPC && NPC.TypeToHeadIndex(Main.npc[i].type) != -1)
				{
					if (!flag && Main.npc[i].type == 368)
					{
						flag = true;
					}
					var num = 0;
					if (Main.npc[i].homeless)
					{
						num = 1;
					}
					NetMessage.SendData(MessageID.NPCHome, plr, -1, NetworkText.Empty, i, (float)Main.npc[i].homeTileX, (float)Main.npc[i].homeTileY, (float)num, 0, 0, 0);
				}
			}
			if (flag)
			{
				NetMessage.SendTravelShop(plr);
			}
		}

		public static void SendAnglerQuest(int remoteClient)
		{
			if (Main.netMode != 2)
			{
				return;
			}
			if (remoteClient == -1)
			{
				for (var i = 0; i < 255; i++)
				{
					if (Netplay.Clients[i].State == 10)
					{
						NetMessage.SendData(MessageID.AnglerQuest, i, -1, NetworkText.FromLiteral(Main.player[i].name), Main.anglerQuest, 0f, 0f, 0f, 0, 0, 0);
					}
				}
				return;
			}
			if (Netplay.Clients[remoteClient].State == 10)
			{
				NetMessage.SendData(MessageID.AnglerQuest, remoteClient, -1, NetworkText.FromLiteral(Main.player[remoteClient].name), Main.anglerQuest, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		private static void EnsureLocalPlayerIsPresent()
		{
			if (!Main.autoShutdown)
			{
				return;
			}
			var flag = false;
			for (var i = 0; i < 255; i++)
			{
				if (Netplay.Clients[i].State == 10 && Netplay.Clients[i].Socket.GetRemoteAddress().IsLocalHost())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Console.WriteLine(Language.GetTextValue("Net.ServerAutoShutdown"));
				WorldFile.saveWorld();
				Netplay.disconnect = true;
			}
		}

		private void RegisterHandler()
		{
			_packethandler = new Dictionary<int, PacketHandlerDelegate>()
			{
				{ MessageID.SpawnPlayer, PlayerSpawn },
				{ MessageID.PlayerPvP, PlayerPVP },
				{ MessageID.NetModules, HandleNetModules },
				{ MessageID.TileChange, TileChange },
				{ MessageID.PlayerControls, PlayerControls },
				//{ MessageID.RequestChestOpen, RequestChestOpen }
			};
		}

		private bool TileChange(ref BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 2)
			{
				try
				{
					var p = Main.player[playerNumber];
					var player = p.GetServerPlayer();
					int action = reader.ReadByte();
					var X = reader.ReadInt16();
					var Y = reader.ReadInt16();
					var type = reader.ReadInt16();
					int style = reader.ReadByte();
					if (!player.Group.HasPermission("changetile"))
					{
						if (MWorld.TileMessageCD[playerNumber] == 0)
						{
							MessageSender.SendErrorMessage(playerNumber, "你没有权限改变这个物块");
							MWorld.TileMessageCD[playerNumber] = 60;
						}
						NetMessage.SendTileSquare(-1, X, Y, 4);
						return true;
					}
					//if (ServerSideCharacter.CheckSpawn(X, Y) && player.PermissionGroup.Name != "spadmin")
					//{
					//	player.SendErrorInfo("Warning: Spawn is protected from change");
					//	NetMessage.SendTileSquare(-1, X, Y, 4);
					//	return true;
					//}
					//else if (ServerSideCharacter.RegionManager.CheckRegion(X, Y, player))
					//{
					//	player.SendErrorInfo("Warning: You don't have permission to change this tile");
					//	NetMessage.SendTileSquare(-1, X, Y, 4);
					//	return true;
					//}
					//else if (player.PermissionGroup.Name == "criminal")
					//{
					//	player.SendErrorInfo("Warning: Criminals cannot change tiles");
					//	NetMessage.SendTileSquare(-1, X, Y, 4);
					//	return true;
					//}
				}
				catch (Exception ex)
				{
					CommandBoardcast.ConsoleError(ex);
				}
			}
			return false;
		}


		private bool PlayerPVP(ref BinaryReader reader, int playerNumber)
		{
			int playerId = reader.ReadByte();
			var pvp = reader.ReadBoolean();
			if(Main.netMode == 2)
			{
				if(ServerSideCharacter2.Config.PvpMode != JsonData.PVPMode.Normal)
				{
					NetMessage.SendData(MessageID.PlayerPvP, playerNumber, -1, NetworkText.FromLiteral(""), playerNumber);
					return true;
				}
			}
			
			return false;
		}
	}
}
