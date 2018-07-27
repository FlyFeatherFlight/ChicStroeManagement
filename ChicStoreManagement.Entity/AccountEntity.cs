using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicStoreManagement.Model
{
    public class AccountEntity

    {

        [Required(ErrorMessage = "LogID cann't be empty!")] //Required 验证

        public string Name { get; set; }



        [Required(ErrorMessage = "Password cann't be empty!")]   //Required 验证

        public string Password { get; set; }



        public AccountEntity() { } //无参构造函数

        public AccountEntity(string ID, string Pwd)  //有参构造函数 为了测试数据

        {

            Name = ID;

            Password = Pwd;

        }

    }
}
