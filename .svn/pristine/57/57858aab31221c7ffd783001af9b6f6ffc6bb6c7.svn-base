using ChicStoreManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChicStoreManagement.WEB.ViewModel
{
    /// <summary>
    /// CHIC软装服务设计案提交表
    /// </summary>
    public class DesignSubmitModel
    {
       
        public int Id { get; set; }

        [Display(Name ="接待记录ID")]
        [DataType(DataType.Text)]
        [Required]
        public int 接待记录ID { get; set; }
        [Display(Name = "客户姓名")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "客户姓名为必填项。")]
        public string 客户姓名 { get; set; }
        [Display(Name = "联系方式")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string 联系方式 { get; set; }
        [Display(Name = "职业")]
        [DataType(DataType.Text)]
        [Required]
        public string 职业 { get; set; }
        [Display(Name = "家庭成员")]
        [DataType(DataType.Text)]
        [Required]
        public string 家庭成员 { get; set; }
        [Display(Name = "楼盘具体位置")]
        [DataType(DataType.Text)]
        [Required]
        public string 楼盘具体位置 { get; set; }
        [Display(Name = "面积大小")]
        [DataType(DataType.Text)]
        [Required]
        public string 面积大小 { get; set; }
        [Display(Name = "装修风格")]
        [DataType(DataType.Text)]
        [Required]
        public string 装修风格 { get; set; }
        [Display(Name = "装修进度")]
        [DataType(DataType.Text)]
        [Required]
        public string 装修进度 { get; set; }
        [Display(Name = "预算")]
        [DataType(DataType.Currency)]
        
        public string 预算 { get; set; }
        [Display(Name = "客户喜好或忌讳")]
        [DataType(DataType.Text)]
    
        public string 客户喜好 { get; set; }
        [Display(Name = "客户在意品牌或已购买品牌")]
        [DataType(DataType.Text)]
        
        public string 客户在意品牌或已购买品牌 { get; set; }
       
        [Display(Name = "客户提问与要求")]
        [DataType(DataType.Text)]

        public string 客户提问与要求 { get; set; }
       
        [Display(Name = "整体软装配饰")]
   
        public bool? 整体软装配饰 { get; set; }
        [Display(Name = "家具空间")]
        [DataType(DataType.Text)]
     
        public string 家具空间 { get; set; }
 
        [Display(Name = "销售人员")]
        [DataType(DataType.Text)]
        
        [Required(ErrorMessage ="销售人员是必须的！")]
        public string 销售人员 { get; set; }
        [Display(Name = "设计人员")]
        [DataType(DataType.Text)]
        
        public string 设计人员 { get; set; }
        [Display(Name = "店长")]
        [DataType(DataType.Text)]
        
        public string 店长 { get; set; }

        [Display(Name = "店长审核")]
        [DataType(DataType.Text)]
      
        public Nullable<bool> 店长审核 { get; set; }
        [Display(Name = "设计人员审核")]
        [DataType(DataType.Text)]
    
        public Nullable<bool> 设计人员审核 { get; set; }

        [Display(Name = "项目提交时间")]
        [DataType(DataType.DateTime)]
        [Required]
        public System.DateTime 项目提交时间 { get; set; }
        [Display(Name = "项目量房时间")]
        [DataType(DataType.Date)]
    
        public DateTime 项目量房时间 { get; set; }
        [Display(Name = "项目预计完成时间")]
        [DataType(DataType.Date)]
        [Required]
        public System.DateTime 项目预计完成时间 { get; set; }
        [Display(Name = "审批状态")]
        [DataType(DataType.Text)]
        public bool? 审批状态 { get; set; }

        [Display(Name ="备注")]
        [DataType(DataType.Text)]
        public string 备注 { get; set; }
        [Display(Name ="最后一次更新人")]
        [DataType(DataType.Text)]
        public string 更新人 { get; set; }
        [Display(Name ="更新日期")]
        [DataType(DataType.DateTime)]
        public DateTime? 更新日期 { get; set; }
        public virtual 销售_接待记录 销售_接待记录 { get; set; }
        public virtual ICollection<销售_设计案提交表_项目相关图纸> 销售_设计案提交表_项目相关图纸 { get; set; }
        public virtual List<Design_CustomerExceptedBuyViewModel> Design_CustomerExceptedBuyViewModel{ get; set; }
        public virtual ICollection<销售_设计案跟进日志> 销售_设计案跟进日志 { get; set; }
    }
}