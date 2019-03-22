using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using ServerSideCharacter2.JsonData;

namespace ServerSideCharacter2.Utils
{
	public class ServerUtils
	{

		public static string RandomGenString()
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			StringBuilder sb = new StringBuilder();
			int n = 2 + Main.rand.Next(9);
			for (int i = 0; i < n; i++)
			{
				sb.Append(chars[Main.rand.Next(chars.Length)]);
			}
			return sb.ToString();
		}
		public static void CopyToItemData(Item[] src, ItemInfo[] dest)
		{
			int size = src.Length;
			for(int i = 0; i < size; i++)
			{
				dest[i] = ItemInfo.CreateInfo(src[i]);
			}
		}

		public static void InfoToItem(ItemInfo[] src, Item[] dest)
		{
			int size = src.Length;
			for (int i = 0; i < size; i++)
			{
				dest[i] = src[i].ToItem();
			}
		}

		public static NetworkMode NetworkMode
		{
			get
			{
				return (NetworkMode)Main.netMode;
			}
		}

		public static Player LocalModPlayer
		{
			get
			{
				return Main.LocalPlayer;
			}
		}

	
	}

	public enum NetworkMode : byte
	{
		None,
		Client,
		Server
	}
}
