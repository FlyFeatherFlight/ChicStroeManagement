using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChicStoreManagement.Common;
using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.Controllers
{
    public class TestController : Controller

    {
        private readonly IStoreEmployeesBLL storeEmployeesBLL;

        public TestController(IStoreEmployeesBLL storeEmployeesBLL)
        {
            this.storeEmployeesBLL = storeEmployeesBLL;
        }

        public ActionResult Index()
        {
            List < 销售_店铺员工档案 > list= storeEmployeesBLL.GetModels(p => true).ToList();
            return View(list);
        }
        public ActionResult Add(销售_店铺员工档案 staff)
        {
            if (storeEmployeesBLL.Add(staff)!=0)
            {
                return Redirect("Index");
            }
            else
            {
                return Content("no");
            }
        }
        public ActionResult Update(销售_店铺员工档案 staff)
        {
            var Name= Request["姓名"];
            var Sex = Request["性别"];
            string[] property={ Name, Sex };
            if (storeEmployeesBLL.Modify(staff,property)!=0)
            {
                return Redirect("Index");
            }
            else
            {
                return Content("no");
            }
        }
        public ActionResult Delete(int Id)
        {
            var staff = storeEmployeesBLL.GetModels(p => p.ID== Id).FirstOrDefault();
            if (storeEmployeesBLL.Del(staff)!=0)
            {
                return Redirect("Index");
            }
            else
            {
                return Content("no");
            }
        }
    }


}