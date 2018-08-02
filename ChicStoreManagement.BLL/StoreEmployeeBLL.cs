using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;
using System;
using System.Linq;

namespace ChicStoreManagement.BLL
{
    public partial class StoreEmployeeBLL : BaseService<销售_店铺员工档案>,IStoreEmployeesBLL

    {

        public bool UpdateUser(销售_店铺员工档案 storeEmploee)
        {
            throw new NotImplementedException();
        }

        public bool AddUser(销售_店铺员工档案 storeEmploee)
        {
            throw new NotImplementedException();
        }

        public bool DelUser(销售_店铺员工档案 storeEmploee)
        {
            throw new NotImplementedException();
        }
       
        public 销售_店铺员工档案 GetById(int id)
        {
            return null;
        }

        public IQueryable<销售_店铺员工档案> GetAll(string entity)
        {
            return this.GetCurrentDbSession.I销售_店铺员工档案Repository.LoadEntitiesAll(entity);
        } 
    }
}
 
