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

public partial class 销售_订单明细
{
    public int ID { get; set; }
    public int 单据ID { get; set; }
    public int SKU_ID { get; set; }
    public int 数量 { get; set; }
    public Nullable<decimal> 标准零售价 { get; set; }
    public Nullable<decimal> 零售单价 { get; set; }
    public Nullable<decimal> 零售小计 { get; set; }
    public Nullable<System.DateTime> 需求日期 { get; set; }
    public Nullable<decimal> 单价 { get; set; }
    public Nullable<decimal> 小计 { get; set; }
    public Nullable<System.DateTime> 预计交期 { get; set; }
    public string 明细备注 { get; set; }
    public Nullable<int> 交货周期 { get; set; }
    public Nullable<System.DateTime> 默认交期 { get; set; }
    public Nullable<int> 序号 { get; set; }

    public virtual 商品档案_SKU 商品档案_SKU { get; set; }
    public virtual 销售_订单 销售_订单 { get; set; }
}
