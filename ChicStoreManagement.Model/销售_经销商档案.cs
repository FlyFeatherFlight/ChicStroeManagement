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
    
    public partial class 销售_经销商档案
    {
        public 销售_经销商档案()
        {
            this.系统用户 = new HashSet<系统用户>();
            this.销售_店铺档案 = new HashSet<销售_店铺档案>();
        }
    
        public int ID { get; set; }
        public string 编号 { get; set; }
        public string 名称 { get; set; }
        public string 负责人 { get; set; }
        public string 联系电话 { get; set; }
        public string 证件号码 { get; set; }
        public Nullable<int> 制单人 { get; set; }
        public Nullable<System.DateTime> 制单日期 { get; set; }
    
        public virtual ICollection<系统用户> 系统用户 { get; set; }
        public virtual ICollection<销售_店铺档案> 销售_店铺档案 { get; set; }
    }
}
