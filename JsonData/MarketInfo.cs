using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerSideCharacter2.Groups;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.JsonData
{
	public struct SimplifiedMarketItem
	{
		public int ItemID;
		public int Price;
		public int Discount;
		public int MinLv;
	}



	public class MarketInfo
	{
		public List<SimplifiedMarketItem> Items;
		public long PlayerCurrency;
		public MarketInfo()
		{
			Items = new List<SimplifiedMarketItem>();
		}
	}
}
