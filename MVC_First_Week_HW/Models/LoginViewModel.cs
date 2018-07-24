using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MVC_First_Week_HW.Models
{
    public class LoginViewModel
    {
        private static string desc_key =  "77887788";
        private static string desc_iv = "12345622";
        public static string Encrypt(string source)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(desc_key);
            byte[] iv = Encoding.ASCII.GetBytes(desc_iv);
            byte[] dataByteArray = Encoding.UTF8.GetBytes(source);
            des.Key = key;
            des.IV = iv;
            string encrypt = "";
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                encrypt = Convert.ToBase64String(ms.ToArray());
            }
            return encrypt;
        }

        public static string Decrypt(string source)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(desc_key);
            byte[] iv = Encoding.ASCII.GetBytes(desc_iv);
            des.Key = key;
            des.IV = iv;

            byte[] dataByteArray = Convert.FromBase64String(source);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}