using ChicStoreManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChicStoreManagement.BLL;
using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;
using ChicStoreManagement.Model.ViewModel;

namespace ChicStoreManagement.Controllers
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

        /// <summary>
        /// 当前员工信息
        /// </summary>
        /// <returns></returns>
        
        public ActionResult ManagerAction()
        {
           
            return View();

            
        }
        public JsonResult getData() {
            StoreEmployeeBLL storeEmployeeBLL = new StoreEmployeeBLL();
            var rows = storeEmployeeBLL.GetAll("销售_店铺员工档案");
          
            if (rows.Any())
            {

                //是否可以省
                return Json(new
                {
                    total = rows.Count(),
                    rows = rows.OrderBy(o => o.ID)
                }, JsonRequestBehavior.AllowGet);


            }

            return null ;
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