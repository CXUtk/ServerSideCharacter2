using Newtonsoft.Json;
using ServerSideCharacter2.Crypto;
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

		public static CryptedUserInfo Create(string username, string password)
		{
			var info = new CryptedUserInfo
			{
				UserName = username
			};
			using (var md5Hash = MD5.Create())
			{
				var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
				var sBuilder = new StringBuilder();
				for (var i = 0; i < data.Length; i++)
				{
					sBuilder.Append(data[i].ToString("x2"));
				}
				info.Password = sBuilder.ToString();
			}
			return info;
		}

		private string GetJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public string GetEncryptedData()
		{
			return RSACrypto.Encrypt(GetJson());
		}

		public static CryptedUserInfo GetDecrypted(string encrypted)
		{
			return JsonConvert.DeserializeObject<CryptedUserInfo>(RSACrypto.Decrypt(encrypted));
		}

		public override string ToString()
		{
			return string.Format("Username: {0}, Password: {1}", UserName, Password);
		}
	}
}
