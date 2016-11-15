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

        Language(Culture cultureInfo = Culture.zh_cn)
        {
            ((ILanguage)this).CurrentCultureInfo = cultureInfo;

        }

        Culture ILanguage.CurrentCultureInfo { get; set; }

        string ILanguage.Get(string keyLang)
        {
            List<LangItem> list = Config.Default.GetConfigFile<List<LangItem>>("langSource");

            LangItem item = list.Where(q => q.zh_cn.ToLower() == keyLang.ToLower()).FirstOrDefault();
            if (item == null)
            {
                return keyLang;
            }

            var dict = item.ToDictList().Where(q => q.Key == ((ILanguage)this).CurrentCultureInfo.ToString()).FirstOrDefault();
            return dict.Value;

        }

        string ILanguage.GetScript()
        {
            List<LangItem> list = Config.Default.GetConfigFile<List<LangItem>>("langSource");

            List<LangScriptItem> langScriptList = new List<LangScriptItem>();

            string result = "";

            foreach (var item in list)
            {

                result = result + "\"" + item.zh_cn.Replace("\"", "") + "\"";
                result = result + ":";
                result = result + "\"" + item.ToDictList().Where(q => q.Key == ((ILanguage)this).CurrentCultureInfo.ToString()).FirstOrDefault().Value.Replace("\"", "") + "\"";
                result = result + ",";

                //langScriptList.Add(new LangScriptItem()
                //{
                //    Key = item.zh_cn,
                //    Value = item.ToDictList().Where(q => q.Key == ((ILanguage)this).CurrentCultureInfo.ToString()).FirstOrDefault().Value,
                //});
            }

            result = "{" + result.Trim(',') + "}";

            return result;

        }
    }
    public class LangScriptItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public enum Culture
    {
        zh_cn,
        en_us,
        ja_jp,
        ko_kr

    }

    public interface ILanguage
    {
        Culture CurrentCultureInfo { get; set; }

        string Get(string keyLang);

        string GetScript();
    }

    public class LangItem
    {
        public string zh_cn { get; set; }
        public string en_us { get; set; }
        public string ja_jp { get; set; }
        public string ko_kr { get; set; }

        public List<KeyValuePair<string, string>> ToDictList()
        {
            List<KeyValuePair<string, string>> dl = new List<KeyValuePair<string, string>>();

            dl.Add(new KeyValuePair<string, string>("zh_cn", zh_cn));
            dl.Add(new KeyValuePair<string, string>("en_us", en_us));
            dl.Add(new KeyValuePair<string, string>("ja_jp", ja_jp));
            dl.Add(new KeyValuePair<string, string>("ko_kr", ko_kr));

            return dl;

        }
    }

}
