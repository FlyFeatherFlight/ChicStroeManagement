using ChicStoreManagement.BLL;
using ChicStoreManagement.Entity;
using ChicStroeManagement.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ChicStroeManagement.Controllers
{

    public class AccountController : Controller
    {

        // GET: Account
        
        public ActionResult SignIn()
        {

            ViewBag.Message = "Your contact page.";
            return View();
        }

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

                ViewBag.msg = "LogID or Password error.";

                return View();

            }
        }




        public ActionResult SignUp()
        {
            ViewBag.Message = "Please Sign Up.";

            return View();
        }

        public ActionResult Register(string userName, string password)
        {
            AccountEntity account = new AccountEntity { Name = userName, Password = password };
           // if (new AccountManageBll().LoginCheck(account))
                return RedirectToAction("Index", "Home");
        }
        public void  SignOut()
        {
            FormsAuthentication.SignOut();

            FormsAuthentication.RedirectToLoginPage();

           
        }
    }
}