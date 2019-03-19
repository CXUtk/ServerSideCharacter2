using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Services
{
	/// <summary>
	/// 收到SSC消息时的处理方式
	/// </summary>
	public interface ISSCNetHandler
	{
		bool Handle(BinaryReader reader, int playerNumber);
	}
}
