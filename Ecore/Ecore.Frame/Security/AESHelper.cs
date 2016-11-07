using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ecore.Frame.Security
{
    public class AESHelper
    {

        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string Key
        {
            get
            {
                return "amoyx.com.568333";    
            }
        }
        //默认密钥向量 
        private static byte[] _key1 = Encoding.UTF8.GetBytes(""); //"www.amoyx.com.cn" 默认空
        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <returns>将加密后的密文转换为Base64编码，以便显示</returns>
        public static string AESEncrypt(string plainText, string key="")
        {
            if(key== "")
            {
                key = Key;
            }
            try
            {
                byte[] keyArray = Convert.FromBase64String(key);
                byte[] ivArray = _key1;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(plainText);

                var aes = Aes.Create();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.Zeros;

                byte[] resultArray = aes.CreateEncryptor(keyArray, ivArray).TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);

            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文字符串</param>
        /// <returns>返回解密后的明文字符串</returns>
        public static string AESDecrypt(string showText, string key = "")
        {
            if (key == "")
            {
                key = Key;
            }
            try
            {
                byte[] keyArray = Convert.FromBase64String(key);
                byte[] ivArray = _key1;
                byte[] toEncryptArray = Convert.FromBase64String(showText);


                var aes = Aes.Create();

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.Zeros;

                byte[] resultArray = aes.CreateDecryptor(keyArray, ivArray).TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return UTF8Encoding.UTF8.GetString(resultArray).Replace("\0", "");




            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
