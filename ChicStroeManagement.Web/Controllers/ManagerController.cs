using ChicStoreManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChicStoreManagement.BLL;
using ChicStoreManagement.IBLL;

namespace ChicStoreManagement.Controllers
{
    public class ManagerController : Controller
    {

        IStoreEmployeesBLL storeEmployeeBLL = new StoreEmployeeBLL(); 
        // GET: Manager
        /// <summary>
        /// 本月交易情况
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 当前员工信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ManagerAction()
        {
            var result=storeEmployeeBLL.GetAll(c=>1==1);
            
            return View(result);

            
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