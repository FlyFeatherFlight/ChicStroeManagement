using ChicStoreManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChicStoreManagement.WEB.ViewModel
{
    /// <summary>
    /// 客户接待信息
    /// </summary>
    public class CustomerInfoModel
    {
        public int ID { get; set; }

        [Display(Name = "店铺")]
        [DataType(DataType.Text)]
        [Required]
        public string 店铺 { get; set; }

        [Display(Name = "接待序号")]
        [DataType(DataType.Text)]
        [Required]
        public string 接待序号 { get; set; }

        [Display(Name = "接待日期")]
        [DataType(DataType.DateTime)]
        [Required]
        public System.DateTime 接待日期 { get; set; }


        [Display(Name = "客户来源")]
        [DataType(DataType.Text)]
        [Required]
        public string 客户来源 { get; set; }

        [Display(Name = "客户姓名")]
        [DataType(DataType.Text)]
        [MaxLength(length: 50)]
        [Required]
        public string 客户姓名 { get; set; }

        [Display(Name = "客户性别")]
        [DataType(DataType.Text)]
        [Required]
        public string 性别 { get; set; }

        [Display(Name = "年龄段")]
        [DataType(DataType.Text)]
        [Required]
        public string 年龄段 { get; set; }

        [Display(Name = "客户类型")]
        [DataType(DataType.Text)]
        [Required]
        public string 客户类别 { get; set; }

        [Display(Name = "客户电话")]
        [DataType(DataType.PhoneNumber)]
       
        public string 客户电话 { get; set; }

        [Display(Name = "客户类型")]
        [DataType(DataType.Text)]
        
        public string 客户类型 { get; set; }

        [Display(Name = "社交软件")]
        [DataType(DataType.Text)]
       
        public string 社交软件 { get; set; }

        [Display(Name = "来电次数")]
        [DataType(DataType.Text)]
        [Required]
        public int 来店次数 { get; set; }

        [Display(Name = "接待人")]
        [DataType(DataType.Text)]
        [Required]
        public string 接待人 { get; set; }
        [Display(Name = "跟进人")]
        [DataType(DataType.Text)]
        
        public string 跟进人{ get; set; }

        [Display(Name = "进店时间")]
        [DataType(DataType.DateTime)]
        [Required]
        public System.DateTime 进店时间 { get; set; }

        [Display(Name = "出店时间")]
        [DataType(DataType.DateTime)]
        [Required]
        public System.DateTime 出店时间 { get; set; }

        [Display(Name = "进店时长")]
        [DataType(DataType.Text)]
        [Required]
        public int 进店时长 { get; set; }

        [Display(Name = "客户职业")]
        [DataType(DataType.Text)]
        public string 客户职业 { get; set; }

        [Display(Name = "客户着装")]
        [DataType(DataType.Text)]
        public string 客户着装 { get; set; }

        [Display(Name = "同行人")]
        [DataType(DataType.Text)]
        public string 同行人 { get; set; }

        [Display(Name = "主导者")]
        [DataType(DataType.Text)]
        public string 主导者 { get; set; }

        [Display(Name = "主导者喜好风格")]
        [DataType(DataType.Text)]
        public string 主导者喜好风格 { get; set; }

        [Display(Name = "客户特征")]
        [DataType(DataType.Text)]
        public string 特征 { get; set; }

        [Display(Name = "设计师")]
        [DataType(DataType.Text)]
        public string 设计师 { get; set; }

        [Display(Name = "家庭成员")]
        [DataType(DataType.Text)]
        public string 家庭成员 { get; set; }

        [Display(Name = "使用空间")]
        [DataType(DataType.Text)]
        [Required]
        public string 使用空间 { get; set; }
        [Display(Name = "装修风格")]
        [DataType(DataType.Text)]
        [Required]
        public string 装修风格 { get; set; }

        [Display(Name = "装修情况")]
        [DataType(DataType.Text)]
        
        public string 装修情况 { get; set; }

        [Display(Name = "装修进度")]
        [DataType(DataType.Text)]
      
        public string 装修进度 { get; set; }

        [Display(Name = "预计使用时间")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime? 预计使用时间 { get; set; }

        [Display(Name = "如何得知品牌")]
        [DataType(DataType.Text)]
       
        public string 如何得知品牌 { get; set; }

        [Display(Name = "比较品牌")]
        [DataType(DataType.Text)]
        
        public string 比较品牌 { get; set; }

        [Display(Name = "预算金额")]
        [DataType(DataType.Text)]
        
        public Nullable<decimal> 预算金额 { get; set; }

        [Display(Name = "预计报价折扣")]
        [DataType(DataType.Currency)]
      
        public string 预报价折扣 { get; set; }

        [Display(Name = "返点")]
        [DataType(DataType.Text)]
        
        public string 返点 { get; set; }

        [Display(Name = "安装地址")]
        [DataType(DataType.Text)]
        
        public string 安装地址 { get; set; }

        [Display(Name = "客户建议")]
        [DataType(DataType.Text)]
        
        public string 客户建议 { get; set; }

        [Display(Name = "是否有意向")]
        [DataType(DataType.Text)]
        public bool? 是否有意向 { get; set; }

        [Display(Name = "制单日期")]
        [DataType(DataType.DateTime)]
        public DateTime? 制单日期 { get; set; }

        [Display(Name = "最后更新人")]
        [DataType(DataType.Text)]
        public string 更新人 { get; set; }

        [Display(Name = "更新日期")]
        [DataType(DataType.DateTime)]
    
        public DateTime? 更新日期 { get; set; }

        public IEnumerable<CustomerExceptedBuyModel> CustomerExceptedBuyModels { set; get; }
        public virtual 销售_店铺档案 销售_店铺档案 { get; set; }
        public virtual 销售_店铺员工档案 销售_店铺员工档案 { get; set; }
        public virtual ICollection<销售_接待记录_意向明细> 销售_接待记录_意向明细 { get; set; }
    }
}