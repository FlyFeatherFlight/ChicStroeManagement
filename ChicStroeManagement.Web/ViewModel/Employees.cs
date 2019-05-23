﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ChicStoreManagement.WEB.ViewModel
{
    /// <summary>
    /// 员工信息
    /// </summary>

    public class Employees
    {
        public int ID { get; set; }
        [Display(Name = "店铺")]
        [DataType(DataType.Text)]
        [Required]
        public string 店铺 { get; set; }
        public int 店铺ID { get; set; }
        [Display(Name = "编号")]
        [DataType(DataType.Text)]
        [Required]
        public string 编号 { get; set; }

        [Display(Name = "姓名")]
        [DataType(DataType.Text)]
        [Required]
        public string 姓名 { get; set; }

        [Display(Name = "性别")]
        [Required]
        public string 性别 { get; set; }

        [Display(Name = "职务")]
        [DataType(DataType.Text)]
        [Required]
        public string 职务 { get; set; }

        [Display(Name = "联系方式")]
        [DataType(DataType.PhoneNumber)]
        public string 联系方式 { get; set; }

        [Display(Name = "停用标志")]
        [Required]
        public bool 停用标志 { get; set; }

        [Display(Name = "制单人")]
        public int? 制单人 { get; set; }

        [Display(Name = "制单日期")]
        [DataType(DataType.DateTime)]
        public DateTime? 制单日期 { get; set; }

        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string 密码 { get; set; }

        [Display(Name = "跟进目标数")]
        [DataType(DataType.Text)]
        public int? 跟进目标数 { get; set; }


        public string 等级 { get; set; }
        public bool? 是否销售 { get; set; }
        public bool? 是否设计师 { get; set; }
        public bool? 是否店长 { get; set; }
        public List<Employees> ListEmployees { get; set; }
        public IQueryable<Employees> IQueryEmployees { get; set; }
        public int 职务ID { get;  set; }
        public int? 跟进目标计划数 { get;  set; }
        public string 网络地址 { get; set; }
    }


}