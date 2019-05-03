using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.GUI.UI;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Mailing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Services.Mails
{
	public class RequestMailContentHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 如果在服务器端
			if (Main.netMode == 2)
			{
				var id = reader.ReadUInt64();
				var player = Main.player[playerNumber].GetServerPlayer();
				var mailist = player.MailList;
				Mail target = null;
				foreach (var mail in mailist)
				{
					if (mail.MailHead.MailID == id)
					{
						target = mail;
						break;
					}
				}
				if (target == null)
				{
					player.SendMessageBox("这个邮件不存在", 180, Color.Yellow);
					return;
				}
				if (!target.MailHead.IsRead)
				{
					target.MailHead.IsRead = true;
					player.GiveGuCoin(target.AttachedGuCoin);
				}
				ModPacket p = ServerSideCharacter2.Instance.GetPacket();
				p.Write((int)SSCMessageType.MailGetContent);
				p.Write(target.Content);
				p.Write(JsonConvert.SerializeObject(target.AttachedItems));
				p.Send(playerNumber);
			}
			else
			{
				var content = reader.ReadString();
				var items = reader.ReadString();
				List<ItemInfo> itemInfos = new List<ItemInfo>();
				var itemlist = JsonConvert.DeserializeObject<List<ItemInfo>>(items);

				// 同步到UI
				lock (MailPageState.Instance)
				{
					MailPageState.Instance.SetContent(content, itemlist);
				}
			}
		}
	}
}
