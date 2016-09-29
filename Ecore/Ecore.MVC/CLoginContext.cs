using Ecore.Frame;
using Ecore.Frame.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.MVC
{
    public class CLoginContext : ILoginContext
    {
        public CLoginContext()
        {
        }



        public User GetUser()
        {
            string userid = Cookie.Default.GetCookieValue("login-uid");
            if (!string.IsNullOrEmpty(userid))
            {
                return null;
            }
            userid = AESHelper.AESDecrypt(MyEncoding.UrlDecode(userid));

            User user = Cache.Default.Get<User>(userid);
            if (user == null)
            {
                user = GetUserById(userid);
            }

            return user;
        }

        public virtual User GetUserByUnique(string unique)
        {
            throw new NotImplementedException();
        }

        public virtual User GetUserById(string Id)
        {
            throw new NotImplementedException();
        }

        public Visitor GetVisitor()
        {
            string uuid = Cookie.Default.GetCookieValue("uuid");
            if (string.IsNullOrEmpty(uuid))
            {
                uuid = IDGenerator.Default.NewID();
                Cookie.Default.SetCookie("uuid", uuid);
            }
            return new Visitor()
            {
                UUID = uuid
            };

        }

        public bool IsLogin()
        {
            var user = GetUser();
            if (string.IsNullOrEmpty(user.UserId))
            {
                return false;
            }
            return true;
        }

        public virtual LoginResult Login(string unique, string password, string group = "")
        {
            throw new NotImplementedException();
        }

        public void LoginOut()
        {
            var user = GetUser();
            if (user != null)
            {
                Cache.Default.Remove("LoginContext.User." + user.UserId);
                Cookie.Default.RemoveCookie("login-uid");
            }

        }


        public void LoginSucceedHandle(User user)
        {
            //登录成功
            user.LastLoginTime = user.LoginTime;
            user.LoginTime = DateTime.Now;
            //写入缓存
            Cache.Default.Add("LoginContext.User." + user.UserId, user);
            //写cookie
            Cookie.Default.SetCookie("login-uid", MyEncoding.UrlEncode(AESHelper.AESEncrypt(user.UserId)));
        }
    }
}
