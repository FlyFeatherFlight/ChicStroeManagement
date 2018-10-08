using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicStoreManagement.WEB.ViewModel
{
    /// <summary>
    /// 意向追踪
    /// </summary>
    public class CustomerTrackingModel
    {

        public int Id { get; set; }
        public string 接待序号 { get; set; }
        public string 跟进人 { get; set; }
        public DateTime 跟进时间 { get; set; }
        public string 跟进方式 { get; set; }
        public string 跟进内容 { get; set; }
        public string 跟进结果 { get; set; }
        public string 店长审核 { get; set; }
        public string 备注 { get; set; }
        public Nullable<bool> 是否申请设计师 { get; set; }
        public string 店铺 { get; set; }

        public virtual 销售_店铺员工档案 销售_店铺员工档案 { get; set; }
        public virtual 销售_接待记录 销售_接待记录 { get; set; }
    }
}