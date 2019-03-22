﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.UI;

namespace ServerSideCharacter2.GUI.UI.Component
{
	public class UISlider : UIScrollbar
	{
		
		// Token: 0x060023C8 RID: 9160 RVA: 0x0047A648 File Offset: 0x00478848
		public UISlider() : base()
		{
		}

		

		// Token: 0x060023CB RID: 9163 RVA: 0x0047A6D4 File Offset: 0x004788D4
		private Rectangle GetHandleRectangle()
		{
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			if (this._maxViewSize == 0f && this._viewSize == 0f)
			{
				this._viewSize = 1f;
				this._maxViewSize = 1f;
			}
			return new Rectangle((int)innerDimensions.X, (int)(innerDimensions.Y + innerDimensions.Height * (this._viewPosition / this._maxViewSize)) - 3, 20, (int)(innerDimensions.Height * (this._viewSize / this._maxViewSize)) + 7);
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x0047A75C File Offset: 0x0047895C
		private void DrawBar(SpriteBatch spriteBatch, Texture2D texture, Rectangle dimensions, Color color)
		{
			spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y - 6, dimensions.Width, 6), new Rectangle?(new Rectangle(0, 0, texture.Width, 6)), color);
			spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), new Rectangle?(new Rectangle(0, 6, texture.Width, 4)), color);
			spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y + dimensions.Height, dimensions.Width, 6), new Rectangle?(new Rectangle(0, texture.Height - 6, texture.Width, 6)), color);
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x0047A81C File Offset: 0x00478A1C
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			if (this._isDragging)
			{
				float num = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - this._dragYOffset;
				this._viewPosition = MathHelper.Clamp(num / innerDimensions.Height * this._maxViewSize, 0f, this._maxViewSize - this._viewSize);
			}
			Rectangle handleRectangle = this.GetHandleRectangle();
			Vector2 mousePosition = UserInterface.ActiveInstance.MousePosition;
			bool isHoveringOverHandle = this._isHoveringOverHandle;
			this._isHoveringOverHandle = handleRectangle.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y));
			if (!isHoveringOverHandle && this._isHoveringOverHandle && Main.hasFocus)
			{
				Main.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
			this.DrawBar(spriteBatch, this._texture, dimensions.ToRectangle(), Color.White);
			this.DrawBar(spriteBatch, this._innerTexture, handleRectangle, Color.White * ((this._isDragging || this._isHoveringOverHandle) ? 1f : 0.85f));
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x0047A93C File Offset: 0x00478B3C
		public override void MouseDown(UIMouseEvent evt)
		{
			// base.MouseDown(evt);
			if (evt.Target == this)
			{
				Rectangle handleRectangle = this.GetHandleRectangle();
				if (handleRectangle.Contains(new Point((int)evt.MousePosition.X, (int)evt.MousePosition.Y)))
				{
					this._isDragging = true;
					this._dragYOffset = evt.MousePosition.Y - (float)handleRectangle.Y;
					return;
				}
				CalculatedStyle innerDimensions = base.GetInnerDimensions();
				float num = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - (float)(handleRectangle.Height >> 1);
				this._viewPosition = MathHelper.Clamp(num / innerDimensions.Height * this._maxViewSize, 0f, this._maxViewSize - this._viewSize);
			}
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x00019503 File Offset: 0x00017703
		public override void MouseUp(UIMouseEvent evt)
		{
			// base.MouseUp(evt);
			this._isDragging = false;
		}

		// Token: 0x04003FF5 RID: 16373
		private float _viewPosition;

		// Token: 0x04003FF6 RID: 16374
		private float _viewSize = 1f;

		// Token: 0x04003FF7 RID: 16375
		private float _maxViewSize = 20f;

		// Token: 0x04003FF8 RID: 16376
		private bool _isDragging;

		// Token: 0x04003FF9 RID: 16377
		private bool _isHoveringOverHandle;

		// Token: 0x04003FFA RID: 16378
		private float _dragYOffset;

		// Token: 0x04003FFB RID: 16379
		private Texture2D _texture;

		// Token: 0x04003FFC RID: 16380
		private Texture2D _innerTexture;
	}
}