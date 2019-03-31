using Microsoft.Xna.Framework;
using ServerSideCharacter2.Crypto;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services.Misc
{
	public class ReceiveRSA : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				var publicKey = reader.ReadString();
				RSACrypto.SetPublicKey(publicKey);
			}
		}
	}
}
