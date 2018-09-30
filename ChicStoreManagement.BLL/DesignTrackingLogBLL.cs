using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.BLL
{
    /// <summary>
    /// 设计案跟进日志
    /// </summary>
  public partial  class DesignTrackingLogBLL:BaseService<销售_设计案跟进日志>,IDesignTrackingLogBLL
    {
        private IDesignTrackingLogDAL designTrackingLogDAL;
        public DesignTrackingLogBLL(IDesignTrackingLogDAL designTrackingLogDAL) {
            this.designTrackingLogDAL = designTrackingLogDAL;
            Dal = designTrackingLogDAL;
        }
    }
}
