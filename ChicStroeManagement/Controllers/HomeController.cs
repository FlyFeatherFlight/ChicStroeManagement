using ChicStoreManagement.BLL;
using ChicStoreManagement.Entity;
using System.Web.Mvc;
using System.Web.Security;
using ChicStroeManagement.CustomAttributes;

namespace ChicStroeManagement.Controllers
{
     [BasicAuthAttribute]
    public class HomeController: Controller
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


    }
}
