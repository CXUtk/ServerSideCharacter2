using Newtonsoft.Json;
using ServerSideCharacter2.Mailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.JsonData
{
	public class MailsHeadInfo
	{
		public List<MailHead> Mails;
		public MailsHeadInfo()
		{
			Mails = new List<MailHead>();
		}
		public string GetJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.None);
		}
	}
}
