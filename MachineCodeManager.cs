using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace ServerSideCharacter2
{
    public class MachineCodeManager
    {
		/// <summary>
		/// 获取当前激活网络的MAC地址、IPv4地址、IPv6地址 - 方法2
		/// </summary>
		/// <param name="mac">网卡物理地址</param>
		/// <param name="ipv4">IPv4地址</param>
		public static void GetActiveIpAndMac2(out string mac)
		{

			mac = "";
			string ipv4 = "";
			string ipv6 = "";

			NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface adapter in nics)
			{
				IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
				UnicastIPAddressInformationCollection allAddress = adapterProperties.UnicastAddresses;
				if (allAddress.Count > 0)
				{
					if (adapter.OperationalStatus == OperationalStatus.Up)
					{
						mac = adapter.GetPhysicalAddress().ToString();
						foreach (UnicastIPAddressInformation addr in allAddress)
						{
							if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
							{
								ipv4 = addr.Address.ToString();
							}
							if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
							{
								ipv6 = addr.Address.ToString();
							}
						}

						if (string.IsNullOrWhiteSpace(mac) ||
							(string.IsNullOrWhiteSpace(ipv4) && string.IsNullOrWhiteSpace(ipv6)))
						{
							mac = "";
							ipv4 = "";
							ipv6 = "";
							continue;
						}
						else
						{
							if (mac.Length == 12)
							{
								mac = string.Format("{0}-{1}-{2}-{3}-{4}-{5}",
									mac.Substring(0, 2), mac.Substring(2, 2), mac.Substring(4, 2),
									mac.Substring(6, 2), mac.Substring(8, 2), mac.Substring(10, 2));
							}
							break;
						}
					}
				}
			}
		}

		private static string GetMD5WithString(string input)
        {
            System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                str.Append(data[i].ToString("x2"));
            }
            return str.ToString();
        }

        public static string GetMachineCode()
        {
			string mac;
			GetActiveIpAndMac2(out mac);
			return mac;
		}

    }
}
