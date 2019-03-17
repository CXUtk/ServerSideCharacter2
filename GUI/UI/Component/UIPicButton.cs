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
	public delegate void UIMouseEventHandler(UIElement target, Vector2 mousePosition);
	public class UIPicButton : UIElement
	{
		/// <summary>
		/// 标准颜色
		/// </summary>
		public Color ButtonDefaultColor { get; set; }

		/// <summary>
		/// 鼠标移上去的颜色
		/// </summary>
		public Color ButtonSecondColor { get; set; }

		/// <summary>
		/// 按钮的内部贴图
		/// </summary>
		public Texture2D Texture { get; set; }

		/// <summary>
		/// 鼠标移动到按钮上后显示的文本
		/// </summary>
		public string Tooltip { get; set; }

		public Rectangle? SourceRect
		{
			get;
			set;
		}

		private bool _dragging = false;
		private bool _mouseOver = false;
		private int _dragTime = 0;
		private float _alpha = 0f;
		private Vector2 _offset = new Vector2();


		public UIPicButton()
		{
			ButtonDefaultColor = Color.White;
			SourceRect = null;
		}


		public override void MouseOver(UIMouseEvent evt)
		{
			Main.PlaySound(12, -1, -1, 1, 1f, 0f);
			base.MouseOver(evt);
			_mouseOver = true;
			if (Tooltip != "")
			{
				ServerSideCharacter2.ShowTooltip = Tooltip;
			}
			//OnMouseHover(evt.Target, evt.MousePosition);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			_mouseOver = false;
		}

		public override void Click(UIMouseEvent evt)
		{
			// 如果拖动时间长于半秒默认不触发点击效果
			if (_dragTime > 30) return;
			base.Click(evt);
		}


		public override void MouseDown(UIMouseEvent evt)
		{
			_offset = new Vector2(evt.MousePosition.X - this.Left.Pixels, evt.MousePosition.Y - this.Top.Pixels);
			_dragging = true;
			base.MouseDown(evt);
		}

		public override void MouseUp(UIMouseEvent evt)
		{
			Vector2 end = evt.MousePosition;
			_dragging = false;
			Left.Set(end.X - _offset.X, 0f);
			Top.Set(end.Y - _offset.Y, 0f);
			Recalculate();
			_dragTime = 0;
			base.MouseUp(evt);
		}

		public override void Update(GameTime gameTime)
		{
			if (_dragging)
			{
				Vector2 end = Main.MouseScreen;
				Left.Set(end.X - _offset.X, 0f);
				Top.Set(end.Y - _offset.Y, 0f);
				if(Left.Pixels < 0)
				{
					Left.Set(0f, 0f);
				}
				else if (Left.Pixels + Width.Pixels > Main.screenWidth)
				{
					Left.Set(Main.screenWidth - Width.Pixels, 0f);
				}
				if (Top.Pixels < 0)
				{
					Top.Set(0f, 0f);
				}
				else if (Top.Pixels + Height.Pixels > Main.screenHeight)
				{
					Top.Set(Main.screenHeight - Height.Pixels, 0f);
				}
				_dragTime++;
			}
			if (_mouseOver)
			{
				if(_alpha < 1.0)
				{
					_alpha += 0.05f;
				}
			}
			else
			{
				if (_alpha > 0.0)
				{
					_alpha -= 0.05f;
				}
			}
			base.Update(gameTime);
		}


		protected override void DrawSelf(SpriteBatch sb)
		{
			CalculatedStyle innerDimension = GetInnerDimensions();
			Color c = Color.Lerp(ButtonDefaultColor, ButtonSecondColor, _alpha);
			Drawing.DrawAdvBox(sb, innerDimension.ToRectangle(), c, ServerSideCharacter2.ModTexturesTable["Box"], new Vector2(12, 12));
			Rectangle rect = innerDimension.ToRectangle();
			if (Texture != null)
				sb.Draw(Texture, new Rectangle(rect.X + 4, rect.Y + 4, rect.Width - 8, rect.Height - 8), SourceRect, 
					Color.White);
			base.DrawSelf(sb);
		}
	}
}
