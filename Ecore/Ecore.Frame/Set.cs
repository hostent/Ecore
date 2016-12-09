﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecore.Frame
{

    public interface ISet<T> : IQuery<T>, ICommand<T>, IXmlQuery<T>, IXmlCommand<T> where T : class, new()
    {

    }

    public interface IQuery<T> where T : class, new()
    {
        IQuery<T> Where(Expression<Func<T, bool>> exp);

        IQuery<T> OrderBy(Expression<Func<T, object>> exp);

        IQuery<T> OrderByDesc(Expression<Func<T, object>> exp);

        IQuery<T> Limit(int form, int length);

        IQuery<T> Distinct();

        


        T First();

        R FirstAs<R>(Expression<Func<T, R>> singleSelector);

        R FirstAs<R>() where R : class, new();

        List<T> ToList();

        List<R> ToListAs<R>(Expression<Func<T, R>> singleSelector);

        List<R> ToListAs<R>() where R : class, new();

        long Count();

        bool Exist();

        T Get(object id);

        T GetUnique(string uniqueCode);

        // Group , avi, sum  统计类,用sql

    }

    public interface ICommand<T> where T : class, new()
    {
        object Add(T t);

        int Delete(object id);

        int Update(T t);
    }

    public interface IXmlCommand<T>
    {
        void ExecXml(string reportName, IDictionary<string, object> par);
    }

    public interface IXmlQuery<T> where T : class, new()
    {
        IList<T> QueryXml(string reportName, IDictionary<string, object> par);

        PageData<T> QueryXml(string reportName, PagePars param);
    }

    public interface IEntity
    {
        [Key]
        string Id { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CacheAttribute : Attribute
    {
        public int Second { get; set; }
    }

    public interface IEntityRecord
    {
        string CreateBy { get; set; }
        string UpdateBy { get; set; }
        DateTime? CreateDate { get; set; }
        DateTime? UpdateDate { get; set; }
    }

    public class PageData<T>
    {
        public int total;
        public List<T> rows;
        /// <summary>
        /// 页数(从1开始)
        /// </summary>
        public int current;
        /// <summary>
        /// 分页大小
        /// </summary>
        public int rowCount;

        public PageData()
        {
        }

        public PageData(int counts, List<T> page)
        {
            total = counts;
            rows = page;
        }
    }

    public class PagePars
    {
        public PagePars()
        {
            Where = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Where { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string Order { get; set; }

    }


    public enum ExecWay
    {
        Transaction = 1,
        UnitOfWork = 2
    }

}
