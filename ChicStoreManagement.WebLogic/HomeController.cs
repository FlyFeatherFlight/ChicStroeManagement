using ChicStoreManagement.BLL;
using ChicStoreManagement.Entity;
using ChicStoreManagement.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace ChicStoreManagement.WebLogic
{
    public class HomeController:Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult SignIn()
        {

            ViewBag.Message = "Your contact page.";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignIn(string userName ,  string password )
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
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("SignIn", "Home");

         
        }
    }
}
