using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.Utils;
using System.Collections.Generic;
using System.Text;

namespace ServerSideCharacter2.Regions
{
	public class Region : IName
	{
		public string Name { get; set; }
		public int OwnerGUID { get; set; }
		public Rectangle Area { get; set; }
		public string OwnerName { get; set; }

		[JsonIgnore]
		public ServerPlayer Owner
		{
			get
			{
				return ServerSideCharacter2.PlayerCollection.Get(OwnerGUID);
			}
		}

		public Region(string name, Rectangle rect)
		{
			Name = name;
			Area = rect;
			OwnerGUID = -1;
			OwnerName = "";
		}

		public string WelcomeInfo()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format("欢迎来到领地 '{0}'!", Name));
			sb.AppendLine(string.Format("领地主人: {0}", OwnerGUID == -1 ? "无主" : Owner.Name));
			sb.AppendLine(string.Format("领地面积: {0}", Area.ToString()));
			return sb.ToString();
		}

		public string LeaveInfo()
		{
			return string.Format("你离开了领地 '{0}'", Name);
		}
	}
}
