using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ServerSideCharacter2.Utils;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ServerSideCharacter2.GUI.UI
{
    public class UIButtonDraggable : UIButton
    {
		public UIButtonDraggable(Texture2D texture, bool withBox = true) : base(texture, withBox)
		{

		}

		protected override void PostUpdate()
		{

		}
    }
}
