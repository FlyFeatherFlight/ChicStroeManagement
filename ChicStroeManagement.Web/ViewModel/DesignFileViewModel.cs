using System;
using System.ComponentModel.DataAnnotations;

namespace ChicStoreManagement.WEB.ViewModel
{
    public class DesignFileViewModel
    {
        
        public int Id { get; set; }
        [Display(Name= "设计案提交表序号")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage ="设计案序号不能为空")]
        public int 设计案提交表序号 { get; set; }
       
        [Display(Name = "设计案提交表序号")]
        [DataType(DataType.Text)]
        public string 备注 { get; set; }
    
        [Display(Name = "文件路径")]
        [DataType(DataType.Text)]
        public string Path { get; set; }
        [Display(Name ="文件类型")]
        [DataType(DataType.Text)]
        public string 文件类型 { get; set; }
        [Display(Name ="文件名")]
        public string 文件名 { get; set; }

        [Display(Name = "提交人")]
        [DataType(DataType.Text)]
        public string 提交人 { get; set; }
        [Display(Name = "设计案提交表序号")]
        [DataType(DataType.DateTime)]
        public DateTime? 提交时间 { get; set; }
        [Display(Name = "更新人")]
        [DataType(DataType.Text)]
        public string 更新人 { get; set; }
        [Display(Name = "更新日期")]
        [DataType(DataType.DateTime)]
        public DateTime? 更新日期 { get; set; }
        [Display(Name ="是否是完结效果")]
        [DataType(DataType.Text)]
        public bool? 完成效果 { get; set; }

    }
}