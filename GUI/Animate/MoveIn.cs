using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ServerSideCharacter2.GUI.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.GUI.Animate
{
	public class MoveIn : UIAnimation
	{
		public float _opacity = 0f;

		public Vector2 _finalPos;

		public MoveIn(float timeOut, Vector2 finalPos) : base(timeOut)
		{
			_finalPos = finalPos;
		}

		protected override void Animate(UIAnimatedState state)
		{
			state.Top.Set(_finalPos.Y, 0);
			state.Left.Set(_finalPos.X * Timer / TimeOut, 0);
		}
	}
}
