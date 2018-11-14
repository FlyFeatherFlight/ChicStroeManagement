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
    /// 设计案完结表
    /// </summary>
  public  partial class DesignResultBLL:BaseService<设计_设计案完结单>,IDesignResultBLL
    {
        private IDesignResultDAL designResultDAL;

        public DesignResultBLL(IDesignResultDAL designResultDAL)
        {
            this.designResultDAL = designResultDAL;
            Dal = designResultDAL;
        }
    }
}
