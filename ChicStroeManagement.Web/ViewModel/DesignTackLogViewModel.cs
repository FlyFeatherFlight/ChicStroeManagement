using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicStoreManagement.WEB.ViewModel
{
    /// <summary>
    /// 设计案跟进日志ViewModel
    /// </summary>
    public class DesignTackLogViewModel
    {
        public int Id { get; set; }
        public string  客户姓名 { get; set; }
        public string 联系方式 { get; set; }
        public string 楼盘具体位置 { get; set; }
        public string 销售人员 { get; set; }
        public string 设计师 { get; set; }
        public DateTime? 设计案需求提交时间 { get; set; }
        public string 跟进日期 { get; set; }
        public string 进度描述 { get; set; }
        public string 参与人员 { get; set; }
        public string 需要的支持 { get; set; }
        public DateTime? 预计签约时间 { get; set; }
        public bool? 店长审查 { get; set; }
        public string 备注 { get; set; }
        public int 设计案提交表id { get; set; }

      
    }
}