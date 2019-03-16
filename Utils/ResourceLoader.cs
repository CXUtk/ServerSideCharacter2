using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Utils
{
	public class ResourceLoader
	{
		public static void LoadAll()
		{
			if (Main.dedServ) return;
			LoadTextures();
			Drawing.Box1 = ServerSideCharacter2.ModTexturesTable["Box"];
			Drawing.Box2 = ServerSideCharacter2.ModTexturesTable["Box2"];
			Drawing.Bar1 = ServerSideCharacter2.ModTexturesTable["Bar"];
		}

		private static void LoadTexture(string name)
		{
			ServerSideCharacter2.ModTexturesTable.Add(name.Substring("Graphics/".Length), ServerSideCharacter2.Instance.GetTexture(name));
		}

		private static void LoadTextures()
		{
			IDictionary<string, Texture2D> textures = (IDictionary<string, Texture2D>)(typeof(Mod).GetField("textures",
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(ServerSideCharacter2.Instance));

			var names = textures.Keys.Where((name) =>
			{
				return name.StartsWith("Graphics/");
			});
			foreach (var name in names)
				LoadTexture(name);
		}
	}
}
