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
	/// <summary>
	/// 高级UI面板：
	/// 用于绘制可自定义边框样式贴图的基本面版
	/// </summary>
	public class UIPages : UIElement
	{
		private Dictionary<string, UIElement> _pages;

		private UIElement _currentPage;

		public UIPages()
		{
			_pages = new Dictionary<string, UIElement>();
		}

		public void AddPage(string name, UIElement page)
		{
			if (!_pages.ContainsKey(name)) {
				page.Deactivate();
				Append(page);
				_pages.Add(name, page);
			}
			else
			{
				throw new SSCException("Page already exist");
			}
		}

		public void SetPage(string name)
		{
			if (_currentPage != null)
				_currentPage.Deactivate();
			if(!_pages.TryGetValue(name, out _currentPage))
			{
				throw new SSCException("Cannot find this page");
			}
			_currentPage.Activate();
		}

		protected override void DrawChildren(SpriteBatch spriteBatch)
		{
			_currentPage.Draw(spriteBatch);
		}

	}
}
