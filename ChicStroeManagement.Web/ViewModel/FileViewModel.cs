using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChicStoreManagement.WEB.ViewModel
{
    public class FileViewModel
    {
        [Display(Name ="文件路径")]
        public string Path { get; set; }
        [Display(Name ="文件名称")]
        public string FileName { get; set; }
        [Display(Name ="文件类型")]
        [Required(ErrorMessage ="选择正确的文件类型")]
        public FileType FileType { get; set; }
        [Display(Name ="店铺名称")]
        public string StoreName { get; set; }
        [Display(Name ="使用名称")]
        public string UserName { get; set; }
        [Display(Name ="文件大小")]
        public long FileSize { get; set; }
        [Display(Name ="提交时间")]
        public DateTime SubmitDate { get; set; }
        [Display(Name = "文件")]
        [Required(ErrorMessage = "请上传你的文件！")]
        public HttpPostedFileBase UploadStream { get; set; }

        [DataType(DataType.Text)]
        [Required]
        public int DesignId { get; set; }
    }
    public enum FileType {
        CAD图=0,
        效果3D图=1,
        对比图=2,
        PDF文件=3,
        PPT文件=4,
        Excel文件=5,
        其它=6

    }
}