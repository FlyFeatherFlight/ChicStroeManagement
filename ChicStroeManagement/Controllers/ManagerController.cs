using ChicStroeManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicStroeManagement.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        /// <summary>
        /// 本月交易情况
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManagerAction()
        {


            return View();
        }


        #region  目标管理
        public ActionResult StoreGoalView() {
            return View();
        }


        public ActionResult EditGoalView() {
            return View();
        }

        public ActionResult SplitGoalView() {

            return View();

        }

        public ActionResult DataStatisticsView() {

            return View();

        }
        #endregion
    }
}