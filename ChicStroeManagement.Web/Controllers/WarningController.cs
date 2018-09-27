using System.Web.Mvc;

namespace ChicStoreManagement.Controllers
{

    public class WarningController : Controller
    {
        public ActionResult RepeatSubmit(string controllerName,string actionName) {
            ViewBag.ControllerName = controllerName;
            ViewBag.ActionName = actionName;
            string str = string.Format("<script>alert('重复操作！');parent.location.href='Index';</script>");
            return Content(str);
        }
    }
}

