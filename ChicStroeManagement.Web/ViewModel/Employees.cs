using System;
using System.ComponentModel.DataAnnotations;
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

        [DataType(DataType.Text)]
        [Required]
        public string 店铺 { get; set; }

        [DataType(DataType.Text)]
        [Required]
        public string 编号 { get; set; }

        [DataType(DataType.Text)]
        [Required]
        public string 姓名 { get; set; }

      
        [Required]
        public string 性别 { get; set; }

        [DataType(DataType.Text)]
        [Required]
        public string 职务{ get; set; }

        public string 联系方式 { get; set; }

        [Required]
        public bool 停用标志 { get; set; }

        public Nullable<int> 制单人 { get; set; }

        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> 制单日期 { get; set; }

        [DataType(DataType.Text)]
        public string 密码 { get; set; }

     
    }
}