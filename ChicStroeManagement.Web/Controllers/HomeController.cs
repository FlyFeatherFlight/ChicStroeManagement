using ChicStoreManagement.BLL;
using ChicStoreManagement.Model;
using System.Web.Mvc;
using System.Web.Security;
using ChicStoreManagement.CustomAttributes;

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
