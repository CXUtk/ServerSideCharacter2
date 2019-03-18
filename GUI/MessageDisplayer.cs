using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using ServerSideCharacter2.GUI.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.GUI
{
	public class MessageDisplayer
	{
		private int _messageTimer;

		private int _animationMaxTime;

		private string _message;

		private float _messageWidth;

		private Color _color;

		private const int FADEIN_TIME = 20;
		private const int FADEOUT_TIME = 20;
		private const float MSG_MAX_WIDTH = 300;
		private const float MSG_MAX_HEIGHT = 50;

		public void ShowMessage(string msg, int time, Color color)
		{
			_animationMaxTime = time + FADEIN_TIME + FADEOUT_TIME;
			_messageTimer = _animationMaxTime;
			_message = msg;
			_color = color;
		}

		public MessageDisplayer()
		{

		}

		public void Update(GameTime gameTime)
		{
			if (_messageTimer > 0)
			{
				_messageTimer--;
				if (_messageTimer >= _animationMaxTime - FADEIN_TIME)
				{
					float factor = _animationMaxTime - _messageTimer;
					_messageWidth = MathHelper.Lerp(0, MSG_MAX_WIDTH, factor / FADEIN_TIME);
				}
				else if(_messageTimer <= FADEOUT_TIME)
				{
					float factor = _messageTimer / (float)FADEOUT_TIME;
					Main.NewText(factor);
					_messageWidth = MathHelper.Lerp(0, MSG_MAX_WIDTH, factor);
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			if (_messageTimer <= 0) return;
			Vector2 topLeft = new Vector2(Main.screenWidth / 2 - _messageWidth / 2, Main.screenHeight / 2 - MSG_MAX_HEIGHT / 2);
			Drawing.DrawAdvBox(sb, new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)_messageWidth, (int)MSG_MAX_HEIGHT), Drawing.DefaultBoxColor,
				ServerSideCharacter2.ModTexturesTable["Box"], new Vector2(12, 12));
			if (_messageTimer < _animationMaxTime - FADEIN_TIME && _messageTimer > FADEOUT_TIME)
			{
				Vector2 texSize = Main.fontMouseText.MeasureString(_message);
				Terraria.Utils.DrawBorderStringFourWay(sb, Main.fontMouseText, _message,
					Main.screenWidth / 2, Main.screenHeight / 2 + 4, _color, Color.Black, texSize * 0.5f, 1f);
			}
		}
	}
}
