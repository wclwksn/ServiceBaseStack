using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Base.ST.Tool
{
    public class BaseTools
    {
        public static string Encrypt(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            byte[] output = md5.ComputeHash(bytes);

            return BitConverter.ToString(output);
        }
    }
}