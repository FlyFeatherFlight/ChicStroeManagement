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
    
    public partial class 销售_设计案跟进日志
    {
        public int id { get; set; }
        public string 销售人员 { get; set; }
        public string 设计师 { get; set; }
        public Nullable<System.DateTime> 设计案需求提交时间 { get; set; }
        public System.DateTime 跟进日期 { get; set; }
        public string 进度描述 { get; set; }
        public string 参与人员 { get; set; }
        public string 需要的支持 { get; set; }
        public Nullable<System.DateTime> 预计签约时间 { get; set; }
        public Nullable<bool> 店长审查 { get; set; }
        public string 备注 { get; set; }
        public int 设计案提交表id { get; set; }
        public Nullable<System.DateTime> 更新日期 { get; set; }
        public string 更新人 { get; set; }
        public int 店铺ID { get; set; }
    
        public virtual 销售_店铺档案 销售_店铺档案 { get; set; }
        public virtual 销售_设计案提交表 销售_设计案提交表 { get; set; }
    }
}
