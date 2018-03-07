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
    public class UIButton : UIElement
    {
        /// <summary>
        /// Text appeared on the button
        /// </summary>
        public string ButtonText { get; set; }
        /// <summary>
        /// Color when mouse move on the button
        /// </summary>
        public Color ButtonChangeColor { get; set; }
        /// <summary>
        /// Default button color
        /// </summary>
        public Color ButtonDefaultColor { get; set; }
        /// <summary>
        /// Text color
        /// </summary>
        public Color ButtonTextColor { get; set; }
        /// <summary>
        /// True if you want the button using box's texture
        /// Default: True
        /// </summary>
        public bool WithBox { get; set; }
		/// <summary>
		/// Button's texture
		/// </summary>
		public Texture2D Texture { get; set; }

        private float alpha;

		private Color CurrentColor;

		private bool isMouseInside = false;

        public UIButton(Texture2D texture, bool withBox = true)
        {
			Texture = texture;
			alpha = 0f;
            ButtonText = "";
            ButtonChangeColor = Color.White * 0.75f;
            ButtonDefaultColor = Drawing.DefaultBoxColor * 0.75f;
			CurrentColor = ButtonDefaultColor;
            ButtonTextColor = Color.White;
			WithBox = withBox;
            
        }


		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			Main.PlaySound(12, -1, -1, 1, 1f, 0f);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			isMouseInside = false;
		}

		public override void Update(GameTime gameTime)
		{
			if (!isMouseInside)
			{
				if (alpha > 0)
					alpha -= 0.05f;
				CurrentColor = Color.Lerp(ButtonDefaultColor, ButtonChangeColor, alpha);
			}
			if (ContainsPoint(Main.MouseScreen))
			{
				isMouseInside = true;
				if (alpha < 1)
					alpha += 0.05f;
				CurrentColor = Color.Lerp(ButtonDefaultColor, ButtonChangeColor, alpha);
			}

			PostUpdate();
		}

		protected virtual void PostUpdate()
		{

		}

		protected override void DrawSelf(SpriteBatch sb)
		{

			CalculatedStyle innerDimension = GetInnerDimensions();
			if (WithBox)
			{
				Drawing.DrawAdvBox(sb, (int)innerDimension.X, (int)innerDimension.Y,
					(int)innerDimension.Width, (int)innerDimension.Height,
					CurrentColor, Drawing.Box1, new Vector2(10, 10));
			}
			else
			{
				sb.Draw(Texture, innerDimension.ToRectangle(), CurrentColor);
			}
			if (ButtonText != "")
			{
				Vector2 txtMeasure = Main.fontMouseText.MeasureString(ButtonText);
				Terraria.Utils.DrawBorderStringFourWay(sb, Main.fontMouseText, ButtonText,
					innerDimension.Center().X - txtMeasure.X / 2, innerDimension.Center().Y - txtMeasure.Y / 2,
					ButtonTextColor,
					Color.Black, Vector2.Zero);
			}
		}
    }
}
