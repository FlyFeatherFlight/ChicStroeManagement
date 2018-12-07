﻿using ChicStoreManagement.Model;
using System.ComponentModel.DataAnnotations;

namespace ChicStoreManagement.WEB.ViewModel
{
    public class CustomerExceptedBuyModel
    {
        public int ID { get; set; }

        [Display(Name = "接待记录id")]
        [DataType(DataType.Text)]
        [Required]
        public int 接待{ get; set; }

        [Display(Name = "备注")]
        [DataType(DataType.Text)]
   
        public string 备注 { get; set; }

        [Display(Name = "商品型号")]
        [DataType(DataType.Text)]
        [Required]
        public string  型号{ get; set; }
        [Display(Name = "空间")]
        [DataType(DataType.Text)]
        [Required]
        public string 空间 { get; set; }
       
        public virtual 销售_接待记录 销售_接待记录 { get; set; }
    }
}