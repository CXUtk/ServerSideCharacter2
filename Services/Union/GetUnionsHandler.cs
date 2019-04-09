using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.GUI.UI;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Services.Union
{
	public class GetUnionsHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.UnionsInfo);
				p.Write(JsonConvert.SerializeObject(ServerSideCharacter2.UnionManager.GetUnionsData(), Formatting.None));
				p.Send();
				CommandBoardcast.ConsoleMessage($"全局公会信息已经发送给{Main.player[playerNumber].name}");
			}
			else
			{
				var data = reader.ReadString();
				var info = JsonConvert.DeserializeObject<UnionInfo>(data);
				lock (UnionPageState.Instance)
				{
					UnionPageState.Instance.ClearUnions();
					UnionPageState.Instance.AppendUnions(info);
				}
			}
		}
	}
}
