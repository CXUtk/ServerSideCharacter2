using ServerSideCharacter2.JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Mailing
{
	public class Mail : IComparable
	{
		public MailHead MailHead { get; set; }
		public string Content { get; set; }
		public List<ItemInfo> AttachedItems { get; set; }

		public Mail()
		{
			Content = "";
			AttachedItems = new List<ItemInfo>();
		}

		public int CompareTo(object obj)
		{
			var other = (Mail)obj;
			return MailHead.SendTime.CompareTo(other.MailHead.SendTime);
		}
	}
}
