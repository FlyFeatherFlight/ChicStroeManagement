using ChicStoreManagement.IDAL;
using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.BLL
{
    /// <summary>
    /// 员工信息
    /// </summary>
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
 
