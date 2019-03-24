using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.UI.Chat;
using Terraria.Localization;
using Terraria.Net;
using Terraria.UI.Chat;

namespace ServerSideCharacter2.Network
{
	// Token: 0x02000439 RID: 1081
	public class NetTextModulePlus : NetModule
	{
		// Token: 0x06002553 RID: 9555 RVA: 0x004805DC File Offset: 0x0047E7DC
		public static NetPacket SerializeClientMessage(ChatMessage message)
		{
			NetPacket result = NetModule.CreatePacket<NetTextModule>(message.GetMaxSerializedSize());
			message.Serialize(result.Writer);
			return result;
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x0001A23A File Offset: 0x0001843A
		public static NetPacket SerializeServerMessage(NetworkText text, Color color)
		{
			return NetTextModule.SerializeServerMessage(text, color, byte.MaxValue);
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x00480604 File Offset: 0x0047E804
		public static NetPacket SerializeServerMessage(NetworkText text, Color color, byte authorId)
		{
			NetPacket result = NetModule.CreatePacket<NetTextModule>(1 + text.GetMaxSerializedSize() + 3);
			result.Writer.Write(authorId);
			text.Serialize(result.Writer);
			result.Writer.WriteRGB(color);
			return result;
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x0048064C File Offset: 0x0047E84C
		private bool DeserializeAsClient(BinaryReader reader, int senderPlayerId)
		{
			byte b = reader.ReadByte();
			string text = NetworkText.Deserialize(reader).ToString();
			Color c = reader.ReadRGB();
			if (b < 255)
			{
				Main.player[(int)b].chatOverhead.NewMessage(text, Main.chatLength / 2);
				text = NameTagHandler.GenerateTag(Main.player[(int)b].name) + " " + text;
			}
			Main.NewTextMultiline(text, false, c, -1);
			return true;
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x004806BC File Offset: 0x0047E8BC
		private bool DeserializeAsServer(BinaryReader reader, int senderPlayerId)
		{
			ChatMessage message = ChatMessage.Deserialize(reader);
			ChatManager.Commands.ProcessReceivedMessage(message, senderPlayerId);
			return true;
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x0001A248 File Offset: 0x00018448
		private void BroadcastRawMessage(ChatMessage message, byte author, Color messageColor)
		{
			NetManager.Instance.Broadcast(NetTextModule.SerializeServerMessage(NetworkText.FromLiteral(message.Text), messageColor), -1);
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x0001A266 File Offset: 0x00018466
		public override bool Deserialize(BinaryReader reader, int senderPlayerId)
		{
			return this.DeserializeAsServer(reader, senderPlayerId);
		}
	}
}
