using SqlSugar;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISqlSugarClient _sqlSugarClient;

        //public UnitOfWork(ISqlSugarClient sqlSugarClient)
        //{
        //    _sqlSugarClient = sqlSugarClient;
        //}
        public UnitOfWork()
        {
            _sqlSugarClient = DbTransient.Sugar;
        }

        /// <summary>获取DB，保证唯一性</summary>
        /// <returns></returns>
        public SqlSugarClient GetDbClient()
        {
            // 必须要as，后边会用到切换数据库操作
            return _sqlSugarClient as SqlSugarClient;
        }

        public void BeginTranReadUncommitted()
        {
            GetDbClient().Ado.BeginTran(System.Data.IsolationLevel.ReadUncommitted);
        }

        public void CommitTranUncommitted()
        {
            try
            {
                GetDbClient().Ado.CommitTran(); //
            }
            catch (Exception ex)
            {
                GetDbClient().Ado.RollbackTran();
                throw ex;
            }
        }

        public void RollbackTranUncommitted()
        {
            GetDbClient().RollbackTran();
        }

        public void BeginTran()
        {
            GetDbClient().BeginTran();
        }

        public void CommitTran()
        {
            try
            {
                GetDbClient().CommitTran(); //
            }
            catch (Exception ex)
            {
                GetDbClient().RollbackTran();
                throw ex;
            }
        }

        public void RollbackTran()
        {
            GetDbClient().RollbackTran();
        }
    }
}