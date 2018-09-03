using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.BLL
{
    /// <summary>
    /// 客户预计购买
    /// </summary>
    public partial class ExceptedBuyBLL:BaseService<销售_接待记录_意向明细>,IExceptedBuyBLL
    {
        private IExpectedBuyDAL exceptedBuyDAL;

        public ExceptedBuyBLL(IExpectedBuyDAL exceptedBuyDAL)
        {
            this.exceptedBuyDAL = exceptedBuyDAL;
            Dal = exceptedBuyDAL;
        }
    }
}
