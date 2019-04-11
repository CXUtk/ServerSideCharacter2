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


		public static MailHead GenerateHead(string sender, string recevier)
		{
			MailHead head = new MailHead
			{
				MailID = ServerSideCharacter2.MailManager.MainCurrentID++
			};
			return head;
		}

		internal MailHead()
		{
			Sender = "<系统>";
			Recevier = "";
			SendTime = DateTime.Now;
			IsRead = false;
		}


	}
}
