using System.Web.Mvc;

namespace ChicStoreManagement.Controllers
{

    public class WarningController : Controller
    {
        public ActionResult RepeatSubmit(string controllerName,string actionName) {
            ViewBag.ControllerName = controllerName;
            ViewBag.ActionName = actionName;
            string str = string.Format("<script>alert('重复操作！');location.href='@Url.Action('CustomerIndex','Home')';</script>");
            return Content(str);
        }
    }
}

