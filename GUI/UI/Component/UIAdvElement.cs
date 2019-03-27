using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ServerSideCharacter2.GUI.UI.Component
{
	public class UIAdvElement : UIElement
	{

		public static Rectangle GetRectIntersections(Rectangle r1, Rectangle r2)
		{
			int xmin = Math.Max(r1.X, r2.X);
			int xmax1 = r1.X + r1.Width;
			int xmax2 = r2.X + r2.Width;
			int xmax = Math.Min(xmax1, xmax2);
			if (xmax > xmin)
			{
				int ymin = Math.Max(r1.Y, r2.Y);
				int ymax1 = r1.Y + r1.Height;
				int ymax2 = r2.Y + r2.Height;
				int ymax = Math.Min(ymax1, ymax2);
				if (ymax > ymin)
				{
					Rectangle outrect = new Rectangle
					{
						X = xmin,
						Y = ymin,
						Width = xmax - xmin,
						Height = ymax - ymin
					};
					return outrect;
				}
			}
			return new Rectangle();
		}



		public override void Draw(SpriteBatch spriteBatch)
		{
			// 傻逼原版程序员不好好写剪裁效果，连矩形相交都不判
			bool overflowHidden = this.OverflowHidden;
			bool useImmediateMode = this._useImmediateMode;
			RasterizerState rasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
			Rectangle scissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
			SamplerState anisotropicClamp = SamplerState.AnisotropicClamp;

			var mystate = (RasterizerState)typeof(UIElement)
				.GetField("_overflowHiddenRasterizerState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
				.GetValue(null);
			if (useImmediateMode)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, mystate, null, Main.UIScaleMatrix);
				this.DrawSelf(spriteBatch);
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, mystate, null, Main.UIScaleMatrix);
			}
			else
			{
				this.DrawSelf(spriteBatch);
			}
			if (overflowHidden)
			{
				spriteBatch.End();
				Rectangle clippingRectangle = this.GetClippingRectangle(spriteBatch);
				spriteBatch.GraphicsDevice.ScissorRectangle = GetRectIntersections(scissorRectangle, clippingRectangle);
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, mystate, null, Main.UIScaleMatrix);
			}
			this.DrawChildren(spriteBatch);
			if (overflowHidden)
			{
				spriteBatch.End();
				spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, rasterizerState, null, Main.UIScaleMatrix);
			}
		}
	}
}
