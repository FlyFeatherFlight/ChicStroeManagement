using ChicStoreManagement.DAL;
using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.BLL
{

    /// <summary>
    /// 顾客接待信息
    /// </summary>

    public partial class CustomerInfoBLL:BaseService<销售_接待记录>,ICustomerInfoBLL
    {
        private CustomerInfoDAL customerDAL;

        public CustomerInfoBLL(CustomerInfoDAL customerDAL)
        {
            
            this.customerDAL = customerDAL;
            base.Dal = customerDAL;
        }
    }
}
