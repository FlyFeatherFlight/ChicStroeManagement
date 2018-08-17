using ChicStoreManagement.IDAL;
using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;
using System;
using System.Linq;

namespace ChicStoreManagement.BLL
{
    public partial class StoreEmployeeBLL : BaseService<销售_店铺员工档案>,IStoreEmployeesBLL

    {
        private IEmployeeDAL storeEmployeesDAL;

        public StoreEmployeeBLL(IEmployeeDAL storeEmployeesDAL)
        {
            this.storeEmployeesDAL = storeEmployeesDAL;
            base.Dal = storeEmployeesDAL;
        }

    }
}
 
