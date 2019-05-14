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
	public class ShopBuyHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				int id = reader.ReadInt32();
				int amount = reader.ReadInt16();
				var player = Main.player[playerNumber];
				var splayer = player.GetServerPlayer();
				var marketitem = ServerSideCharacter2.MarketManager.GetMarketItem(id);
				if (marketitem == null)
				{
					splayer.SendMessageBox("该商品不存在！", 120, Color.Red);
					CommandBoardcast.ConsoleError($"玩家 {player.name} 发来的购买物品封包 数据异常，可能已被篡改");
					return;
				}
				if (splayer.InMatch)
				{
					splayer.SendMessageBox("参与游戏时不允许购买物品", 120, Color.Red);
					return;
				}
				Item item = new Item();
				item.netDefaults(id);
				if (amount <= 0 || amount > item.maxStack)
				{
					splayer.SendMessageBox("购买数量不合法", 120, Color.Red);
					CommandBoardcast.ConsoleError($"玩家 {player.name} 发来的购买物品封包 数据异常，可能已被篡改");
					return;
				}
				long cost = marketitem.RealPrice * amount;
				if (!splayer.CheckGuCoin(cost, true))
				{
					splayer.SendMessageBox("你没有足够的咕币去购买这个物品", 120, Color.Red);
					return;
				}
				item.stack = amount;
				ServerSideCharacter2.MailManager.ServerSendMail(splayer, "商城购买", $"您成功的购买了 {amount} 个 {item.HoverName}，总共花费 {cost} 咕币",
					new List<Item>() { item });
				splayer.SendMessageBox($"购买成功，商品已经发送到您的邮件，请注意查收", 180, Color.LimeGreen);
			}
			
		}
	}
}
