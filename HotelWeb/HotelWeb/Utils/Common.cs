using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HotelWeb.Utils
{
    public static class Common
    {
        public static string CalculateMD5Hash(string input)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            inputBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in inputBytes)
            {
                sb.Append(b.ToString("x2").ToLower());
            }

            return sb.ToString();
        }





    }
}