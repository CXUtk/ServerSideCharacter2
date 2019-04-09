using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
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

		/// <summary>
		/// 创建领地，创建之前需要确保名字没有冲突
		/// </summary>
		/// <param name="name"></param>
		/// <param name="owner"></param>
		public void CreateUnion(string name, ServerPlayer owner)
		{
			bool lockTaken = false;
			Monitor.TryEnter(this, 3000, ref lockTaken);
			if (lockTaken)
			{
				try
				{
					Union union = new Union(name);
					union.Owner = owner.Name;
					union.AddMember(owner);
					Unions.Add(name, union);
					owner.SyncUnionInfo();
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

		public void Donate(ServerPlayer player, string name, int amount)
		{
			var union = Unions[name];
			union.IncreaseEXP(amount);
			foreach(var member in union.Members)
			{
				var splayer = ServerSideCharacter2.PlayerCollection.Get(member);
				splayer?.SendInfoMessage($"玩家 {player.Name} 给公会捐献了 {amount} 财富", Color.Green);
			}
			CommandBoardcast.ConsoleMessage($"玩家 {player.Name} 给公会 {name} 捐献了 {amount} 财富");
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
					Unions.Remove(name);
					foreach(var member in members)
					{
						var player = ServerSideCharacter2.PlayerCollection.Get(member);
						player.SendMessageBox($"你所在的公会 {name} 已经解散！", 180, Color.OrangeRed);
						if(player != null)
						{
							player.SetUnion("");
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
