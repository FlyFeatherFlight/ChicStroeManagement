using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicStoreManagement.WEB.ViewModel
{
    /// <summary>
    /// 日报表
    /// </summary>
    public class RecReportModel
    {
        public DateTime DateRec { get; set; }

        public int 店铺ID { get; set; }
        public string 店铺名字 { get; set; }
        public List<CustomerInfoModel> CustomerInfoModels { get; set; }
    }
}