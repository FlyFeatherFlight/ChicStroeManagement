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

    public class AccountController : Controller
    {

        DataTable dt = new DataTable("test");

        public AccountController()
        {
             MakeData();
            

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


        public ActionResult ManagerAction() {
            
            
            return View(dt);
        }

        [HttpPost]
        public ActionResult ChangeVisitInfo(List<string> AccountName,List<string> CustomerName,List<string> StartTime,List<string> VisitWay, List<string> VisitResult,List<string> ManagerTips) {

            for (int i=0;i<AccountName.Count;i++) {
                VisitInfoModel vim = new VisitInfoModel();
                vim.AccountName = AccountName[i];
                vim.CustomerName = CustomerName[i];
                vim.StartTime = DateTime.Parse( StartTime[i].ToString());
                vim.VisitWay = VisitWay[i];
                vim.VisitResult = VisitResult[i];
                vim.ManagerTips = ManagerTips[i];
            }
            return RedirectToAction("Index", "Home");
        }

        public bool VisitInfoAdd(VisitInfoModel vim) {
            dt = new DataTable();
            dt.Clear();
            dt.Rows.Add(vim);
            return true;
        }
        public ActionResult ShowVisitInfo()
        {
            ViewData["TestData"] = dt;
            return View();

        }
        public DataTable MakeData() {
            
            dt.Columns.Add("1", typeof(String));
            dt.Columns.Add("2", typeof(String));
            dt.Columns.Add("3", typeof(String));
            dt.Columns.Add("4", typeof(String));
            dt.Columns.Add("5", typeof(String));
            dt.Columns.Add("6", typeof(String));
            dt.Rows.Add("1", "2", "3", "4", "5", "6");
            dt.Rows.Add("1", "2", "3", "4", "5", "6");
            dt.Rows.Add("1", "2", "3", "4", "5", "6");
            dt.Rows.Add("1", "2", "3", "4", "5", "6");
            dt.Rows.Add("1", "2", "3", "4", "5", "6");
            return dt;

        }
    }


}