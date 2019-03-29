using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ServerSideCharacter2.GUI.UI.Component;
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
			ServerSideCharacter2.ModTexturesTable.Clear();
			LoadTextures();
			Drawing.Box1 = ServerSideCharacter2.ModTexturesTable["Box"];
			Drawing.Box2 = ServerSideCharacter2.ModTexturesTable["Box2"];
			Drawing.Bar1 = ServerSideCharacter2.ModTexturesTable["Bar"];
		}

		private static void LoadTexture(string name)
		{
			var name1 = name.Substring("Graphics/".Length);
			if (ServerSideCharacter2.ModTexturesTable.ContainsKey(name1)) return;
			ServerSideCharacter2.ModTexturesTable.Add(name1, ServerSideCharacter2.Instance.GetTexture(name));
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
