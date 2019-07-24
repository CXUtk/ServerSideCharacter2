using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;

namespace ServerSideCharacter2
{
    public class MachineCodeManager
    {
        ///// <summary>
        ///// 获取当前激活网络的MAC地址、IPv4地址、IPv6地址 - 方法2
        ///// </summary>
        ///// <param name="mac">网卡物理地址</param>
        ///// <param name="ipv4">IPv4地址</param>
        //public static string GetMac()
        //{
        //	StringBuilder sb = new StringBuilder();
        //	string mac = "";
        //	string ipv4 = "";
        //	string ipv6 = "";

        //	NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        //	foreach (NetworkInterface adapter in nics)
        //	{
        //		IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
        //		UnicastIPAddressInformationCollection allAddress = adapterProperties.UnicastAddresses;
        //		if (allAddress.Count > 0)
        //		{
        //			if (adapter.OperationalStatus == OperationalStatus.Up)
        //			{
        //				mac = adapter.GetPhysicalAddress().ToString();
        //				foreach (UnicastIPAddressInformation addr in allAddress)
        //				{
        //					if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        //					{
        //						ipv4 = addr.Address.ToString();
        //					}
        //					if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
        //					{
        //						ipv6 = addr.Address.ToString();
        //					}
        //				}
        //				if (string.IsNullOrWhiteSpace(mac) ||
        //					(string.IsNullOrWhiteSpace(ipv4) && string.IsNullOrWhiteSpace(ipv6)))
        //				{
        //					mac = "";
        //					ipv4 = "";
        //					ipv6 = "";
        //					continue;
        //				}
        //				sb.Append(mac);
        //			}
        //		}
        //	}
        //	return sb.ToString();
        //}

        /// <summary>
        /// 获取字符串MD5
        /// </summary>
        /// <param name="input">待处理字符串</param>
        /// <returns> string </returns>
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

        ///// <summary>
        ///// 获取机器码
        ///// </summary>
        ///// <returns>string</returns>
        //public static string GetMachineCode()
        //{
        //    string mac = GetMac();
        //    return GetMD5WithString(mac);
        //}

        ///   <summary> 
        ///   获取cpu序列号     
        ///   </summary> 
        ///   <returns> string </returns> 
        public static string GetCpuInfo()
        {
            string cpuInfo = "";
            try
            {
                using (ManagementClass cimobject = new ManagementClass("Win32_Processor"))
                {
                    ManagementObjectCollection moc = cimobject.GetInstances();

                    foreach (ManagementObject mo in moc)
                    {
                        cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return cpuInfo.ToString();
        }

        ///   <summary> 
        ///   获取硬盘ID     
        ///   </summary> 
        ///   <returns> string </returns> 
        public static string GetHDid()
        {
            string HDid = "";
            try
            {
                using (ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive"))
                {
                    ManagementObjectCollection moc1 = cimobject1.GetInstances();
                    foreach (ManagementObject mo in moc1)
                    {
                        HDid = (string)mo.Properties["Model"].Value;
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return HDid.ToString();
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources")) return null;
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager("ServerSideCharacter2" + ".Resources", System.Reflection.Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllName);
            return System.Reflection.Assembly.Load(bytes);
        }
        /// <summary>
        /// 获取机器码
        /// </summary>
        /// <returns> string </returns>
        public static string GetMachineCode()
        {
            string machineCode = "";
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                machineCode = GetCpuInfo() + GetHDid();
            }
            catch
            {
                return "";
            }
            
            return GetMD5WithString(machineCode);
        }

    }
}
