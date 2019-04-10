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
	public class UnionCandidateHandler : ISSCNetHandler
	{

		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var name = reader.ReadString();
				bool accept = reader.ReadBoolean();
				var player = Main.player[playerNumber].GetServerPlayer();
				var target = ServerSideCharacter2.PlayerCollection.Get(name);
				if(target == null)
				{
					player.SendErrorInfo("不存在这个玩家");
					return;
				}
				if (target.Union != null)
				{
					player.SendErrorInfo("该玩家已经有公会");
					return;
				}
				if (player.Union == null)
				{
					player.SendErrorInfo("不合法的操作");
					return;
				}
				if (player.Union.Owner != player.Name)
				{
					player.SendErrorInfo("你不是公会会长，不能进行操作");
					return;
				}
				var union = player.Union;
				if (accept)
				{
					if (!union.CanAccept())
					{
						player.SendErrorInfo("公会人数已经达到上限");
						return;
					}
					union.AcceptCandidate(target);
				}
				else
				{
					union.RejectCandidate(target);
				}
				CommandBoardcast.ConsoleMessage($"公会 {union.Name} 拒绝了 {target.Name} 的加入申请");
			}
		}

	}
}
