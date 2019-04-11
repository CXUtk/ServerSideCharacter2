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
	public class PickMailSlotHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 如果在服务器端
			if (Main.netMode == 2)
			{
				var id = reader.ReadUInt64();
				var slotid = reader.ReadByte();
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
				target.AttachedItems[slotid] = new ItemInfo();
				return;
			}
		}
	}
}
