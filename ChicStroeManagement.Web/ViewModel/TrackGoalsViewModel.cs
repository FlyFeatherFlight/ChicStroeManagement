using ChicStoreManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicStoreManagement.WEB.ViewModel
{
    public class TrackGoalsViewModel
    {
        public int ID { get; set; }
        public string 店铺 { get; set; }
        public string  员工姓名 { get; set; }
        public int 申请跟进数 { get; set; }
        public bool? 审核状态 { get; set; }
        public string 备注 { get; set; }
        public string 更新人 { get; set; }
        public DateTime? 更新日期 { get; set; }

        public virtual 销售_店铺档案 销售_店铺档案 { get; set; }
        public virtual 销售_店铺员工档案 销售_店铺员工档案 { get; set; }
    }
}