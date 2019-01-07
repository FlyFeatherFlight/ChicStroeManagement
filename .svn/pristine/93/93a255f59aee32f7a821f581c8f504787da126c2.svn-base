using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.BLL
{
    /// <summary>
    /// 职位逻辑
    /// </summary>
    public partial class PositionBLL:BaseService<销售_职务>,IPositionBLL
    {
        private IPositionDAL positionDAL ;

        public PositionBLL(IPositionDAL positionDAL)
        {
            this.positionDAL = positionDAL;
            base.Dal = positionDAL;
        }
    }
}
