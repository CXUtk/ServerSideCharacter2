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

		private List<string> _messageLine = new List<string>();

		private const int FADEIN_TIME = 20;
		private const int FADEOUT_TIME = 20;
		private const float MSG_MAX_WIDTH = 350;
		private const float MSG_INIT_HEIGHT = 70;
		private const float MSG_PADDING_TOP = 20;
		private const float MSG_PADDING_LEFT = 30;
		private const float MSG_LINE_MARGIN = 25;

		public void ShowMessage(string msg, int time, Color color)
		{
			_animationMaxTime = time + FADEIN_TIME + FADEOUT_TIME;
			_messageTimer = _animationMaxTime;
			_message = msg;
			_color = color;
			_messageLine.Clear();
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < _message.Length; i++)
			{
				sb.Append(_message[i]);
				string str = sb.ToString();
				if (Main.fontMouseText.MeasureString(str).X >= MSG_MAX_WIDTH - MSG_PADDING_LEFT * 2)
				{
					_messageLine.Add(str);
					sb.Clear();
				}
			}
			if(sb.Length > 0)
			{
				_messageLine.Add(sb.ToString());
			}
			Main.NewText(_messageLine.Count);
			for (int i = 0; i < _messageLine.Count; i++)
			{
				Main.NewText(_messageLine[i]);
			}
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
					_messageWidth = MathHelper.Lerp(0, MSG_MAX_WIDTH, factor);
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			if (_messageTimer <= 0) return;
			float realHeight = MSG_INIT_HEIGHT + (_messageLine.Count - 1) * MSG_LINE_MARGIN;
			Vector2 topLeft = new Vector2(Main.screenWidth / 2 - _messageWidth / 2, Main.screenHeight / 2 - realHeight / 2);
			Drawing.DrawAdvBox(sb, new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)_messageWidth, (int)realHeight), Drawing.DefaultBoxColor,
				ServerSideCharacter2.ModTexturesTable["Box"], new Vector2(12, 12));
			if (_messageTimer < _animationMaxTime - FADEIN_TIME && _messageTimer > FADEOUT_TIME)
			{
				for (int i = 0; i < _messageLine.Count; i++)
				{
					float h = (_messageLine.Count / 2) * MSG_LINE_MARGIN;
					Vector2 drawPos = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 + 4 - h / 2 + i * MSG_LINE_MARGIN);
					Vector2 texSize = Main.fontMouseText.MeasureString(_messageLine[i]);
					Terraria.Utils.DrawBorderStringFourWay(sb, Main.fontMouseText, _messageLine[i], drawPos.X,
						drawPos.Y, _color, Color.Black, texSize * 0.5f, 1f);
				}
			}
		}
	}
}
