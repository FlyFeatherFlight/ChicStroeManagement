﻿using System;
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
        [DataType(DataType.Text)]
     
        public FileType Filetype { get; set; }
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
      public enum FileType
    {
        [Display(Name = "CAD图")]
        CAD图,
        [Display(Name = "效果3D图")]
        效果3D图,
        [Display(Name = "对比图")]
        对比图,
        [Display(Name = "PDF文件")]
        PDF文件,
        [Display(Name = "PPT文件")]
        PPT文件,
        [Display(Name = "Excel文件")]
        Excel文件,
        [Display(Name = "完成文件")]
        完成文件,
        [Display(Name = "其它")]
        其它   
    }
}