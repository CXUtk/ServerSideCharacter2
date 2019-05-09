using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Regions;
using ServerSideCharacter2.Utils;

namespace ServerSideCharacter2.Unions
{
	public class UnionManager
	{
		internal Dictionary<string, Union> Unions;

		public static int CurrencyType;
		public static int CustomCurrencyID;

		public UnionManager()
		{
			Unions = new Dictionary<string, Union>();
		}

		public bool ContainsUnion(string name)
		{
			return Unions.ContainsKey(name);
		}

		public Union Get(string name)
		{
			return Unions[name];
		}

		public UnionInfo GetUnionsData()
		{
			UnionInfo info = new UnionInfo();
			foreach(var union in Unions)
			{
				var u = union.Value;
				info.Unions.Add(u.GetSimplified());
			}
			return info;
		}


		private static Region FindNextRegion()
		{
			for(int i = 0; i < 20; i++)
			{
				string name = $"公会领地{i + 1}";
				if (ServerSideCharacter2.RegionManager.Contains(name))
				{
					var region = ServerSideCharacter2.RegionManager.Regions[name];
					if(region.OwnedUnionName == "")
					{
						return region;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// 创建领地，创建之前需要确保名字没有冲突
		/// </summary>
		/// <param name="name"></param>
		/// <param name="owner"></param>
		public bool CreateUnion(string name, ServerPlayer owner)
		{
			lock (this)
			{
				if (Unions.Count == 20) return false;
				var region = FindNextRegion();
				if (region == null)
				{
					throw new SSCException("没有可用的领地来分配");
				}
				Union union = new Union(name);
				union.Owner = owner.Name;
				owner.Union = union;
				owner.SyncUnionInfo();
				union.AddMember(owner);
				union.RegionName = region.Name;
				region.OwnedUnionName = union.Name;
				Unions.Add(name, union);
				return true;
			}
		}


		/// <summary>
		/// 移除领地，移除之前需要确保名字存在
		/// </summary>
		/// <param name="name"></param>
		public void RemoveUnion(string name)
		{
			bool lockTaken = false;
			Monitor.TryEnter(this, 3000, ref lockTaken);
			if (lockTaken)
			{
				try
				{
					var union = Unions[name];
					var members = union.Members.ToList();
					var region = union.OwnedRegion;
					if(region != null)
					{
						Console.WriteLine($"解散领地{region.Name}开始");
						region.ResetAsUnion();
						region.OwnedUnionName = "";
					}
					Unions.Remove(name);
					foreach(var member in members)
					{
						var player = ServerSideCharacter2.PlayerCollection.Get(member);
						player.SendMessageBox($"你所在的公会 {name} 已经解散！", 180, Color.OrangeRed);
						if(player != null)
						{
							player.Union = null;
							player.SyncUnionInfo();
						}
					}
				}
				catch(Exception ex)
				{
					CommandBoardcast.ConsoleError(ex);
				}
				finally
				{
					Monitor.Exit(this);
				}
			}
			else
			{
				throw new SSCException("死锁发生");
			}
		}

	}
}
