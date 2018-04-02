using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ServerSideCharacter2.GUI.Animate;

namespace ServerSideCharacter2.GUI.UI
{
	public class UIAnimatedState : UIState
	{
		protected UIAnimation _animation;

		public UIAnimatedState(UIAnimation animation = null)
		{
			_animation = animation;
		}

		public void setAnimation(UIAnimation animation)
		{
			_animation = animation;
		}

		public override void Update(GameTime gameTime)
		{
			if (_animation != null)
				_animation.Play(this);
			base.Update(gameTime);
		}

	}
}
