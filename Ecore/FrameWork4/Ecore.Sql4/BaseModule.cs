using Ecore.Frame;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecore.Sql4
{
    public class BaseModule
    {

        protected string Constr_sql { get; set; }


        protected DbType SqlType { get; set; }

        public BaseModule(string connName, DbType sqlType)
        {

            Constr_sql = Config.Default.GetConnString(connName + ".sql");

            SqlType = sqlType;
        }


        public static ThreadLocal<ExecWay?> ThreadLocal_Tag = new ThreadLocal<ExecWay?>();
        private static ThreadLocal<IDbConnection> ThreadLocal_Connection = new ThreadLocal<IDbConnection>();
        private static ThreadLocal<IDbTransaction> ThreadLocal_Transaction = new ThreadLocal<IDbTransaction>();



        #region Connection

        public IDbConnection GetConnection()
        {
            IDbConnection conn = null;
            if (ThreadLocal_Tag.Value == null)
            {
                conn = GetBaseConnection();
                return conn;
            }

            if (ThreadLocal_Tag.Value == ExecWay.Transaction)
            {
                conn = (IDbConnection)ThreadLocal_Connection.Value;
                if (conn == null)
                {
                    conn = GetBaseConnection();
                    BeginTran(conn);
                }

                return conn;
            }


            return conn;
        }


        private IDbConnection GetBaseConnection()
        {
            IDbConnection conn = null;
            if (SqlType == DbType.Sql)
            {
                conn = new SqlConnection(Constr_sql);
            }
            //等mysql 出驱动
            else if (SqlType == DbType.Mysql)
            {
                conn = new MySqlConnection(Constr_sql);
            }

            return conn;
        }

        #endregion


        #region tran


        void FinishTran(bool isSucceed)
        {
            //clear 


            if (ThreadLocal_Connection.Value != null)
            {

                if (ThreadLocal_Transaction.Value == null)
                {
                    throw new Exception("Transaction is null");
                }
                if (isSucceed)
                {
                    ThreadLocal_Transaction.Value.Commit();
                }
                else
                {
                    ThreadLocal_Transaction.Value.Rollback();
                }

                var item = ThreadLocal_Connection.Value;

                if (item.State != ConnectionState.Closed)
                {
                    item.Close();
                    item.Dispose();
                }

            }
            ThreadLocal_Tag.Value = null;
            ThreadLocal_Transaction.Value = null;
            ThreadLocal_Connection.Value = null;
        }

        public void Transaction(Action act)
        {
            if (ThreadLocal_Tag.Value != null)
            {
                throw new Exception("当前方法有未完成的事务,不能开启新的事务");
            }
            ThreadLocal_Tag.Value = ExecWay.Transaction;

            try
            {
                act();
            }
            catch (Exception e)
            {
                FinishTran(false);

                throw e;
            }

            FinishTran(true);

        }

        private void BeginTran(IDbConnection conn)
        {
            conn.Open();
            ThreadLocal_Connection.Value = conn;
            ThreadLocal_Transaction.Value = conn.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        internal static IDbTransaction GetTran()
        {
            if (ThreadLocal_Tag != null && ThreadLocal_Tag.Value == ExecWay.Transaction)
            {
                return ThreadLocal_Transaction.Value;
            }
            return null;
        }

        public IDbTransaction GetTransaction()
        {
            if (ThreadLocal_Tag != null && ThreadLocal_Tag.Value == ExecWay.Transaction)
            {
                return ThreadLocal_Transaction.Value;
            }
            return null;
        }

        #endregion


        #region UnitWork


        public void UnitWork(Action act)
        {
            ThreadLocal_Tag.Value = ExecWay.UnitOfWork;
            try
            {
                act();
                ThreadLocal_Tag.Value = null;
                //当这个时候，sql收集完毕，标记也变成了事务。
                UnitOfWork.Exec(this);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                UnitOfWork.ThreadLocal_Tag.Value = null;
            }



        }



        #endregion



    }
}
