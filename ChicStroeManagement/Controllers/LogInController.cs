using ChicStoreManagement.BLL;
using ChicStoreManagement.Entity;
using ChicStroeManagement.CustomAttributes;
using ChicStroeManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ChicStroeManagement.Controllers
{

    public class LogInController : Controller
    {

     

        public LogInController()
        {
           

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
        public ActionResult Login(string userName, string password)
        {
            AccountEntity account = new AccountEntity { Name = userName, Password = password };
            if (new AccountManageBll().LoginCheck(account))

            {

                FormsAuthentication.SetAuthCookie(account.Name, true);
               
                return RedirectToAction("Index", "Home");

            }

            else

            {

                ViewBag.msg = "用户名或密码错误！.";

                return RedirectToAction("SigIn", "Account");

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


       
   
    }


}