using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Services.Mails
{
	public class RequestMailListHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 如果在服务器端
			if (Main.netMode == 2)
			{
				var player = Main.player[playerNumber].GetServerPlayer();
				MailsHeadInfo info = new MailsHeadInfo();
				foreach (var mail in player.MailList)
				{
					info.Mails.Add(mail.MailHead);
				}
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.MailGetHeads);
				p.Write(info.GetJson());
				p.Send(playerNumber);
				Utils.CommandBoardcast.ConsoleMessage("收到玩家请求查看邮件列表");
			}
			else
			{
				var str = reader.ReadString();
				MailsHeadInfo info = new MailsHeadInfo();
				info = JsonConvert.DeserializeObject<MailsHeadInfo>(str);
				// 同步到UI
			}
		}
	}
}
