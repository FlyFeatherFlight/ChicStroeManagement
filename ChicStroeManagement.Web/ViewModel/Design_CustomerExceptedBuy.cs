using ChicStoreManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicStoreManagement.WEB.ViewModel
{

    /// <summary>
    /// 设计—客户意向产品明细
    /// </summary>
    public class Design_CustomerExceptedBuy
    {
        public int Id { get; set; }
        public int 接待记录ID { get; set; }
        public int 设计提交案 { get; set; }
        public string 空间 { get; set; }
        public string 系列 { get; set; }
        public string 产品 { get; set; }
        public string 编号 { get; set; }
        public string 尺寸 { get; set; }
        public string 单位 { get; set; }
        public int? 数量 { get; set; }
        public string 配置 { get; set; }
        public string 更新人 { get; set; }
        public DateTime? 更新日期 { get; set; }

        public virtual 销售_接待记录 销售_接待记录 { get; set; }
        public virtual 销售_设计案提交表 销售_设计案提交表 { get; set; }
    }
}