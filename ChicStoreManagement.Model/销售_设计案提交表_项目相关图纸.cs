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
    
    public partial class 销售_设计案提交表_项目相关图纸
    {
        public int id { get; set; }
        public int 设计案提交表ID { get; set; }
        public Nullable<System.DateTime> 提交时间 { get; set; }
        public string 备注 { get; set; }
        public string 更新人 { get; set; }
        public Nullable<System.DateTime> 更新日期 { get; set; }
        public string 提交人 { get; set; }
        public int 文件类型 { get; set; }
        public string 存储路径 { get; set; }
        public string 文件名 { get; set; }
    
        public virtual 销售_设计案提交表 销售_设计案提交表 { get; set; }
    }
}
