using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.BLL
{
    /// <summary>
    /// 系统用户逻辑
    /// </summary>
    public partial class SystemAccountBLL:BaseService<系统用户>,ISystemAccountBLL
    {
        private ISystemAccountDAL systemAccountDAL;

        public SystemAccountBLL(ISystemAccountDAL systemAccountDAL)
        {
            this.systemAccountDAL = systemAccountDAL;
            Dal = systemAccountDAL;
        }
    }
}
