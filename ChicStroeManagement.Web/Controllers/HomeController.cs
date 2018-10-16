using ChicStoreManagement.BLL;
using ChicStoreManagement.Model;
using System.Web.Mvc;
using System.Web.Security;
using ChicStoreManagement.CustomAttributes;
using ChicStoreManagement.WEB.ViewModel;

namespace ChicStoreManagement.Controllers
{
    
    public class HomeController: Controller
    {
      

        /// <summary>
        /// 门店管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string userName = HttpContext.User.Identity.Name;
            if (userName != null)
            {
                var employees = HttpContext.Session["Employee"] as Employees;

                ViewBag.StoreName = employees.店铺;
                ViewBag.Position = employees.职务;
            }
           
            return View();
        }

        /// <summary>
        /// 公司简介
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        /// <summary>
        /// 公司联系方式
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}
