using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2
{
    public class MachineCodeManager
    {
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
            string filepath = System.Environment.CurrentDirectory + "\\License.lic";
            if (System.IO.File.Exists(filepath))
            {
                string license = System.IO.File.ReadAllText(filepath);
                int sub = license.IndexOf("<Next>");
                string machinecode = license.Substring(0, sub);
                string check = license.Substring(sub + 6);
                if (check == GetMD5WithString(machinecode + "This is the MACHINE CODE key."))
                { return check; }
                else
                {
                    File.Delete(filepath);
                    return "MD5ERROR";
                }
            }
            else
            { return "FILENOTFOUND"; }

        }

    }
}
