using ChicStoreManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChicStoreManagement.WEB.ViewModel
{
    public class DesignResultViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string 客户编号 { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string 销售单号 { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string 单据编号 { get; set; }
        public DateTime? 计划完成时间 { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string 实际完成时间 { get; set; }
        public string 计划完成空间 { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string 实际完成空间 { get; set; }
        public string 延期或无法完成原因 { get; set; }
       
        public decimal? 合计成交金额 { get; set; }
        public string 建议 { get; set; }
        public string 销售人员 { get; set; }
        public string 设计师 { get; set; }
        public string 店长 { get; set; }
        public DateTime? 销售人员确认日期 { get; set; }
        public DateTime? 设计师确认日期 { get; set; }
        public DateTime? 店长审核日期 { get; set; }
        public bool? 完成效果图 { get; set; }

        public string 完成效果图路径 { get; set; }
        public bool? 审批状态 { get; set; }
        public string 更新人 { get; set; }
        public DateTime? 制单日期 { get; set; }
       
        public int? 设计案提交表ID { get; set; }

        public string 制单人 { get; set; }
        public virtual List<设计_设计案完结单_家具成交单> 设计_设计案完结单_家具成交单 { get; set; }
        public virtual 销售_设计案提交表 销售_设计案提交表 { get; set; }
    }
}