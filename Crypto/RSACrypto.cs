using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using ServerSideCharacter2.Utils;
using System.IO;
using Terraria;

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
			using (var rsa = new RSACryptoServiceProvider())
			{
				publicKey = rsa.ToXmlString(false);
				privateKey = rsa.ToXmlString(true);
				isSet = true;
			}
			using (var sw = new StreamWriter("SSC/publickey.xml"))
			{
				sw.Write(publicKey);
			}
			using (var sw = new StreamWriter("SSC/privateKey.xml"))
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
				return RsaEncrypt(data, publicKey);
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
				return null;
			}
		}

		public static string EncryptWithTag(string data, string tag)
		{
			if (!isSet) throw new SSCException("Public Key isn't available!");

			try
			{
				return RsaEncrypt(data + tag, publicKey);
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
				return null;
			}
		}

		public static bool DecryptWithTag(string data, string tag, out string result)
		{
			if (!isSet) throw new SSCException("Public Key isn't available!");

			try
			{
				string s = RsaDecrypt(data, privateKey);
				if (!s.EndsWith(tag))
				{
					result = "";
					return false;
				}
				else
				{
					result = s.Substring(0, s.Length - tag.Length);
					return true;
				}
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
				result = "";
				return false;
			}
		}

		public static string RsaEncrypt(string rawInput, string publicKey)
		{
			using (var rsaProvider = new RSACryptoServiceProvider())
			{
				var inputBytes = Encoding.UTF8.GetBytes(rawInput);
				rsaProvider.FromXmlString(publicKey);
				//单块最大长度
				int bufferSize = (rsaProvider.KeySize / 8) - 11;
				var buffer = new byte[bufferSize];
				using (MemoryStream inputStream = new MemoryStream(inputBytes),
					 outputStream = new MemoryStream())
				{
					while (true)
					{
						int readSize = inputStream.Read(buffer, 0, bufferSize);
						if (readSize <= 0)
						{
							break;
						}

						var temp = new byte[readSize];
						Array.Copy(buffer, 0, temp, 0, readSize);
						var encryptedBytes = rsaProvider.Encrypt(temp, false);
						outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
					}
					return Convert.ToBase64String(outputStream.ToArray());
				}
			}
		}

		public static string Decrypt(string data)
		{
			try
			{
				return RsaDecrypt(data, privateKey);
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
				return null;
			}
		}


		public static string RsaDecrypt(string encryptedInput, string privateKey)
		{
			using (var rsaProvider = new RSACryptoServiceProvider())
			{
				var inputBytes = Convert.FromBase64String(encryptedInput);
				rsaProvider.FromXmlString(privateKey);
				//单块最大长度
				int bufferSize = rsaProvider.KeySize / 8;
				var buffer = new byte[bufferSize];
				using (MemoryStream inputStream = new MemoryStream(inputBytes),
					 outputStream = new MemoryStream())
				{
					while (true)
					{
						int readSize = inputStream.Read(buffer, 0, bufferSize);
						if (readSize <= 0)
						{
							break;
						}

						var temp = new byte[readSize];
						Array.Copy(buffer, 0, temp, 0, readSize);
						var rawBytes = rsaProvider.Decrypt(temp, false);
						outputStream.Write(rawBytes, 0, rawBytes.Length);
					}
					return Encoding.UTF8.GetString(outputStream.ToArray());
				}
			}
		}
	}
}
