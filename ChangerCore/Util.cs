using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace ChangerCore
{
    class Util
    {
        /// <summary>
        /// Generates a MD5 hash based on the given input
        /// </summary>
        /// <param name="strInput">Input to hash</param>
        /// <returns>hash of input string</returns>
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

        /// <summary>
        /// Creates a timestamp
        /// </summary>
        /// <returns>timestamp as string</returns>
        public static string GenerateFileTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMdd-HHmm");
        }

        /// <summary>
        /// Checks the compatibility of the current environment only runs on WIN10
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows10()
        {
            return (Environment.OSVersion.Version.Major >= 10);
        }
    }
}
