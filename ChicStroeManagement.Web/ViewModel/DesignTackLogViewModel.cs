using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChicStoreManagement.WEB.ViewModel
{
    /// <summary>
    /// 设计案跟进日志ViewModel
    /// </summary>
    public class DesignTackLogViewModel
    {
        public int? Id { get; set; }
        [DataType(DataType.Text)]
      
        public string  客户姓名 { get; set; }
        [DataType(DataType.Text)]
      
        public string 联系方式 { get; set; }
        [DataType(DataType.Text)]
       
        public string 楼盘具体位置 { get; set; }
        [DataType(DataType.Text)]
       
        public string 销售人员 { get; set; }
        [DataType(DataType.Text)]
      
        public string 设计师 { get; set; }
        [DataType(DataType.Date)]
       
        public DateTime? 设计案需求提交时间 { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "跟进日期不能为空！")]
        public DateTime 跟进日期 { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "进度描述不能为空！")]
        public string 进度描述 { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "参与人员不能为空！")]
        public string 参与人员 { get; set; }
        
        public string 需要的支持 { get; set; }
        public DateTime? 预计签约时间 { get; set; }
        public bool? 店长审查 { get; set; }
        public string 备注 { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "设计案提交表id 不能为空！")]
        public int 设计案提交表id { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "店铺id 不能为空！")]
        public int 店铺ID { get; set; }
      
    }
}