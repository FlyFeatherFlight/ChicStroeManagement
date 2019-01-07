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
    /// 跟进目标数申请表
    /// </summary>
   public partial class TrackGoalBLL:BaseService<销售_跟进目标数申请表>,ITrackGoalBLL
    {
        private ITrackGoalDAL trackGoalDAL;

        public TrackGoalBLL(ITrackGoalDAL trackGoalDAL)
        {
            this.trackGoalDAL = trackGoalDAL;
            Dal = trackGoalDAL;
        }
    }
}
