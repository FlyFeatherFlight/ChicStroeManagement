using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicStoreManagement.BLL
{
    /// <summary>
    /// 商品SKU
    /// </summary>
   public partial class ProductSkuBLL:BaseService<商品档案_SKU>,IProductSkuBLL
    {

        private IProductSkuDAL productSkuDAL;

        public ProductSkuBLL(IProductSkuDAL productSkuDAL)
        {
            this.productSkuDAL = productSkuDAL;
            Dal = productSkuDAL;
        }
    }
}
