using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ServerSideCharacter2.Utils;
using Terraria.ModLoader;
using Terraria;

namespace ServerSideCharacter2.Regions
{

	public class RegionManager
	{
		public Dictionary<string, Region> Regions;

		public RegionManager()
		{
			Regions = new Dictionary<string, Region>();

		}
		public void CreateNewRegion(Rectangle rect, string name, ServerPlayer player)
		{

			Region playerRegion = new Region(name, rect)
			{
				OwnerName = player.Name
			};
			Regions.Add(name, playerRegion);
			player.Regions.Add(playerRegion);

		}
		public void CreateNewRegion(Rectangle rect, string name)
		{

			Region playerRegion = new Region(name, rect)
			{
				OwnerName = "无"
			};
			Regions.Add(name, playerRegion);

		}

		public void AddRegion(Region region)
		{

			Regions.Add(region.Name, region);

		}

		public void RemoveRegionWithName(string name)
		{
			if (Regions.ContainsKey(name))
			{
				if (Regions[name].Owner != null)
				{
					var owner = Regions[name].Owner;
					owner.Regions.Remove(Regions[name]);
				}
				Regions.Remove(name);
			}
			else
			{
				throw new SSCException($"无法移除领地 {name}，不存在该领地");
			}

		}

		public bool Contains(string name)
		{
			return Regions.ContainsKey(name);
		}

		public bool CheckPlayerRegionMax(ServerPlayer player)
		{
			return Regions.Count(info => info.Value.Owner != null && info.Value.OwnerName.Equals(player)) < ServerSideCharacter2.Config.PlayerMaxRegions;
		}

		public bool CheckRegionConflict(Rectangle rect)
		{
			return Regions.All(region => !region.Value.Area.Intersects(rect));
		}

		//public void ReadRegion()
		//{
		//	if (!File.Exists(_filePath))
		//	{
		//		string json = JsonConvert.SerializeObject(_regions);
		//		using (StreamWriter sw = new StreamWriter(_filePath))
		//		{
		//			sw.Write(json);
		//		}
		//	}
		//	else
		//	{
		//		string json = File.ReadAllText(_filePath);
		//		_regions = JsonConvert.DeserializeObject<RegionData>(json);
		//	}
		//}

		//public void WriteRegion()
		//{
		//	lock (_regions)
		//	{
		//		string json = JsonConvert.SerializeObject(_regions, Newtonsoft.Json.Formatting.Indented);
		//		using (StreamWriter sw = new StreamWriter(_filePath))
		//		{
		//			sw.Write(json);
		//		}
		//	}

		//}

		public bool CheckRegion(int X, int Y, ServerPlayer player)
		{
			Vector2 tilePos = new Vector2(X, Y);
			if (X < 0 || X >= Main.maxTilesX || Y < 0 || Y >= Main.maxTilesY) return true;
			if (player.Group.Name == "criminal") return true;
			foreach (var pair in Regions)
			{
				var region = pair.Value;
				if (region.Area.Contains(X, Y) && (region.Owner.Equals(player) || (player.Union != null && player.Union.RegionName != region.Name)))
				{
					return false;
				}
			}
			return true;
		}

		public bool CheckRegionSize(ServerPlayer player, Rectangle area)
		{
			return player.Group.IsSuperAdmin || (area.Width < ServerSideCharacter2.Config.MaxRegionWidth
				&& area.Height < ServerSideCharacter2.Config.MaxRegionHeight);
		}

		//public void ShareRegion(ServerPlayer p, ServerPlayer target, string name)
		//{
		//	int index = _regions.ServerRegions.FindIndex(region => region.Name == name);
		//	if (index == -1)
		//	{
		//		p.SendErrorInfo("Cannot find this region!");
		//		return;
		//	}
		//	var reg = _regions.ServerRegions[index];
		//	reg.SharedOwner.Add(target.UUID);
		//	p.SendSuccessInfo("Successfully shared " + reg.Name + " to " + target.Name);
		//	target.SendSuccessInfo(p.Name + " shared region " + reg.Name + " with you!");
		//}

		public Region Get(string name)
		{
			if (!Regions.ContainsKey(name)) return null;
			return Regions[name];
		}

		internal bool ValidRegion(ServerPlayer player, string name, Rectangle area, out string errmsg)
		{
			if (name.Length < 2 || name.Length > 20)
			{
				errmsg = "领地名字长度不合法，应为2-20个字符！";
				return false;
			}
			else if (Contains(name))
			{
				errmsg = "已经存在相同名字的领地！";
				return false;
			}
			else if (Regions.Count >= ServerSideCharacter2.Config.MaxRegions)
			{
				errmsg = "领地数量达到服务器规定上限";
				return false;
			}
			else if (!CheckPlayerRegionMax(player))
			{
				errmsg = "领地数量达到玩家上限";
				return false;
			}
			else if (!CheckRegionConflict(area))
			{
				errmsg = "该领地与其他领地存在冲突";
				return false;
			}
			else if (!CheckRegionSize(player, area))
			{
				errmsg = "该领地的尺寸过大";
				return false;
			}
			errmsg = "";
			return true;
		}

		internal void WriteRegions(BinaryWriter binaryWriter)
		{
			binaryWriter.Write(Regions.Count);
			foreach (var pair in Regions)
			{
				var region = pair.Value;
				binaryWriter.Write(region.Name);
				binaryWriter.Write(region.Owner == null ? "" : region.Owner.Name);
				binaryWriter.WriteRect(region.Area);
				binaryWriter.Write((byte)region.PVP);
				binaryWriter.Write(region.Forbidden);
			}
		}
	}
}
