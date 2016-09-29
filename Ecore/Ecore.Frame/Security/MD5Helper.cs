using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecore.Frame.Security
{
    public class MD5Helper
    {
        /// <summary>
        /// 标准MD5
        /// </summary>
        /// <param name="AppKey"></param>
        /// <returns></returns>
        public static string Encrypt_MD5(string strSource)
        {

            var md5 = System.Security.Cryptography.MD5.Create();

            //注意编码UTF8、UTF7、Unicode等的选择　
             byte[] newSource = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strSource));
       
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < newSource.Length; i++)
            {
                sb.Append(newSource[i].ToString("x").PadLeft(2, '0'));
            }
            string crypt = sb.ToString();
            return crypt;
        }
    }
}
