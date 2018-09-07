using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.BLL
{

    /// <summary>
    /// 顾客接待信息
    /// </summary>

    public partial class CustomerInfoBLL:BaseService<销售_接待记录>,ICustomerInfoBLL
    {
        private ICustomerInfoDAL customerDAL;

        public CustomerInfoBLL(ICustomerInfoDAL customerDAL)
        {
            
            this.customerDAL = customerDAL;
            base.Dal = customerDAL;
        }
    }
}
