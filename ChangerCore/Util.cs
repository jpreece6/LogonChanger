using System;
using System.Security.Cryptography;
using System.Text;

namespace ChangerCore
{
    class Util
    {
        public static string GetMd5Hash(string strInput)
        {
            var md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(strInput));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            foreach (byte bit in data)
            {
                sBuilder.Append(bit.ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string GenerateFileTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMdd-HHmm");
        }
    }
}
