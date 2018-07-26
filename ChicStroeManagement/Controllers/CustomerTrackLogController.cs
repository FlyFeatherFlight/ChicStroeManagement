using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicStroeManagement.Controllers
{
    /// <summary>
    /// 客户跟进日志 
    /// </summary>
    public class CustomerTrackLogController : Controller
    {
        // GET: CustomerTrackLog
        /// <summary>
        /// 跟进日志展示
        /// </summary>
        /// <returns></returns>
        public ActionResult TrackLogIndex()
        {
            return View();
        }

        /// <summary>
        /// 添加跟进日志
        /// </summary>
        /// <returns></returns>
        public ActionResult AddTrackLogView() {

            return View();
        }
        /// <summary>
        /// 修改跟进日志
        /// </summary>
        /// <returns></returns>
        public ActionResult EditTrackLogView() {

            return View();
        }
    }
}