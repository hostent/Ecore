using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ecore.Frame.Security
{
    public class StringFomat
    {
        /// <summary>
        /// 数字id 变成字符串
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static String BlurIdentify(Object identify, String flag = "et")
        {
            if (identify == null) throw new ArgumentNullException();
            if (String.IsNullOrWhiteSpace(identify.ToString()))
                throw new ArgumentException("Identify value is empty string.");

            identify = identify.ToString().Replace("-", String.Empty).Replace(",", String.Empty).Replace(".", String.Empty);

            Int32 convertedIdentify;
            String blurredString = String.Empty;
            var random = new Random(DateTime.Now.Millisecond);

            if (Int32.TryParse(identify.ToString(), out convertedIdentify))
            {
                blurredString += flag;

                foreach (char item in convertedIdentify.ToString(CultureInfo.InvariantCulture))
                {
                    blurredString += char.ConvertFromUtf32(97 + 7 + Int32.Parse(item.ToString()));
                    blurredString += item.ToString();
                }

                return blurredString;
            }

            throw new InvalidCastException("Identify cannot cast to blurred text.");
        }


        /// <summary>
        /// 字符串解出数字id
        /// </summary>
        /// <param name="blurredIdentify"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static Int32 UnblurIdentify(Object blurredIdentify, String flag = "et")
        {
            if (blurredIdentify == null) throw new ArgumentNullException();
            if (String.IsNullOrWhiteSpace(blurredIdentify.ToString()))
                throw new ArgumentException("Blurred identify value is empty string.");

            String tempString = String.Empty;

            foreach (char item in blurredIdentify.ToString())
                if (item >= 48 && item <= 57) tempString += item.ToString();

            Int32 result;
            if (Int32.TryParse(tempString, out result)) return result;



            return -1;

            
        }


        /// <summary>  
        /// 字符串转为UniCode码字符串  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        public static string StringToUnicode(string s)
        {
            char[] charbuffers = s.ToCharArray();
            byte[] buffer;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charbuffers.Length; i++)
            {
                buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                sb.Append(String.Format("//u{0:X2}{1:X2}", buffer[1], buffer[0]));
            }
            return sb.ToString();
        }

        public static string UnicodeToString(string str)
        {
            Regex reg = new Regex("&#([^&#;][0-9]*);");

            var ml = reg.Matches(str);

            foreach (Match item in ml)
            {
                str = str.Replace(item.Value, ((char)Convert.ToInt32(item.Value.Replace("&#", "").Replace(";", ""))).ToString());
            }

            return str;


            

        }



    }
}
