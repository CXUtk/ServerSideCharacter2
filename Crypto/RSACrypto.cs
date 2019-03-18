using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using ServerSideCharacter2.Utils;
using System.IO;

namespace ServerSideCharacter2.Crypto
{
	public static class RSACrypto
	{
		private static string publicKey;
		private static string privateKey;

		private static bool isSet;
		public static string PublicKey
		{
			get
			{
				return publicKey;
			}
		}

		public static void GenKey()
		{
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
			{
				publicKey = rsa.ToXmlString(true);
				privateKey = rsa.ToXmlString(false);
				isSet = true;
			}
			using (StreamWriter sw = new StreamWriter("SSC/publickey.xml"))
			{
				sw.Write(publicKey);
			}
			using (StreamWriter sw = new StreamWriter("SSC/privateKey.xml"))
			{
				sw.Write(privateKey);
			}
		}

		public static void SetPublicKey(string pubkey)
		{
			publicKey = pubkey;
			isSet = true;
		}

		public static string Encrypt(string data)
		{
			if (!isSet) throw new SSCException("Public Key isn't available!");

			try
			{
				byte[] arr = Encoding.UTF8.GetBytes(data);
				byte[] encrypted;
				RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
				rsa.FromXmlString(publicKey);
				encrypted = rsa.Encrypt(arr, false);
				return Convert.ToBase64String(encrypted);
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
				return null;
			}
		}

		public static string Decrypt(string data)
		{
			try
			{
				byte[] arr = Convert.FromBase64String(data);
				byte[] decrypted;
				RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
				rsa.FromXmlString(publicKey);
				decrypted = rsa.Decrypt(arr, false);
				return Encoding.UTF8.GetString(decrypted);
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
				return null;
			}
		}
	}
}
