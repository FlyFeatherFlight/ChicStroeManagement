using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;
using PagedList;

namespace ChicStoreManagement.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IStoreEmployeesBLL storeEmployeesBLL;

        public ManagerController(IStoreEmployeesBLL storeEmployeesBLL)
        {
            this.storeEmployeesBLL = storeEmployeesBLL;
        }





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

        public ViewResult ManagerAction(string sortOrder, string searchString, string currentFilter, int? page)
        {
            //只提取前10 位
            //var list = storeEmployeesBLL.GetPagedList(1,10, p => true,p => true,true).ToList();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.Number = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.Name = sortOrder == "last" ? "last_desc" : "last";

            var workers = storeEmployeesBLL.GetModels(p => true);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;//获得前端传回来的搜索关键词

            if (!string.IsNullOrEmpty(searchString))
            {
                workers = workers.Where(w => w.姓名.Contains(searchString));//通过姓名查找
            }
            #region 排序，默认按ID升序
            switch (sortOrder)
            {
                case "first_desc":
                    workers = workers.OrderByDescending(w => w.ID);
                    break;
                case "last_desc":
                   workers = workers.OrderByDescending(w => w.姓名); 
                    break;
                case "last":
                   workers = workers.OrderBy(w => w.姓名); 
                    break;
                default:
                    workers = workers.OrderBy(w => w.ID);
                    break;
            }

            #endregion
            int pageSize = 10;
            int pageNumber = (page ?? 1);
          
            return View(workers.ToPagedList(pageNumber, pageSize));


        }
        public ActionResult EditEmploee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var worker = storeEmployeesBLL.GetModel(p=>p.ID==id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            return View(worker);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(销售_店铺员工档案 model)

        {
            
            if (ModelState.IsValid)
            {
                
                storeEmployeesBLL.Modify(model);
               
                return RedirectToAction("ManagerAction");
            }
            return View(model);
        }


        #region  目标管理
        public ActionResult StoreGoalView()
        {
            return View();
        }


        public ActionResult EditGoalView()
        {
            return View();
        }

        public ActionResult SplitGoalView()
        {

            return View();

        }

        public ActionResult DataStatisticsView()
        {

            return View();

        }
        #endregion
    }
}