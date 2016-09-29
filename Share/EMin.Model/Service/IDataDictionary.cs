using System.Collections.Generic;

namespace EMin.Model.Service
{
    /// <summary>
    /// 数据字典接口
    /// </summary>
    public interface IDataDictionary
    {
        /// <summary>
        /// 获取数据字典数据值
        /// </summary>
        /// <param name="Type">数据类型</param>
        /// <param name="Text">显示文本</param>
        /// <returns></returns>
        string Get_DataDictionaryValue(string Type, string Text);

        /// <summary>
        /// 获取数据字典显示文本
        /// </summary>
        /// <param name="Type">数据类型</param>
        /// <param name="Value">数据值</param>
        /// <returns></returns>
        string Get_DataDictionaryText(string Type, string Value);

        /// <summary>
        /// 获取数据字典信息
        /// </summary>
        /// <param name="Type">数据类型</param>
        /// <returns></returns>
        Dictionary<string, string> Get_DataDictionary(string Type);
    }
}
