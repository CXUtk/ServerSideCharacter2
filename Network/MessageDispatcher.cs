using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Network
{
	public delegate bool MessagePatchDelegate(ref BinaryReader reader, int playerNumber);

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
				MessagePatchDelegate method;
				if (_method.TryGetValue(msgType, out method))
				{
					return method(ref reader, playerNumber);
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
