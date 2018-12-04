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
    
    public partial class 设计_设计案完结单
    {
        public 设计_设计案完结单()
        {
            this.设计_设计案完结单_家具成交单 = new HashSet<设计_设计案完结单_家具成交单>();
        }
    
        public int id { get; set; }
        public string 客户单号 { get; set; }
        public string 销售单号 { get; set; }
        public string 单据编号 { get; set; }
        public Nullable<System.DateTime> 计划完成时间 { get; set; }
        public string 实际完成时间 { get; set; }
        public string 计划完成空间 { get; set; }
        public string 实际完成空间 { get; set; }
        public string 延期或无法完成原因 { get; set; }
        public Nullable<decimal> 合计成交金额 { get; set; }
        public string 建议 { get; set; }
        public string 销售人员 { get; set; }
        public string 设计师 { get; set; }
        public string 店长 { get; set; }
        public Nullable<System.DateTime> 销售人员确认日期 { get; set; }
        public Nullable<System.DateTime> 设计师确认日期 { get; set; }
        public Nullable<System.DateTime> 店长确认日期 { get; set; }
        public Nullable<bool> 审批状态 { get; set; }
        public string 更新人 { get; set; }
        public Nullable<System.DateTime> 制单日期 { get; set; }
        public Nullable<int> 设计案提交表ID { get; set; }
        public string 制单人 { get; set; }
        public Nullable<bool> 店长审核 { get; set; }
        public Nullable<bool> 销售审核 { get; set; }
    
        public virtual ICollection<设计_设计案完结单_家具成交单> 设计_设计案完结单_家具成交单 { get; set; }
        public virtual 销售_设计案提交表 销售_设计案提交表 { get; set; }
    }
}
