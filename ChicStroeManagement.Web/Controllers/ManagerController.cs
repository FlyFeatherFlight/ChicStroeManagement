using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ChicStoreManagement.CustomAttributes;
using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;
using ChicStoreManagement.WEB.ViewModel;
using PagedList;

namespace ChicStoreManagement.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IStoreEmployeesBLL storeEmployeesBLL;
        private readonly IStoreBLL storeBLL;
        private readonly IPositionBLL positionBLL;
       private IQueryable<Employees> workers;


        public ManagerController(IStoreEmployeesBLL storeEmployeesBLL, IStoreBLL storeBLL, IPositionBLL positionBLL) 
        {
            this.storeEmployeesBLL = storeEmployeesBLL;
            this.storeBLL = storeBLL;
            this.positionBLL = positionBLL;
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
        /// 员工信息首页，员工信息查询
        /// </summary>
        /// <returns></returns>
       
        public ViewResult ManagerAction(string sortOrder, string searchString, string currentFilter, int? page)
        {
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Number = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.Name = sortOrder == "last" ? "last_desc" : "last";

            BuildEmploess();//将员工数据优化

          
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
            Session["Name"] = workers.FirstOrDefault();
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


        #region 添加员工信息
        /// <summary>
        /// 员工信息添加页面
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public ActionResult AddEmploee() {
            List<销售_职务> positionList = positionBLL.GetModels(p => true).ToList();
            SelectList PositionList = new SelectList(positionList, "职务", "职务");
            ViewBag.PositionList = PositionList;
            List<销售_店铺档案> storeList = storeBLL.GetModels(p => true).ToList();
            SelectList StoreList = new SelectList(storeList, "名称", "名称");
            ViewBag.StoreList = StoreList;
            return View();
        }

        /// <summary>
        /// 提交员工信息
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost]
      
        public ActionResult EmployeeAdd(Employees models) {

            销售_店铺员工档案 Emodel = new 销售_店铺员工档案
            {
                ID = models.ID,
                姓名=models.姓名,
                性别=models.性别,
                编号=models.编号,
                联系方式=models.联系方式,
                停用标志=models.停用标志,
                制单人=models.制单人,
                制单日期=models.制单日期,
                密码=models.密码,
                店铺ID = storeBLL.GetModel(p => p.名称 == models.店铺).ID,
                职务ID = positionBLL.GetModel(p => p.职务 == models.职务).ID
            };

           
            if (ModelState.IsValid)
            {

                storeEmployeesBLL.Add(Emodel);

                return RedirectToAction("ManagerAction");
            }
            return View();
          
        }
        #endregion

        #region 修改员工信息
        /// <summary>
        /// 修改页面，传入修改的对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditEmploee(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = storeEmployeesBLL.GetModel(p=>p.ID==id);

            Employees Emodel = new Employees
            {
                ID = model.ID,
                姓名 = model.姓名,
                
                性别 = model.性别,
                编号 = model.编号,
                店铺 = storeBLL.GetModel(p => p.ID == model.店铺ID).名称,
                职务 = positionBLL.GetModel(p => p.ID == model.职务ID).职务,
                联系方式 = model.联系方式,
                停用标志 = model.停用标志,
                制单人 = model.制单人,
                制单日期 = model.制单日期,
                密码 = model.密码
            };
            List<销售_职务> positionList = positionBLL.GetModels(p => true).ToList();       
                SelectList PositionList = new SelectList(positionList, "职务", "职务");
            ViewBag.PositionList = PositionList;
            List<销售_店铺档案> storeList = storeBLL.GetModels(p => true).ToList();
            SelectList StoreList = new SelectList(storeList, "名称", "名称");
            ViewBag.StoreList = StoreList;
            if (model == null)
            {
                return HttpNotFound();
            }
            
            return View(Emodel);
        }
        /// <summary>
        /// 提交修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeEdit(Employees model)

        {
         
            var Emodel=storeEmployeesBLL.GetModels(p => p.ID == model.ID);
            Emodel.First().店铺ID = storeBLL.GetModel(p => p.名称 == model.店铺).ID;
            Emodel.First().职务ID = positionBLL.GetModel(p => p.职务 == model.职务).ID;
            if (ModelState.IsValid)
            {
                
                storeEmployeesBLL.Modify(Emodel.First());
               
                return RedirectToAction("ManagerAction");
            }
            return View(model);
        }

        #endregion

        #region 删除员工信息
        public ActionResult DelEmploee(int id) {

            try
            {
                storeEmployeesBLL.DelBy(p=>p.ID==id);
            }
            catch (DataException )
            {
                return RedirectToAction("ManageAction", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("ManageAction");
        }
        #endregion


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

        /// <summary>
        /// 将员工数据优化
        /// </summary>
        public void BuildEmploess() {
            List<Employees> employeesList = new List<Employees>();

            if (workers == null)
            {

                var worker = storeEmployeesBLL.GetModels(p => true);//查询初始员工信息
                #region 对员工信息进行数据优化


                foreach (var item in worker)
                {
                    Employees employees = new Employees
                    {
                        ID = item.ID,
                        停用标志 = item.停用标志,
                        制单人 = item.制单人,
                        制单日期 = item.制单日期,
                        姓名 = item.姓名,
                        密码 = item.密码,
                        性别 = item.性别,
                        编号 = item.编号,
                        职务 = positionBLL.GetModel(p => p.ID == item.职务ID).职务,
                        店铺 = storeBLL.GetModel(p => p.ID == item.店铺ID).名称,
                        联系方式 = item.联系方式
                    };
                    employeesList.Add(employees);
                }
                #endregion
                workers = employeesList.AsEnumerable().AsQueryable();
            }
        }

        /// <summary>
        /// 获得职位
        /// </summary>
        /// <returns></returns>
        public List<String> GetPosition( ){

            var positionList = positionBLL.GetListBy(p => true);
            List<String> lis = new List<string>();
            foreach (var item in positionList)
            {
                lis.Add(item.职务);

            }
            return lis;
        }

        /// <summary>
        /// 获得所有店铺名字
        /// </summary>
        /// <returns></returns>
        public List<String> GetStore (){

            var storeList = storeBLL.GetListBy(p => true);
            List<String> lis = new List<string>();
            foreach (var item in storeList)
            {
                lis.Add(item.名称);

            }
            return lis;

        }
    }
}