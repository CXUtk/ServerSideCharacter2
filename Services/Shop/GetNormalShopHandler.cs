using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.GUI.UI;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Services.Shop
{
	public class GetNormalShopHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.GetNormalShop);
				p.Write(JsonConvert.SerializeObject(ServerSideCharacter2.MarketManager.GetMarketInfoNormal(playerNumber), Formatting.None));
				p.Send(playerNumber);
				CommandBoardcast.ConsoleMessage($"商城信息已经发送给{Main.player[playerNumber].name}");
			}
			else
			{
				var data = reader.ReadString();
				var info = JsonConvert.DeserializeObject<MarketInfo>(data);
				lock (NormalShopState.Instance)
				{
					NormalShopState.Instance.ClearItems();
					NormalShopState.Instance.Apply(info);
				}
			}
		}
	}
}
