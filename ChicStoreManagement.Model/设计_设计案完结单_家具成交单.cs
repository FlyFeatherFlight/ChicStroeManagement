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

public partial class 设计_设计案完结单_家具成交单
{
    public int id { get; set; }
    public string 空间 { get; set; }
    public string 产品型号 { get; set; }
    public string 单位 { get; set; }
    public Nullable<int> 数量 { get; set; }
    public Nullable<decimal> 成交价 { get; set; }
    public int 设计案完结单 { get; set; }

    public virtual 设计_设计案完结单 设计_设计案完结单 { get; set; }
}
