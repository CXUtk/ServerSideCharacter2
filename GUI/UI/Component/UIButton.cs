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
	/// 我写的垃圾代码，以后重构
	/// </summary>
    public class UIButton : ToggableElement
	{
        /// <summary>
        /// 显现在按钮上的文本
        /// </summary>
        public string ButtonText { get; set; }
        /// <summary>
        /// 鼠标移动到按钮上后需要变成的颜色
        /// </summary>
        public Color ButtonChangeColor { get; set; }
        /// <summary>
        /// 按钮的默认颜色
        /// </summary>
        public Color ButtonDefaultColor { get; set; }
        /// <summary>
        /// 按钮文本的颜色
        /// </summary>
        public Color ButtonTextColor { get; set; }
        /// <summary>
        /// 按钮是否有默认边框，默认为true
        /// </summary>
        public bool WithBox { get; set; }
		/// <summary>
		/// 按钮的内部贴图
		/// </summary>
		public Texture2D Texture { get; set; }
        /// <summary>
        /// 鼠标移动到按钮上后显示的文本
        /// </summary>
		public string Tooltip { get; set; }

		public Texture2D BoxTexture { get; set; }
		public Vector2 CornerSize { get; set; }

        private float _alpha;

		private Color _currentColor;

		public new event UIElement.MouseEvent OnClick;
		public new event UIElement.MouseEvent OnMouseDown;
		public new event UIElement.MouseEvent OnMouseUp;

		public UIButton(Texture2D texture, bool withBox = true)
        {
			Texture = texture;
			_alpha = 0f;
            ButtonText = "";
            ButtonChangeColor = Color.White;
            ButtonDefaultColor = Drawing.DefaultBoxColor * 0.75f;
			_currentColor = ButtonDefaultColor;
            ButtonTextColor = Color.White;
			WithBox = withBox;
			Tooltip = "";
			Enabled = true;
			CornerSize = new Vector2(10, 10);
			BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack2"];
        }


		public override void MouseOver(UIMouseEvent evt)
		{
			if (Enabled)
				Main.PlaySound(12, -1, -1, 1, 1f, 0f);
			base.MouseOver(evt);
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			OnMouseDown?.Invoke(evt, this);
			base.MouseDown(evt);
		}
		public override void Click(UIMouseEvent evt)
		{
			if (Enabled)
				OnClick?.Invoke(evt, this);
			base.Click(evt);
		}

		public override void MouseUp(UIMouseEvent evt)
		{
			OnMouseUp?.Invoke(evt, this);
			base.MouseUp(evt);
		}


		public override void Update(GameTime gameTime)
		{
            if(Tooltip != "" && IsMouseHovering)
			{
				ServerSideCharacter2.ShowTooltip = Tooltip;
			}

			if (!IsMouseHovering)
			{
				if (_alpha > 0)
					_alpha -= 0.05f;
				_currentColor = Color.Lerp(ButtonDefaultColor, ButtonChangeColor, _alpha);
			}
			else
			{
				if (_alpha < 1.0f)
					_alpha += 0.05f;
				_currentColor = Color.Lerp(ButtonDefaultColor, ButtonChangeColor, _alpha);
			}
			base.Update(gameTime);
		}
		protected override void DrawSelf(SpriteBatch sb)
		{
			CalculatedStyle innerDimension = GetInnerDimensions();
			if (WithBox)
			{
				Drawing.DrawAdvBox(sb, (int)innerDimension.X, (int)innerDimension.Y,
					(int)innerDimension.Width, (int)innerDimension.Height,
					_currentColor, BoxTexture, CornerSize);
			}
			else
			{
				if (Texture != null)
					sb.Draw(Texture, innerDimension.ToRectangle(), _currentColor);
			}
			if (ButtonText != "")
			{
				Vector2 txtMeasure = Main.fontMouseText.MeasureString(ButtonText);
				Terraria.Utils.DrawBorderStringFourWay(sb, Main.fontMouseText, ButtonText,
					innerDimension.Center().X, innerDimension.Center().Y + 4,
					ButtonTextColor,
					Color.Black, txtMeasure * 0.5f);
			}
		}
    }
}
