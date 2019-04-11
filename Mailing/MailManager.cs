using Newtonsoft.Json;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Mailing
{
	public class MailsData
	{
		public ulong MainCurrentID = 0;
		public List<Mail> Mails = new List<Mail>();

		public void SaveTo(string filename)
		{
			var data = JsonConvert.SerializeObject(this, Formatting.None);
			using (var writer = new StreamWriter(filename, false, Encoding.UTF8))
			{
				writer.Write(data);
			}
		}
	}
	public class MailManager
	{
		public Dictionary<ulong, Mail> MailList { get; set; }
		internal ulong MainCurrentID;
		private const string FILENAME = "mails.json";
		
		public void Load()
		{
			try
			{
				if (!File.Exists(FILENAME))
				{
					CommandBoardcast.ConsoleMessage(GameLanguage.GetText("creatingMailsData"));
					MailsData mailsData = new MailsData();
					mailsData.SaveTo(FILENAME);
					return;
				}

				CommandBoardcast.ConsoleMessage(GameLanguage.GetText("readingMailsData"));
				string data;
				using (var reader = new StreamReader(FILENAME, Encoding.UTF8))
				{
					data = reader.ReadToEnd();
				}

				var list = JsonConvert.DeserializeObject<MailsData>(data);
				foreach(var mail in list.Mails)
				{
					MailList.Add(mail.MailHead.MailID, mail);
				}
				CommandBoardcast.ConsoleMessage(GameLanguage.GetText("finishReadPlayerDoc"));
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}

		public void Save()
		{
			MailsData data = new MailsData
			{
				MainCurrentID = MainCurrentID,
				Mails = MailList.Values.ToList()
			};
			
		}

		public MailManager()
		{
			MailList = new Dictionary<ulong, Mail>();
			Load();
		}

		public void ServerSendMail(ServerPlayer target, string title, string content)
		{
			Mail mail = new Mail
			{
				MailHead = MailHead.GenerateHead("<系统>", target.Name),
				Title = title,
				Content = content
			};
			MailList.Add(mail.MailHead.MailID, mail);
			target.MailList.Add(mail);
			if (target.MailList.Count > ServerSideCharacter2.Config.MaxMailsPerPlayer)
			{
				target.MailList.RemoveAt(0);
			}
		}
	}
}
