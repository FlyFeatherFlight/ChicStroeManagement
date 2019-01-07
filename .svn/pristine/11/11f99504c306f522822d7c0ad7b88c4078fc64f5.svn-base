using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.BLL
{
    /// <summary>
    /// 商店信息
    /// </summary>
    public partial class StoreBLL:BaseService<销售_店铺档案>,IStoreBLL
    {
        private IStoreDAL storeDAL;

        public StoreBLL(IStoreDAL storeDAL)
        {
            this.storeDAL = storeDAL;
            base.Dal = storeDAL;
        }
    }
}
