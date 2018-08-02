using System;
using System.ComponentModel.DataAnnotations;

namespace ChicStoreManagement.Model
{


    [MetadataType(typeof(SysFieldMetadata))]//使用SysFieldMetadata对SysField进行数据验证
    public partial class 销售_店铺员工档案
        {
        #region 自定义属性，即由数据实体扩展的实体

        [Display(Name = "父节点")]
        public string ParentIdOld { get; set; }

        #endregion
    }

    internal class SysFieldMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "主键", Order = 1)]
        public int ID { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "店铺ID", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        public int 店铺ID { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "编号", Order = 3)]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        [Required(ErrorMessage = "不能为空")]
        public string 编号 { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "姓名", Order = 4)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        public string 姓名 { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "性别", Order = 5)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(50, ErrorMessage = "长度不可超过10")]
        public string 性别 { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "职务ID", Order = 6)]
        [Required(ErrorMessage = "不能为空")]
        public int 职务ID { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "联系方式", Order = 7)]
        public string 联系方式 { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "停用标志", Order = 8)]
        [Required(ErrorMessage = "不能为空")]
        public bool 停用标志 { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "制单人", Order = 9)]
        
        public Nullable<int> 制单人 { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "制单日期", Order = 10)]
        public Nullable<System.DateTime> 制单日期 { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "密码", Order = 11)]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        public string 密码 { get; set; }

    }
}


