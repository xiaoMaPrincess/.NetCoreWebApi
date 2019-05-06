using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Core.Common.Helper
{
    /// <summary>
    /// Md5
    /// </summary>
    public class MD5Helper
    {
        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5String(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] vs = md5.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            foreach (var item in vs)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
