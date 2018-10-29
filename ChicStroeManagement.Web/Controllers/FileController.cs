
using System.Web.Mvc;

namespace ChicStoreManagement.WEB.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UploadFileView() {
            return View();
        }
    }
}