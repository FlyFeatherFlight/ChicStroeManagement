//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChicStoreManagement.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class 销售_店铺员工档案
    {
        public 销售_店铺员工档案()
        {
            this.销售_接待记录 = new HashSet<销售_接待记录>();
            this.销售_订单 = new HashSet<销售_订单>();
            this.销售_意向追踪日志 = new HashSet<销售_意向追踪日志>();
            this.销售_跟进目标数申请表 = new HashSet<销售_跟进目标数申请表>();
        }
    
        public int ID { get; set; }
        public int 店铺ID { get; set; }
        public string 编号 { get; set; }
        public string 姓名 { get; set; }
        public string 性别 { get; set; }
        public int 职务ID { get; set; }
        public string 联系方式 { get; set; }
        public bool 停用标志 { get; set; }
        public Nullable<int> 制单人 { get; set; }
        public Nullable<System.DateTime> 制单日期 { get; set; }
        public string 密码 { get; set; }
        public Nullable<int> 跟进目标计划数 { get; set; }
    
        public virtual 销售_店铺档案 销售_店铺档案 { get; set; }
        public virtual 销售_职务 销售_职务 { get; set; }
        public virtual ICollection<销售_接待记录> 销售_接待记录 { get; set; }
        public virtual ICollection<销售_订单> 销售_订单 { get; set; }
        public virtual ICollection<销售_意向追踪日志> 销售_意向追踪日志 { get; set; }
        public virtual ICollection<销售_跟进目标数申请表> 销售_跟进目标数申请表 { get; set; }
    }
}
