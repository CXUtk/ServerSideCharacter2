using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using ServerSideCharacter2.JsonData;
using System.IO;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Utils
{
	public static class ServerUtils
	{

		public static void WriteRect(this BinaryWriter bb, Rectangle rect)
		{
			
			bb.Write(rect.X);
			bb.Write(rect.Y);
			bb.Write(rect.Width);
			bb.Write(rect.Height);
		}

		public static Rectangle ReadRect(this BinaryReader bb)
		{
			Rectangle rect = new Rectangle();
			rect.X = bb.ReadInt32();
			rect.Y = bb.ReadInt32();
			rect.Width = bb.ReadInt32();
			rect.Height = bb.ReadInt32();
			return rect;
		}

		public static int TryGetInt(this ServerPlayer player, string name)
		{
			if (player.ContainsValueName(name))
			{
				return (int)((long)player.GetExtraValue(name));
			}
			else
			{
				return 0;
			}
		}

		public static void TrySetInt(this ServerPlayer player, string name, int value)
		{
			player.ModifyExtraValue(name, (long)value);
		}


		public static void TryAddInt(this ServerPlayer player, string name, int inc)
		{
			if (player.ContainsValueName(name))
			{
				player.ModifyExtraValue(name, (long)player.GetExtraValue(name) + (long)inc);
			}
			else
			{
				player.ModifyExtraValue(name, (long)inc);
			}
		}

		public static string RandomGenString(int varlen = 9)
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var sb = new StringBuilder();
			var n = 2 + Main.rand.Next(varlen);
			for (var i = 0; i < n; i++)
			{
				sb.Append(chars[Main.rand.Next(chars.Length)]);
			}
			return sb.ToString();
		}
		public static void CopyToItemData(Item[] src, ItemInfo[] dest)
		{
			var size = src.Length;
			for(var i = 0; i < size; i++)
			{
				dest[i] = ItemInfo.Create();
				dest[i].FromItem(src[i]);
			}
		}

		public static void InfoToItem(ItemInfo[] src, Item[] dest)
		{
			var size = src.Length;
			for (var i = 0; i < size; i++)
			{
				dest[i] = src[i].ToItem();
			}
		}

		public static void CheckLocality(string msg)
		{
			if(Main.netMode == 2)
			{
				Console.WriteLine($"服务器端：{msg}");
			}
			else
			{
				Main.NewText($"客户端：{msg}");
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
