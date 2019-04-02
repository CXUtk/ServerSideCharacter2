using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerSideCharacter2.Regions
{
	public class Region : IName
	{
		public string Name { get; set; }
		public int OwnerGUID { get; set; }
		[JsonConverter(typeof(MyRectangleConverter))]
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
			sb.Append(string.Format("领地面积: {0}", Area.ToString()));
			return sb.ToString();
		}

		public string LeaveInfo()
		{
			return string.Format("你离开了领地 '{0}'", Name);
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
