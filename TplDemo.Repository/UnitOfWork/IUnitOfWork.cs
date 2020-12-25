using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        SqlSugarClient GetDbClient();

        void BeginTranReadUncommitted();

        void CommitTranUncommitted();

        void RollbackTranUncommitted();

        void BeginTran();

        void CommitTran();

        void RollbackTran();
    }
}