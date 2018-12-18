using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicStoreManagement.WEB.ViewModel
{
    /// <summary>
    /// 设计案完结—商品清单
    /// </summary>
    public class DesignResult_OrderDetailViewModel
    {
        public string 空间 { get; set; }
        public string 产品型号 { get; set; }
        public string 单位 { get; set; }
        public Nullable<int> 数量 { get; set; }
        public Nullable<decimal> 成交价 { get; set; }
    }
}