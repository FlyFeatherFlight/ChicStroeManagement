using ChicStoreManagement.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicStoreManagement.WEB.ViewModel
{
    public class Employees
    {
        private readonly IStoreEmployeesBLL storeEmployeesBLL;

        public Employees(IStoreEmployeesBLL storeEmployeesBLL)
        {
            this.storeEmployeesBLL = storeEmployeesBLL;
            SetData();
        }

        private void SetData()
        {
           var model= storeEmployeesBLL.GetModel(p => p.ID == this.ID);
            this.店铺=
        }

        public int ID { get; set; }
        public string 店铺 { get; set; }
        public string 编号 { get; set; }
        public string 姓名 { get; set; }
        public string 性别 { get; set; }
        public string 职务{ get; set; }
        public string 联系方式 { get; set; }
        public bool 停用标志 { get; set; }
        public Nullable<int> 制单人 { get; set; }
        public Nullable<System.DateTime> 制单日期 { get; set; }
        public string 密码 { get; set; }

     
    }
}