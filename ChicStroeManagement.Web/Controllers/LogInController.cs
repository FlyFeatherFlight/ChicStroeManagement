using ChicStoreManagement.BLL;
using ChicStoreManagement.Model;
using ChicStoreManagement.CustomAttributes;
using ChicStoreManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ChicStoreManagement.WEB.ViewModel;
using ChicStoreManagement.IBLL;

namespace ChicStoreManagement.Controllers
{
    [AllowAnonymous]
    public class LogInController : Controller
    {

        private readonly IStoreEmployeesBLL storeEmployeesBLL;
        private readonly IStoreBLL storeBLL;
        private readonly IPositionBLL positionBLL;
        private IQueryable<Employees> workers;

        public LogInController(IStoreEmployeesBLL storeEmployeesBLL, IStoreBLL storeBLL, IPositionBLL positionBLL)
        {
            this.storeEmployeesBLL = storeEmployeesBLL;
            this.storeBLL = storeBLL;
            this.positionBLL = positionBLL;
            BuildEmploess();
        }


        // GET: Account

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult SignIn()
        {

            ViewBag.Message = "Your contact page.";
            return View();
        }




        public ActionResult Viewtest()
        {
            return View();
        }

        


        /// <summary>
        /// 登录操作
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        [HttpPost]
 
        public ActionResult Login(Employees employees)
        {
            employees.IQueryEmployees = workers;
            if (workers.FirstOrDefault(p=>p.姓名==employees.姓名)!=null&& workers.FirstOrDefault(p => p.姓名 == employees.姓名).密码==employees.密码)

            {
                //Session["CurrentUser"] = Database.Users.Find(u => u.Name == user);
                FormsAuthentication.SetAuthCookie(employees.姓名, false);
                HttpCookie aCookie = new HttpCookie("userName")
                {
                    Value = employees.姓名,
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Add(aCookie);
                Session["Employee"] = workers.First(p => p.姓名 == employees.姓名);
                return RedirectToAction("Index", "Home");

            }

            else

            {

                TempData["msg"] = "用户名或密码错误！";

                return RedirectToAction("SignIn", "LogIn");

            }
        }




      
         /// <summary>
         /// 注销
         /// </summary>
        public void  SignOut()
        {
            FormsAuthentication.SignOut();

            FormsAuthentication.RedirectToLoginPage();

           
        }


        /// <summary>
        /// 将员工数据优化
        /// </summary>
        public void BuildEmploess()
        {
            List<Employees> employeesList = new List<Employees>();

            if (workers == null)
            {

                var worker = storeEmployeesBLL.GetModels(p => true);//查询初始员工信息
                #region 对员工信息进行数据优化


                foreach (var item in worker)
                {
                    Employees employees = new Employees
                    {
                        ID = item.ID,
                        停用标志 = item.停用标志,
                        制单人 = item.制单人,
                        制单日期 = item.制单日期,
                        姓名 = item.姓名,
                        密码 = item.密码,
                        性别 = item.性别,
                        编号 = item.编号,
                        职务 = positionBLL.GetModel(p => p.ID == item.职务ID).职务,
                        店铺 = storeBLL.GetModel(p => p.ID == item.店铺ID).名称,
                        联系方式 = item.联系方式
                    };
                    employeesList.Add(employees);
                }
                #endregion
                workers = employeesList.AsEnumerable().AsQueryable();
            }
        }


    }


}