using ChicStoreManagement.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ChicStoreManagement.BLL
{
    public  abstract partial class BaseService<T> where T : class, new()

    {
        protected IBaseDAL<T> Dal;
        //public BaseService()
        //{
        //    SetDal();
        //}

        //public IBaseDAL<T> Dal;

        //public abstract void SetDal();

        public int Add(T t)
        {
            Dal.Add(t);
            return Dal.SaveChanges();
        }

        /// <summary>
        /// 1.1 新增实体，返回对应的实体对象
        /// </summary>
        /// <param name = "model" ></ param >
        /// < returns ></ returns >
        public T AddReturnModel(T model)
        {
           return Dal.AddReturnModel(model);
        }
        public int DelBy(Expression<Func<T, bool>> delWhere) {
            Dal.DelBy(delWhere);
            return Dal.SaveChanges();

        }
        public int Del(T t)
        {
            Dal.Del(t);
            return Dal.SaveChanges();
        }
        public int Modify(T model) {
            Dal.Modify(model);
            return Dal.SaveChanges();

        }
        public int Modify(T t, params string[] propertyNames)
        {

            try
            {
                        Dal.Modify(t,propertyNames);
                return Dal.SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            } 
            
        }
        /// <summary>
        /// 3.2 批量修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="whereLambda"></param>
        /// <param name="modifiedPropertyNames"></param>
        /// <returns></returns>
        public int ModifyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedPropertyNames)
        {
            Dal.ModifyBy(model,whereLambda, modifiedPropertyNames);
            return Dal.SaveChanges();
        }

        //查，查单个model
        #region 4.0 根据条件查询单个model + T GetModel(Expression<Func<T, bool>> whereLambda)
        /// <summary>
        /// 4.0 根据条件查询单个model
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T GetModel(Expression<Func<T, bool>> whereLambda)
        {
            return Dal.GetModel(whereLambda);
            
        }

        /// <summary>
        /// 4.1 根据条件查询单个model并排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public T GetModel<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true)
        {
            return Dal.GetModel(whereLambda, orderLambda, isAsc);
        }
        #endregion

       #region  查，返回List
       
        /// <summary>
        /// 5.0 根据条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<T> GetListBy(Expression<Func<T, bool>> whereLambda)
        {
            return Dal.GetListBy(whereLambda);
        }
        
        
        /// <summary>
        /// 5.1 根据条件查询，并排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public List<T> GetListBy<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true)
        {
            return Dal.GetListBy(whereLambda, orderLambda, isAsc);
        }
   
        /// <summary>
        /// 5.2 根据条件查询Top多少个，并排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="top"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public List<T> GetListBy<TKey>(int top, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true)
        {
            return Dal.GetListBy(top, whereLambda, orderLambda, isAsc);
        }
       
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
        public List<T> GetListBy<TKey1, TKey2>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey1>> orderLambda1, Expression<Func<T, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true)
        {
            return Dal.GetListBy(whereLambda,orderLambda1,orderLambda2,isAsc1,isAsc2);
        }
        
    
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
        public List<T> GetListBy<TKey1, TKey2>(int top, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey1>> orderLambda1, Expression<Func<T, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true)
        {
            return Dal.GetListBy(top,whereLambda,orderLambda1,orderLambda2,isAsc1,isAsc2);
        }
        #endregion

       #region 查，带分页查询
        
        /// <summary>
        /// 分页查询 + List<T> GetPagedList
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">条件 lambda表达式</param>
        /// <param name="orderBy">排序 lambda表达式</param>
        /// <returns></returns>
        public List<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy
            return Dal.GetPagedList(pageIndex,pageSize,whereLambda,orderByLambda,isAsc);
        }
        
        
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
        public List<T> GetPagedList<TKey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true)
        {
            return Dal.GetPagedList(pageIndex,pageSize,ref rowCount,whereLambda,orderByLambda,isAsc);
        }
     
      
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
        public List<T> GetPagedList<TKey1, TKey2>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey1>> orderByLambda1, Expression<Func<T, TKey2>> orderByLambda2, bool isAsc1 = true, bool isAsc2 = true)
        {
            return Dal.GetPagedList(pageIndex,pageSize,ref rowCount,whereLambda,orderByLambda1,orderByLambda2,isAsc1,isAsc2);
        }
        #endregion

        /// <summary>
        /// 查询，返回IQueryable<T>
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return Dal.GetModels(whereLambda);
        }
        /// <summary>
        /// 查询分页并排序
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="isAsc"></param>
        /// <param name="OrderByLambda"></param>
        /// <param name="WhereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc,
            Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda)
        {
            return Dal.GetModelsByPage(pageSize, pageIndex, isAsc, OrderByLambda, WhereLambda);
        }
    }
}
