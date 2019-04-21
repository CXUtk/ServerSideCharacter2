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
	public class GetUnionComplexHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var splayer = Main.player[playerNumber].GetServerPlayer();
				if(splayer.Union == null)
				{
					splayer.SendMessageBox("你没有在任何一个公会中", 180, Color.OrangeRed);
					// 告诉客户端不在公会中
					splayer.SyncUnionInfo();
					return;
				}
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.UnionInfoComplex);
				p.Write(JsonConvert.SerializeObject(splayer.Union.GetComplex(playerNumber), Formatting.None));
				p.Send(playerNumber);
				CommandBoardcast.ConsoleMessage($"{splayer.Union.Name} 公会信息已经发送给{Main.player[playerNumber].name}");
			}
			else
			{
				var data = reader.ReadString();
				var info = JsonConvert.DeserializeObject<ComplexUnionInfo>(data);
				lock (UnionPageState2.Instance)
				{
					UnionPageState2.Instance.ClearMembers();
					UnionPageState2.Instance.Apply(info);
					UnionCandidatePage.Instance.AppendCandidates(info);
				}
			}
		}
	}
}
