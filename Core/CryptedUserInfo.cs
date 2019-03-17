using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ServerSideCharacter2.Core
{
	public class CryptedUserInfo
	{
		public string UserName { get; set; }
		public string Password { get; set; }

		public CryptedUserInfo(string username, string password)
		{
			UserName = username;
			using (MD5 md5Hash = MD5.Create())
			{
				byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
				StringBuilder sBuilder = new StringBuilder();
				for (int i = 0; i < data.Length; i++)
				{
					sBuilder.Append(data[i].ToString("x2"));
				}
				Password = sBuilder.ToString();
			}
		}
	}
}
