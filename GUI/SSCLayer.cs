using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Social;
using Terraria.UI;

namespace ServerSideCharacter2.GUI
{
	public class SSCLayer : GameInterfaceLayer
	{
		private readonly GUIManager _UImanager;

		public SSCLayer(GUIManager manager) : this("SSC: UI", InterfaceScaleType.UI)
		{
			_UImanager = manager;
		}

		private SSCLayer(string name, InterfaceScaleType scaleType) : base(name, scaleType)
		{

		}

		protected override bool DrawSelf()
		{
			_UImanager.RunUI();
			return true;
		}
	}
}
