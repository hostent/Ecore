namespace EMin.Model.Service
{
    /// <summary>
    /// 短信发送接口
    /// </summary>
    public interface ISms
    {
        /// <summary>
        /// 发送短信（异步）
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="templateCode">内容模板编码</param>
        /// <param name="args">模板参数</param>
        void SendAsyn(string phoneNumber, string templateCode, params object[] args);
    }
}
