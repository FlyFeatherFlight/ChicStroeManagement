using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ChicStoreManagement.WEB.ViewModel
{

 
    public class Employees
    {
       

        public Employees()
        {
        }

        public Employees(int iD, string 店铺, string 编号, string 姓名, string 性别, string 职务, string 联系方式, bool 停用标志, int? 制单人, DateTime? 制单日期, string 密码)
        {
            ID = iD;
            this.店铺 = 店铺;
            this.编号 = 编号;
            this.姓名 = 姓名;
            this.性别 = 性别;
            this.职务 = 职务;
            this.联系方式 = 联系方式;
            this.停用标志 = 停用标志;
            this.制单人 = 制单人;
            this.制单日期 = 制单日期;
            this.密码 = 密码;
        }

        public int ID { get; set; }
        [Display(Name = "店铺")]
        [DataType(DataType.Text)]
        [Required]
        public string 店铺 { get; set; }

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
        public string 职务{ get; set; }

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

        public List<Employees> ListEmployees { get; set; }
        public IQueryable<Employees> IQueryEmployees { get; set; }
    }
}