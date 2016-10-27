using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class Language : ILanguage
    {
        public static ILanguage GetCultureLang(Culture cultureInfo)
        {
            return new Language(cultureInfo);
        }

        public Language(Culture cultureInfo = Culture.zh_CN)
        {
            ((ILanguage)this).CurrentCultureInfo = cultureInfo;
 
        }

        Culture ILanguage.CurrentCultureInfo { get; set; }

        string ILanguage.Get(string keyLang)
        {
            var item = Config.Default.GetLangItem(keyLang);

            switch (((ILanguage)this).CurrentCultureInfo)
            {
                case Culture.zh_CN:
                    return item.zh_CN;
                case Culture.en:
                    return item.en;
                case Culture.ja_JP:
                    return item.ja_JP;
                case Culture.ko_KR:
                    return item.ko_KR;
            }
            return "";

        }

    }

    public enum Culture
    {
        zh_CN,
        en,
        ja_JP,
        ko_KR

    }

    public interface ILanguage
    {
        Culture CurrentCultureInfo { get; set; }

        string Get(string keyLang);
    }


}
