using ChicStoreManagement.Model;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.IBLL;

namespace ChicStoreManagement.BLL
{
    /// <summary>
    /// 设计案提交表
    /// </summary>
    public partial class DesignSubmitBLL:BaseService<销售_设计案提交表>,IDesignSubmitBLL
    {
        private IDesignSubmitDAL designSubmitDAL;
        public DesignSubmitBLL(IDesignSubmitDAL designSubmitDAL) {
            this.designSubmitDAL = designSubmitDAL;
            Dal = designSubmitDAL;
        }
    }
}
