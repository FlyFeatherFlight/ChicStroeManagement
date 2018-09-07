using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicStoreManagement.WEB.ViewModel
{
    public class ProductCodeModel
    {

        public int ID { get; set; }
        public int 分类ID { get; set; }
        public int? 品牌ID { get; set; }
        public int? 系列ID { get; set; }
        public string 型号 { get; set; }
        public int? 制单人 { get; set; }
        public DateTime? 制单日期 { get; set; }
        public bool? 停用标志 { get; set; }
    }
}