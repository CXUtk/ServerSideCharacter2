using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using ServerSideCharacter2.Utils;

namespace ServerSideCharacter2.Network
{
	public class PacketHandler : MessageDispatcher<SSCMessageType>
	{
		public bool DispatchPacket(BinaryReader reader, int playerNumber)
		{
			SSCMessageType msgType = (SSCMessageType)reader.ReadInt32();
			return Dispatch(msgType, reader, playerNumber);
		}



		protected override void RegisterMethod()
		{
			_method = new Dictionary<SSCMessageType, MessagePatchDelegate>
			{
				{ SSCMessageType.SyncPlayerBank, SyncPlayerBank },
			};
		}

		private bool SyncPlayerBank(ref BinaryReader reader, int playerNumber)
		{
			return false;
		}

	}
}
