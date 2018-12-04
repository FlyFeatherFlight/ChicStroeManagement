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
   public partial class ProductBLL:BaseService<商品档案_商品>,IProductBLL
    {
        private IProductDAL productDAL;

        public ProductBLL(IProductDAL productDAL)
        {
            this.productDAL = productDAL;
            Dal = productDAL;
        }
    }
}
