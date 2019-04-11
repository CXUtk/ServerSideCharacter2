using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Mailing
{
	public class MailHead
	{
		public bool IsRead { get; set; }
		public ulong MailID { get; set; }
		public string Sender { get; set; }
		public string Recevier { get; set; }
		public DateTime SendTime { get; set; }
		public string Title { get; set; }


		public static MailHead GenerateHead(string title, string sender, string recevier)
		{
			MailHead head = new MailHead(title)
			{
				MailID = ServerSideCharacter2.MailManager.MainCurrentID++,
				Sender = sender,
				Recevier = recevier
			};
			return head;
		}

		public MailHead()
		{

		}

		public MailHead(string title)
		{
			Sender = "<系统>";
			Recevier = "";
			Title = title;
			SendTime = DateTime.Now;
			IsRead = false;
		}


	}
}
