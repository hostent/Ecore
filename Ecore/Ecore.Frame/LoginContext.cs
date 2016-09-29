using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class LoginContext
    {
        public static ILoginContext Default { get; set; }


    }

    public interface ILoginContext : IUserInfo
    {
        LoginResult Login(string unique, string password, string group = "");

        void LoginSucceedHandle(User user);

        /// <summary>
        /// 当前用户退出
        /// </summary>
        void LoginOut();

        /// <summary>
        ///  获取当前访客信息
        /// </summary>
        /// <returns></returns>
        Visitor GetVisitor();

        bool IsLogin();
    }

    public interface IUserInfo
    {
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        User GetUser();

        /// <summary>
        /// 根据唯一信息(用户名称，电话号码)，获取用户信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unique"></param>
        /// <returns></returns>
        User GetUserByUnique(string unique);

        User GetUserById(string Id);
    }

    public class Visitor
    {
        public string UUID { get; set; }


    }

    /// <summary>
    /// Visitor 有可能多个，这里取最后一个Visitor。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class User : Visitor
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Group { get; set; }

        public DateTime LastLoginTime { get; set; }

        public DateTime LoginTime { get; set; }

        public object UserInfo { get; set; }
 
    }

    public class LoginResult
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public object Info { get; set; }


        public const string SucceedCode = "OK";
        public const string SucceedMessage = "";

        public const string FailCode = "Fail";
        public const string FailMessage = "登录失败";

        public static LoginResult Succeed = new LoginResult()
        {
            Code = SucceedCode,
            Message = SucceedMessage

        };

        public static LoginResult Fail = new LoginResult()
        {
            Code = FailCode,
            Message = FailMessage

        };
    }
}
