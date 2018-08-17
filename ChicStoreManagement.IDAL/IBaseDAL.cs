using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChicStoreManagement.IDAL
{
    public partial interface IBaseDAL<T> where T : class, new()
    {
        int Add(T t);
        T AddReturnModel(T model);
        int Del(T model);
        int DelBy(Expression<Func<T, bool>> delWhere);
        int Modify(T model);

        int Modify(T model, params string[] propertyNames);
        int ModifyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedPropertyNames);
        /// <summary>
        /// 单个查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        T GetModel(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 根据条件查询单个model并排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        T GetModel<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true);
       /// <summary>
       /// 根据条件查询实体集
       /// </summary>
       /// <param name="whereLambda"></param>
       /// <returns></returns>
        List<T> GetListBy(Expression<Func<T, bool>> whereLambda);


        /// <summary>
        ///  根据条件查询并排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        List<T> GetListBy<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true);
        /// <summary>
        /// 5.2 根据条件查询Top多少个，并排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="top"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        List<T> GetListBy<TKey>(int top, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true);
        /// <summary>
        /// 5.3 根据条件排序查询  双排序
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda1"></param>
        /// <param name="orderLambda2"></param>
        /// <param name="isAsc1"></param>
        /// <param name="isAsc2"></param>
        /// <returns></returns>
        List<T> GetListBy<TKey1, TKey2>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey1>> orderLambda1, Expression<Func<T, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true);
        /// <summary>
        ///  5.3 根据条件排序查询Top个数  双排序
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="top"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda1"></param>
        /// <param name="orderLambda2"></param>
        /// <param name="isAsc1"></param>
        /// <param name="isAsc2"></param>
        /// <returns></returns>
        List<T> GetListBy<TKey1, TKey2>(int top, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey1>> orderLambda1, Expression<Func<T, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true);
        /// <summary>
        /// 分页查询 + List<T> GetPagedList
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">条件 lambda表达式</param>
        /// <param name="orderBy">排序 lambda表达式</param>
        /// <returns></returns>
        List<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true);
        /// <summary>
        /// 分页查询 带输出
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        List<T> GetPagedList<TKey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true);


        /// <summary>
        /// 分页查询 带输出 并支持双字段排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderByLambda1"></param>
        /// <param name="orderByLambda2"></param>
        /// <param name="isAsc1"></param>
        /// <param name="isAsc2"></param>
        /// <returns></returns>
      List<T> GetPagedList<TKey1, TKey2>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey1>> orderByLambda1, Expression<Func<T, TKey2>> orderByLambda2, bool isAsc1 = true, bool isAsc2 = true);

       /// <summary>
       /// 查询所有实体
       /// </summary>
       /// <param name="whereLambda"></param>
       /// <returns></returns>
        IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda);
        /// <summary>
        /// 一个业务中有可能涉及到对多张表的操作,那么可以将操作的数据,打上相应的标记,最后调用该方法,将数据一次性提交到数据库中,避免了多次链接数据库。
        /// </summary>
        int SaveChanges();
    }
}
