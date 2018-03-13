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

namespace ServerSideCharacter2.GUI.UI
{
	public class UIDraggableState : UIAnimatedState
	{

		private UIElement _element;

		private Vector2 _offset;
		protected bool Dragging = false;

		public virtual void AppendDraggableElement(UIElement element)
		{
			_element = element;
			_element.OnMouseDown += new MouseEvent(DragStart);
			_element.OnMouseUp += new MouseEvent(DragEnd);
			Append(_element);
		}

		protected virtual void OnDraw(SpriteBatch sb)
		{

		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (_element.ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
				Main.LocalPlayer.showItemIcon = false;
			}
			if (Dragging)
			{
				_element.Left.Set(Main.MouseScreen.X - _offset.X, 0f);
				_element.Top.Set(Main.MouseScreen.Y - _offset.Y, 0f);
				Recalculate();
			}
			OnDraw(spriteBatch);
		}


		private void DragStart(UIMouseEvent evt, UIElement listeningElement)
		{
			if (ServerSideCharacter2.UIMouseLocker == null)
			{
				_offset = new Vector2(evt.MousePosition.X - _element.Left.Pixels, evt.MousePosition.Y - _element.Top.Pixels);
				Dragging = true;
				ServerSideCharacter2.UIMouseLocker = this;
			}
		}

		private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
		{
			Vector2 end = evt.MousePosition;
			Dragging = false;
			_element.Left.Set(end.X - _offset.X, 0f);
			_element.Top.Set(end.Y - _offset.Y, 0f);
			Recalculate();
			ServerSideCharacter2.UIMouseLocker = null;
		}
	}
}
