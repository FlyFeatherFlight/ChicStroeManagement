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
    
    public partial class 销售_意向追踪日志
    {
        public int id { get; set; }
        public int 店铺ID { get; set; }
        public int 接待记录ID { get; set; }
        public int 跟进人 { get; set; }
        public System.DateTime 跟进时间 { get; set; }
        public string 跟进方式 { get; set; }
        public string 跟进内容 { get; set; }
        public string 跟进结果 { get; set; }
        public string 店长审核 { get; set; }
        public string 备注 { get; set; }
        public Nullable<bool> 是否申请设计师 { get; set; }
    
        public virtual 销售_店铺档案 销售_店铺档案 { get; set; }
        public virtual 销售_店铺员工档案 销售_店铺员工档案 { get; set; }
    }
}
