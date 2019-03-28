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

namespace ServerSideCharacter2.GUI.UI.Component
{
	public class UIImageResizable : UIElement
	{
		private Texture2D _texture;

		public float ImageScale = 1f;

		public bool UsePosition
		{
			get;set;
		}

		//public Vector2 TextureSize
		//{
		//	get
		//	{
		//		return _texture.Size();
		//	}
		//}


		/// <summary>
		/// 鼠标移动到按钮上后显示的文本
		/// </summary>
		public string Tooltip { get; set; }



		public UIImageResizable(Texture2D texture)
		{
			UsePosition = true;
			this._texture = texture;
			base.Width.Set(this._texture.Width, 0f);
			base.Height.Set(this._texture.Height, 0f);
		}

		public void SetImage(Texture2D texture)
		{
			this._texture = texture;
			base.Width.Set(this._texture.Width, 0f);
			base.Height.Set(this._texture.Height, 0f);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (Tooltip != "" && IsMouseHovering)
			{
				ServerSideCharacter2.ShowTooltip = Tooltip;
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
			CalculatedStyle dimensions = base.GetDimensions();
			if (UsePosition)
			{
				spriteBatch.Draw(this._texture, dimensions.Position(), null, Color.White, 0f, Vector2.Zero, this.ImageScale, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(this._texture, dimensions.Center(), null, Color.White, 0f, _texture.Size() * 0.5f, this.ImageScale, SpriteEffects.None, 0f);
			}
			spriteBatch.End();
			spriteBatch.Begin();
		}
	}
}
