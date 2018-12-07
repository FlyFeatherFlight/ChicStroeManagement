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
    /// 商品档案_分类
    /// </summary>
    public partial class Product_ClassfiyBLL:BaseService<商品档案_分类>, IProduct_ClassfiyBLL
    {
        private IProduct_ClassfiyDAL product_ClassfiyDAL;

        public Product_ClassfiyBLL(IProduct_ClassfiyDAL product_ClassfiyDAL)
        {
            this.product_ClassfiyDAL = product_ClassfiyDAL;
            Dal = product_ClassfiyDAL;
        }
    }
}
