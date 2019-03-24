using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ServerSideCharacter2.GUI.UI.Component
{
	/// <summary>
	/// 可以切换Enable以及Draw
	/// </summary>
	public abstract class ToggableElement : UIElement
	{
		public bool Enabled
		{
			get;
			set;
		}

		public bool Visible
		{
			get;
			set;
		}

		public ToggableElement()
		{
			Enabled = true;
			Visible = true;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (!Visible) return;
			base.DrawSelf(spriteBatch);
		}
	}
}
