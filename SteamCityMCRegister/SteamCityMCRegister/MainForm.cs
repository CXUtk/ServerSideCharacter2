using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace SteamCityMCRegister
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private string MachineCode = "";
        private static string GetMD5WithString(string input)
        {
            System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();
            // 将输入字符串转换为字节数组并计算哈希数据  
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // 创建一个 Stringbuilder 来收集字节并创建字符串  
            StringBuilder str = new StringBuilder();
            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
            for (int i = 0; i < data.Length; i++)
            {
                str.Append(data[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位
            }
            // 返回十六进制字符串  
            return str.ToString();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            MachineCode = GetMachineCode();
            textBox_MachineCode.Text = MachineCode;
        }

        private string GetMachineCode()
        {
            string CPU = GetCpuInfo();
            string HD = GetHDid();
            if (CPU != "" && HD != "")
            { return "CPU" + CPU + "HD" + HD; }
            else
            { return ""; }
        }

        ///   <summary> 
        ///   获取cpu序列号     
        ///   </summary> 
        ///   <returns> string </returns> 
        private string GetCpuInfo()
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
            catch
            {
                return "";
            }
            return cpuInfo.ToString();
        }

        ///   <summary> 
        ///   获取硬盘ID     
        ///   </summary> 
        ///   <returns> string </returns> 
        private string GetHDid()
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
            catch
            {
                return "";
            }
            return HDid.ToString();
        }

        private void button_Register_Click(object sender, EventArgs e)
        {
            if (MachineCode == "")
            { MessageBox.Show("机器码获取失败！"); }
            else
            {
                try
                {
                    RegistryKey key = Registry.LocalMachine;
                    RegistryKey software = key.CreateSubKey("software\\SteamCity");
                    software = key.OpenSubKey("software\\SteamCity", true);
                    software.SetValue("MachineCode", MachineCode);
                    software.SetValue("Check", GetMD5WithString(MachineCode + "This is the MACHINE CODE key."));
                    key.Close();
                    MessageBox.Show("注册成功！");
                }
                catch
                { MessageBox.Show("注册失败！请以管理员身份运行。"); }
            }
        }

        private void button_RefreshMC_Click(object sender, EventArgs e)
        {
            MachineCode = GetMachineCode();
            textBox_MachineCode.Text = MachineCode;
        }
    }
}
