using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backlog
{
    class MD5forPHP
    {
        public string ConvertToPHP(string textToHash)
        {
            UTF8Encoding encode = new UTF8Encoding();
            byte[] bytes = encode.GetBytes(textToHash);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashed = md5.ComputeHash(bytes);
            string result = "";

            for (int i = 0; i < hashed.Length; i++)
            {
                result += Convert.ToString(hashed[i], 16).PadLeft(2, '0');
            }
            return result.PadLeft(32, '0');
        }
    }
}
