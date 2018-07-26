using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicStroeManagement.Controllers
{
    public class DesignSchemeController : Controller
    {

        /// <summary>
        /// 设计方案
        /// </summary>
        /// <returns></returns>
        // GET: DesignScheme
        public ActionResult DesignSchemeIndex()
        {
            return View();
        }

        /// <summary>
        /// 添加设计方案
        /// </summary>
        /// <returns></returns>
        public ActionResult AddDesignView() {
            return View();
        }

        /// <summary>
        /// 申请设计
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplyDesignView() {

            return View();

        }
        /// <summary>
        /// 添加设计进度日志
        /// </summary>
        /// <returns></returns>
        public ActionResult AddDesignTrackLogView() {
            return View();
        }
    }
}