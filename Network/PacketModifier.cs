using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ModLoader;
using Terraria.Social;

namespace ServerSideCharacter2.Network
{
	public static class PacketModifier
	{
		/// <summary>
		/// Modify the world data packet so that it sends serversidecharacter information
		/// </summary>
		/// <param name="writer"></param>
		public static void ModifyWorldData(ref BinaryWriter writer)
		{
			writer.Write((int)Main.time);
			BitsByte bb7 = (byte)0;
			bb7[0] = Main.dayTime;
			bb7[1] = Main.bloodMoon;
			bb7[2] = Main.eclipse;
			writer.Write(bb7);
			writer.Write((byte)Main.moonPhase);
			writer.Write((short)Main.maxTilesX);
			writer.Write((short)Main.maxTilesY);
			writer.Write((short)Main.spawnTileX);
			writer.Write((short)Main.spawnTileY);
			writer.Write((short)Main.worldSurface);
			writer.Write((short)Main.rockLayer);
			writer.Write(Main.worldID);
			writer.Write(Main.worldName);
			writer.Write(Main.ActiveWorldFileData.UniqueId.ToByteArray());
			writer.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
			writer.Write((byte)Main.moonType);
			writer.Write((byte)WorldGen.treeBG);
			writer.Write((byte)WorldGen.corruptBG);
			writer.Write((byte)WorldGen.jungleBG);
			writer.Write((byte)WorldGen.snowBG);
			writer.Write((byte)WorldGen.hallowBG);
			writer.Write((byte)WorldGen.crimsonBG);
			writer.Write((byte)WorldGen.desertBG);
			writer.Write((byte)WorldGen.oceanBG);
			writer.Write((byte)Main.iceBackStyle);
			writer.Write((byte)Main.jungleBackStyle);
			writer.Write((byte)Main.hellBackStyle);
			writer.Write(Main.windSpeedSet);
			writer.Write((byte)Main.numClouds);
			for (var l = 0; l < 3; l++)
			{
				writer.Write(Main.treeX[l]);
			}
			for (var m = 0; m < 4; m++)
			{
				writer.Write((byte)Main.treeStyle[m]);
			}
			for (var n = 0; n < 3; n++)
			{
				writer.Write(Main.caveBackX[n]);
			}
			for (var num9 = 0; num9 < 4; num9++)
			{
				writer.Write((byte)Main.caveBackStyle[num9]);
			}
			if (!Main.raining)
			{
				Main.maxRaining = 0f;
			}
			writer.Write(Main.maxRaining);
			BitsByte bb8 = 0;
			bb8[0] = WorldGen.shadowOrbSmashed;
			bb8[1] = NPC.downedBoss1;
			bb8[2] = NPC.downedBoss2;
			bb8[3] = NPC.downedBoss3;
			bb8[4] = Main.hardMode;
			bb8[5] = NPC.downedClown;
			bb8[6] = true;	// ssc mode here
			bb8[7] = NPC.downedPlantBoss;
			writer.Write(bb8);
			BitsByte bb9 = 0;
			bb9[0] = NPC.downedMechBoss1;
			bb9[1] = NPC.downedMechBoss2;
			bb9[2] = NPC.downedMechBoss3;
			bb9[3] = NPC.downedMechBossAny;
			bb9[4] = (Main.cloudBGActive >= 1f);
			bb9[5] = WorldGen.crimson;
			bb9[6] = Main.pumpkinMoon;
			bb9[7] = Main.snowMoon;
			writer.Write(bb9);
			BitsByte bb10 = 0;
			bb10[0] = Main.expertMode;
			bb10[1] = Main.fastForwardTime;
			bb10[2] = Main.slimeRain;
			bb10[3] = NPC.downedSlimeKing;
			bb10[4] = NPC.downedQueenBee;
			bb10[5] = NPC.downedFishron;
			bb10[6] = NPC.downedMartians;
			bb10[7] = NPC.downedAncientCultist;
			writer.Write(bb10);
			BitsByte bb11 = 0;
			bb11[0] = NPC.downedMoonlord;
			bb11[1] = NPC.downedHalloweenKing;
			bb11[2] = NPC.downedHalloweenTree;
			bb11[3] = NPC.downedChristmasIceQueen;
			bb11[4] = NPC.downedChristmasSantank;
			bb11[5] = NPC.downedChristmasTree;
			bb11[6] = NPC.downedGolemBoss;
			bb11[7] = BirthdayParty.PartyIsUp;
			writer.Write(bb11);
			BitsByte bb12 = 0;
			bb12[0] = NPC.downedPirates;
			bb12[1] = NPC.downedFrost;
			bb12[2] = NPC.downedGoblins;
			bb12[3] = Sandstorm.Happening;
			bb12[4] = DD2Event.Ongoing;
			bb12[5] = DD2Event.DownedInvasionT1;
			bb12[6] = DD2Event.DownedInvasionT2;
			bb12[7] = DD2Event.DownedInvasionT3;
			writer.Write(bb12);
			writer.Write((sbyte)Main.invasionType);
			if (!ModNet.AllowVanillaClients)
			{
				// We have to call `WorldIO.SendModData(binaryWriter)` using reflection
				var type = typeof(Main).Assembly.GetType("Terraria.ModLoader.IO.WorldIO");
				var method = type.GetMethod("SendModData", new[] { typeof(BinaryWriter) });
				method.Invoke(null, new object[] { writer });
			}
			if (SocialAPI.Network != null)
			{
				writer.Write(SocialAPI.Network.GetLobbyId());
			}
			else
			{
				writer.Write(0uL);
			}
			writer.Write(Sandstorm.IntendedSeverity);
		}
	}
}
