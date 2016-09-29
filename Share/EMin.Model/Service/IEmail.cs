namespace EMin.Model.Service
{
    /// <summary>
    /// 邮件发送接口
    /// </summary>
    public interface IEmail
    {
        /// <summary>
        /// 发送邮件（异步）
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <param name="title">邮件标题</param>
        /// <param name="templateCode">内容模板编码</param>
        /// <param name="args">模板参数</param>
        void SendAsyn(string email, string title, string templateCode, params object[] args);
    }
}
