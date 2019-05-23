﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicStoreManagement.WEB.ViewModel
{
    /// <summary>
    /// 销售订单详细
    /// </summary>
    public class SalesOrdersDetailsViewModel
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

    }
}