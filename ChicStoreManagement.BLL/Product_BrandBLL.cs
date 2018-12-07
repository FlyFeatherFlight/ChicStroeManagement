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
    /// 商品档案_品牌
    /// </summary>
    public partial class Product_BrandBLL:BaseService<商品档案_品牌>,IProduct_BrandBLL
    {
        private IProduct_BrandDAL product_BrandDAL;

        public Product_BrandBLL(IProduct_BrandDAL product_BrandDAL)
        {
            this.product_BrandDAL = product_BrandDAL;
            Dal = product_BrandDAL;
        }
    }
}
