using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Network
{
	public abstract class MessageDispatcher<T>
	{
		protected Dictionary<T, MessagePatchDelegate> _method;

		protected abstract void RegisterMethod();

		public MessageDispatcher()
		{
			RegisterMethod();
		}

		protected bool Dispatch(T msgType, BinaryReader reader, int playerNumber)
		{
			try
			{
				MessagePatchDelegate mpd;
				if (_method.TryGetValue(msgType, out mpd))
				{
					return mpd(ref reader, playerNumber);
				}
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
			return false;
		}
	}
}
