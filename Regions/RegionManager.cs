using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Regions
{

	public class RegionManager
	{
		private RegionData _regions;
		public Dictionary<string, Region> Regions;
		private static string _filePath = "SSC/regions.json";

		public RegionManager()
		{
			Regions = new Dictionary<string, Region>();

		}
		public void CreateNewRegion(Rectangle rect, string name, ServerPlayer player)
		{
			Region playerRegion = new Region(name, rect);
			Regions.Add(name, playerRegion);
			player.Regions.Add(name);
		}

		public void RemoveRegionWithName(string name)
		{
			if (Regions.ContainsKey(name))
			{
				var owner = Regions[name].Owner;
				owner.Regions.Remove(name);
				Regions.Remove(name);
			}
			else
			{
				throw new SSCException($"无法移除领地 {name}，不存在该领地");
			}
		}

		public bool HasNameConflect(string name)
		{
			return Regions.ContainsKey(name);
		}

		public bool CheckPlayerRegionMax(ServerPlayer player)
		{
			return _regions.ServerRegions.Count(info => info.Owner.Equals(player)) < ServerSideCharacter2.Config.PlayerMaxRegions;
		}

		public bool CheckRegionConflict(Rectangle rect)
		{
			return _regions.ServerRegions.All(region => !region.Area.Intersects(rect));
		}

		public void ReadRegion()
		{
			if (!File.Exists(_filePath))
			{
				string json = JsonConvert.SerializeObject(_regions);
				using (StreamWriter sw = new StreamWriter(_filePath))
				{
					sw.Write(json);
				}
			}
			else
			{
				string json = File.ReadAllText(_filePath);
				_regions = JsonConvert.DeserializeObject<RegionData>(json);
			}
		}

		public void WriteRegion()
		{
			lock (_regions)
			{
				string json = JsonConvert.SerializeObject(_regions, Newtonsoft.Json.Formatting.Indented);
				using (StreamWriter sw = new StreamWriter(_filePath))
				{
					sw.Write(json);
				}
			}

		}

		//public bool CheckRegion(int X, int Y, ServerPlayer player)
		//{
		//	Vector2 tilePos = new Vector2(X, Y);
		//	foreach (var regions in _regions.ServerRegions)
		//	{
		//		if (regions.Area.Contains(X, Y) && !regions.Owner.Equals(player) && !regions.SharedOwner.Contains(player.UUID))
		//		{
		//			return true;
		//		}
		//	}
		//	return false;
		//}

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

		internal bool ValidRegion(ServerPlayer player, string name, Rectangle area)
		{
			return !HasNameConflect(name) && _regions.ServerRegions.Count < ServerSideCharacter2.Config.MaxRegions
				   && CheckPlayerRegionMax(player) && CheckRegionConflict(area)
				   && CheckRegionSize(player, area);
		}
	}
}
