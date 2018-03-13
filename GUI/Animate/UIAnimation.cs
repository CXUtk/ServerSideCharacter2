using ServerSideCharacter2.GUI.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.UI;

namespace ServerSideCharacter2.GUI.Animate
{
	public abstract class UIAnimation
	{
		public float TimeOut { get; set; }

		public bool IsStopped { get; set; }

		protected float Timer { get; set; }

		public UIAnimation(float timeOut)
		{
			TimeOut = timeOut;
		}

		public void Play(UIAnimatedState state)
		{
			if (Timer < TimeOut)
			{
				Timer++;
				Animate(state);
			}
			IsStopped = true;
		}

		public void Reset()
		{
			Timer = 0;
			IsStopped = false;
		}


		protected abstract void Animate(UIAnimatedState state);
	}
}
