using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Mongodb
{
    #region Entity

    enum LogLevel
    {
        Release,
        Debug
    }

    class ErrorEntity : BaseEntity
    {
        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        public string Message { get; set; }
    }

    class MessageEntity : BaseEntity
    {

        public string Category { get; set; }

        public string Message { get; set; }

    }

    class MessageEntity<T> : BaseEntity
    {

        public string Category { get; set; }

        public string Message { get; set; }

        public T CustomerMsg { get; set; }

    }

    class BaseEntity
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserIP { get; set; }

        public DateTime Createtime { get; set; }

        public string PageUrl { get; set; }

        public string Browser { get; set; }

        public string AppPath { get; set; }

        public string MachineName { get; set; }

        public string ThreadName { get; set; }

        public string Track { get; set; }
    }

    #endregion
}
