using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.UI;

namespace ServerSideCharacter2.GUI.UI.Component
{
	public class UISimpleList : UIAdvElement
	{
		private List<UIElement> _contents;

		public float ListPadding { get; set; }

		public UISimpleList()
		{
			_contents = new List<UIElement>();
			ListPadding = 5f;
		}
		
		public override void RecalculateChildren()
		{
			base.RecalculateChildren();
			var num = 0f;
			for (var i = 0; i < this._contents.Count; i++)
			{
				this._contents[i].Top.Set(num, 0f);
				this._contents[i].Recalculate();
				num += this._contents[i].GetOuterDimensions().Height + ListPadding;
			}
			Height.Set(num, 0f);
		}

		public void Clear()
		{
			_contents.Clear();
			RemoveAllChildren();
		}

		public void Add(UIElement element)
		{
			_contents.Add(element);
			Append(element);
		}

		public void Remove(UIElement element)
		{
			RemoveChild(element);
			_contents.Remove(element);
		}


	}
}
