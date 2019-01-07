using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.BLL
{

    /// <summary>
    /// 商品型号逻辑
    /// </summary>
    public partial class ProductCodeBLL:BaseService<商品档案_型号>,IProductCodeBLL
    {
        private IProductCodeDAL productCodeDAL;

        public ProductCodeBLL(IProductCodeDAL productCodeDAL)
        {
            this.productCodeDAL = productCodeDAL;
            Dal = productCodeDAL;
        }
    }
}
