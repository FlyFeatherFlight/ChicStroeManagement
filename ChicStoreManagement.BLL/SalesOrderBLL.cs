using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.BLL
{
    /// <summary>
    /// 销售订单
    /// </summary>
    public partial class SalesOrderBLL:BaseService<销售_订单>, ISalesOrderBLL
    {
        private ISalesOrderDAL salesOrderDAL;

        public SalesOrderBLL(ISalesOrderDAL salesOrderDAL)
        {
            this.salesOrderDAL = salesOrderDAL;
            Dal = salesOrderDAL;
        }
    }
}
