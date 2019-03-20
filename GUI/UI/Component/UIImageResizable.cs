//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using ServerSideCharacter2.Utils;
//using Terraria;
//using Terraria.ModLoader;
//using Terraria.UI;

//namespace ServerSideCharacter2.GUI.UI.Component
//{
//	public class UIImageResizable : UIElement
//	{
//		private Texture2D _texture;


//		public float ImageScale = 1f;

//		public UIImageResizable(Texture2D texture)
//		{
//			this._texture = texture;
//			base.Width.Set(this._texture.Width, 0f);
//			base.Height.Set(this._texture.Height, 0f);
//		}

//		public void SetImage(Texture2D texture)
//		{
//			this._texture = texture;
//			base.Width.Set(this._texture.Width, 0f);
//			base.Height.Set(this._texture.Height, 0f);
//		}

//		protected override void DrawSelf(SpriteBatch spriteBatch)
//		{
//			CalculatedStyle dimensions = base.GetDimensions();
//			if (AlignType == UIAlignType.AlignCenter)
//			{
//				spriteBatch.Draw(this._texture, dimensions.Center(), null, Color.White, 0f, _texture.Size() * 0.5f, this.ImageScale, SpriteEffects.None, 0f);
//			}
//			else
//			{
//				spriteBatch.Draw(this._texture, dimensions.Position(), null, Color.White, 0f, Vector2.Zero, this.ImageScale, SpriteEffects.None, 0f);
//			}
//		}
//	}
//}
