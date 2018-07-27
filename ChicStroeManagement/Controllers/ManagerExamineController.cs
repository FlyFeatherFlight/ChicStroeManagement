using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicStroeManagement.Controllers
{

    /// <summary>
    /// 管理员审查
    /// </summary>
    public class ManagerExamineController : Controller
    {
        // GET: ManagerExamine
        /// <summary>
        /// 显示意向客户名录
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 客户追踪日志审查
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerTrackLogExamineView() {
            return View();
        }

        /// <summary>
        /// 设计跟踪日志审查
        /// </summary>
        /// <returns></returns>
        public ActionResult DesignTrackLogExamineView()
        {
            return View();
        }
        /// <summary>
        /// 诚意/意向客户确认
        /// </summary>
        /// <returns></returns>
        public ActionResult SincerityCustomerConfirmView() {

            return View();

        }

    }
}