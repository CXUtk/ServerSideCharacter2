using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerSideCharacter2.Regions
{
	public delegate void PlayerInteractionHandler(Region sender, ServerPlayer player);
	public class Region : IName
	{
		public string Name { get; set; }
		[JsonConverter(typeof(MyRectangleConverter))]
		public Rectangle Area { get; set; }
		public string OwnerName { get; set; }
		public PVPMode PVP { get; set; }
		public bool Forbidden { get; set; }
		public string OwnedUnionName { get; set; }
		public event PlayerInteractionHandler OnEnter;
		public event PlayerInteractionHandler OnExit;
		[JsonIgnore]

		public ServerPlayer Owner
		{
			get
			{
				return ServerSideCharacter2.PlayerCollection.Get(OwnerName);
			}
		}

		public Region OwnedRegion
		{
			get
			{
				return ServerSideCharacter2.RegionManager.Get(OwnedUnionName);
			}
		}

		public Region(string name, Rectangle rect)
		{
			Name = name;
			Area = rect;
			OwnerName = "";
			OwnedUnionName = "";
			PVP = 0;
			Forbidden = false;
		}

		public string WelcomeInfo()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format("欢迎来到领地 '{0}'!", Name));
			sb.Append(string.Format("领地主人: {0}，所属公会：{1}", Owner == null ? "无" : Owner.Name,
				OwnedUnionName == null ? "无" : OwnedUnionName));
			return sb.ToString();
		}

		public void EnterRegion(ServerPlayer player)
		{
			OnEnter?.Invoke(this, player);
		}


		public void LeaveRegion(ServerPlayer player)
		{
			OnExit?.Invoke(this, player);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Region)) return false;
			var reg = (Region)obj;
			return this.Name.Equals(reg.Name);
		}

		public Rectangle GetWorldHitBox()
		{
			return new Rectangle(Area.X * 16, Area.Y * 16, Area.Width * 16, Area.Height * 16);
		}

	}

	public class MyRectangleConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var rectangle = (Rectangle)value;

			var x = rectangle.X;
			var y = rectangle.Y;
			var width = rectangle.Width;
			var height = rectangle.Height;

			var o = JObject.FromObject(new { x, y, width, height });

			o.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var o = JObject.Load(reader);

			var x = GetTokenValue(o, "x") ?? 0;
			var y = GetTokenValue(o, "y") ?? 0;
			var width = GetTokenValue(o, "width") ?? 0;
			var height = GetTokenValue(o, "height") ?? 0;

			return new Rectangle(x, y, width, height);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Rectangle);
		}

		private static int? GetTokenValue(JObject o, string tokenName)
		{
			JToken t;
			return o.TryGetValue(tokenName, StringComparison.InvariantCultureIgnoreCase, out t) ? (int)t : (int?)null;
		}
	}
}
