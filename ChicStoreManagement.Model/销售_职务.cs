//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public partial class 销售_职务
{
    public 销售_职务()
    {
        this.销售_店铺员工档案 = new HashSet<销售_店铺员工档案>();
    }

    public int ID { get; set; }
    public string 职务 { get; set; }

    public virtual ICollection<销售_店铺员工档案> 销售_店铺员工档案 { get; set; }
}
