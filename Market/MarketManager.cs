using MySql.Data.MySqlClient;
using ServerSideCharacter2.JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Market
{
	public class MarketManager
	{
		public class MarketItem
		{
			public Item Item { get; }
			public int Price { get; }
			public int Discount { get; }
			public bool UnionOnly { get; }
			public int MinLv { get; }
			public string Class { get; }
			public byte CoinType { get; }
			public int RealPrice
			{
				get { return (int)(Price * (100f - Discount) / 100f); }
			}
			public MarketItem(Item item, int price, int discount, string classify, byte cointype, bool uniononly = false, int minlv = 0)
			{
				this.Item = item.Clone();
				this.Price = price;
				this.Class = classify;
				this.Discount = Discount;
				this.UnionOnly = uniononly;
				this.MinLv = minlv;
			}

			public override string ToString()
			{
				return $"物品名称：{Item.HoverName}，ID：{Item.netID}，价格：{Price}，折扣：{Discount}%，种类：{Class}，货币种类：{CoinType}，公会专属？：{UnionOnly}";
			}

			public SimplifiedMarketItem GetSimplified()
			{
				SimplifiedMarketItem item = new SimplifiedMarketItem();
				item.ItemID = Item.netID;
				item.Price = RealPrice;
				item.Discount = Discount;
				item.MinLv = MinLv;
				return item;
			}
		}

		private static Dictionary<int, MarketItem> MarketItems;
		public MarketManager()
		{
			MarketItems = new Dictionary<int, MarketItem>();
			Load();
		}

		public MarketInfo GetMarketInfoNormal(int plr)
		{
			MarketInfo info = new MarketInfo();
			info.PlayerCurrency = Main.player[plr].GetServerPlayer().GuCoin;
			foreach(var pair in MarketItems)
			{
				info.Items.Add(pair.Value.GetSimplified());
			}
			return info;

		}

		public MarketItem GetMarketItem(int id)
		{
			if (!MarketItems.ContainsKey(id)) return null;
			return MarketItems[id];
		}

		public void Load()
		{
			lock (this)
			{
				MarketItems.Clear();
				QQAuth.MySqlManager sqlmanager = new QQAuth.MySqlManager();
				sqlmanager.Connect();
				MySqlCommand cmd = sqlmanager.command;
				cmd.CommandText = $"select * from normalshop";
				using (MySqlDataReader mdr = cmd.ExecuteReader())
				{
					if (!mdr.HasRows) return;
					while (mdr.Read())
					{
						var ismod = (bool)mdr["IsMod"];
						Item item = new Item();
						if (ismod)
						{
							var modname = mdr["ModName"].ToString();
							var typename = mdr["TypeName"].ToString();
							item.netDefaults(ModLoader.GetMod(modname).ItemType(typename));
						}
						else
						{
							item.netDefaults((int)mdr["Type"]);
						}
						var belong = mdr["Class"].ToString();
						MarketItems.Add(item.type, new MarketItem(item, (int)mdr["Price"], (int)mdr["Discount"], belong,
							(byte)(int)mdr["Coin"], belong.Equals("公会")));
					}
				}
				cmd.Cancel();

				foreach(var item in MarketItems)
				{
					Console.WriteLine(item.ToString());
				}
			}
		}
	}
}
