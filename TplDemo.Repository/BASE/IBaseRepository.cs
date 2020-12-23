using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TplDemo.Common;
using TplDemo.Common.Message;

namespace TplDemo.Repository.BASE
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> QueryById(object objId);

        Task<TEntity> QueryById(object objId, bool blnUseCache = false);

        Task<List<TEntity>> QueryByIDs(object[] lstIds);

        Task<int> Add(TEntity model);

        Task<int> Add(List<TEntity> listEntity);

        Task<bool> DeleteById(object id);

        Task<bool> Delete(TEntity model);

        Task<bool> DeleteByIds(object[] ids);

        Task<bool> Update(TEntity model);

        Task<bool> Update(TEntity entity, string strWhere);

        Task<bool> Update(object operateAnonymousObjects);

        Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "");

        Task<List<TEntity>> Query();

        Task<List<TEntity>> Query(string strWhere);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);

        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selectExpression, string strOrderByFileds);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);

        Task<List<TEntity>> Query(string strWhere, string strOrderByFileds);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds);

        Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds);

        Task<List<TEntity>> QuerySql(string strSql, SugarParameter[] parameters = null);

        Task<DataTable> QueryTable(string strSql, SugarParameter[] parameters = null);

        Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, string strOrderByFileds);

        Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds);

        Task<PageModel<List<TEntity>>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);

        Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();

        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<DateTime> GetDateTime(string sql, object parameters);

        /// <summary></summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>

        Task<dynamic> GetDecimal(string sql, object parameters);

        /// <summary>查询返回单条记录(int)</summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> GetInt(string sql, object parameters);

        //以下为返回string
        /// <summary>查询返回单条记录(string)</summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<string> GetString(string sql, object parameters);

        Task<List<TResult>> GetQuerylistTResult<TResult>(string sql, object parameters = null);

        Task<List<TResult>> QueryMuchtwo<T, T2, TResult>(
         Expression<Func<T, T2, object[]>> joinExpression,
         Expression<Func<T, T2, TResult>> selectExpression,
         Expression<Func<T, T2, bool>> whereLambda = null) where T : class, new();

        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selectExpression, string strOrderByFileds, int top);

        Task<PageModel<List<TResult>>> GetQueryablePage<T, T2, TResult>(
                   Expression<Func<T, T2, object[]>> joinExpression,
                   Expression<Func<T, T2, TResult>> selectExpression,
                   Expression<Func<T, T2, bool>> whereLambda = null, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null) where T : class, new();

        Task<PageModel<List<TResult>>> QueryPageTResult<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selectExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);
    }
}