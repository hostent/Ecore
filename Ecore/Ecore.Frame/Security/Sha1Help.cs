using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ecore.Frame.Security
{
    public class Sha1Help
    {
        /// <summary>
        /// Sha1 标准算法，uft8
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string Encrypt_Sha1(string strSource)
        {
            byte[] bytes_sha1_in = UTF8Encoding.UTF8.GetBytes(strSource);

            var provide = SHA1.Create();

            var outSha1 = provide.ComputeHash(bytes_sha1_in);

            return UTF8Encoding.UTF8.GetString(outSha1).Replace("-", "").ToLower();


        }


    }
}
