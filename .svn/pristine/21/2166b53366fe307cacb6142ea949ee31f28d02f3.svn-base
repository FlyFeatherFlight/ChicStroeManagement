using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;


namespace ChicStoreManagement.BLL
{
    /// <summary>
    /// 订单明细
    /// </summary>
   public partial class SalesOrder_detailsBLL:BaseService<销售_订单明细>, ISalesOrder_detailsBLL
    {
        private ISalesOrder_detailsDAL salesOrder_DetailsDAL;

        public SalesOrder_detailsBLL(ISalesOrder_detailsDAL salesOrder_DetailsDAL)
        {
            this.salesOrder_DetailsDAL = salesOrder_DetailsDAL;
            Dal = salesOrder_DetailsDAL;
        }
    }
}
