using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;
using NHibernate;
using NHibernate.Linq;
using System.Linq;


namespace ChicStoreManagement.DAL
{
    public class StoreEmployeesDAL : BaseRepository<销售_店铺员工档案>, I销售_店铺员工档案Repository
    {
       

        public IQueryable<销售_店铺员工档案> GetAll(string entity)
        {
            return LoadEntitiesAll(entity);
        }

        public 销售_店铺员工档案 GetById(int id)
        {
            return FindSingleOrDefault(e => e.ID == id);
        }
    }

}
