using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;
using ChicStoreManagement.WEB.ViewModel;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ChicStoreManagement.Controllers
{

    /// <summary>
    /// 管理员审查
    /// </summary>
    public class ManagerExamineController : Controller
    {
        private readonly ICustomerInfoBLL customerInfoBLL;
        private readonly IExceptedBuyBLL exceptedBuyBLL;
        private readonly IStoreEmployeesBLL storeEmployeesBLL;
        private readonly IStoreBLL storeBLL;
        private readonly IProductCodeBLL productCodeBLL;
        private readonly ICustomerTrackingBLL customerTrackingBLL;
        private readonly IPositionBLL positionBLL;
        private readonly IDesignSubmitBLL designSubmitBLL;
        private readonly IDesignTrackingLogBLL designTrackingLogBLL;
        private readonly IDesign_ProjectDrawingsBLL design_ProjectDrawingsBLL;
        private readonly IDesign_CustomerExceptedBuyBLL design_CustomerExceptedBuyBLL;
        private IDesignResultBLL designResultBLL;
        private IDesignResult_DealListingBLL designResult_DealListingBLL;
        private ISalesOrderBLL salesOrderBLL;
        private ISalesOrder_detailsBLL salesOrder_DetailsBLL;
        private IProduct_SPUBLL product_SPUBLL;
        private IProduct_SKUBLL product_SKUBLL;
        private IProductBLL productBLL;

        private int employeeID;//员工id
        private string employeeName;//员工姓名
        private string store;//当前店铺名称
        private int storeID;//当前店铺id
        private int positionID;

        public ManagerExamineController(ICustomerInfoBLL customerInfoBLL, IExceptedBuyBLL exceptedBuyBLL, IStoreEmployeesBLL storeEmployeesBLL, IStoreBLL storeBLL, IProductCodeBLL productCodeBLL, ICustomerTrackingBLL customerTrackingBLL, IPositionBLL positionBLL, IDesignSubmitBLL designSubmitBLL, IDesignTrackingLogBLL designTrackingLogBLL, IDesign_ProjectDrawingsBLL design_ProjectDrawingsBLL, IDesign_CustomerExceptedBuyBLL design_CustomerExceptedBuyBLL, IDesignResultBLL designResultBLL, IDesignResult_DealListingBLL designResult_DealListingBLL, ISalesOrderBLL salesOrderBLL, ISalesOrder_detailsBLL salesOrder_DetailsBLL, IProduct_SPUBLL product_SPUBLL, IProduct_SKUBLL product_SKUBLL, IProductBLL productBLL)
        {
            this.customerInfoBLL = customerInfoBLL;
            this.exceptedBuyBLL = exceptedBuyBLL;
            this.storeEmployeesBLL = storeEmployeesBLL;
            this.storeBLL = storeBLL;
            this.productCodeBLL = productCodeBLL;
            this.customerTrackingBLL = customerTrackingBLL;
            this.positionBLL = positionBLL;
            this.designSubmitBLL = designSubmitBLL;
            this.designTrackingLogBLL = designTrackingLogBLL;
            this.design_ProjectDrawingsBLL = design_ProjectDrawingsBLL;
            this.design_CustomerExceptedBuyBLL = design_CustomerExceptedBuyBLL;
            this.designResultBLL = designResultBLL;
            this.designResult_DealListingBLL = designResult_DealListingBLL;
            this.salesOrderBLL = salesOrderBLL;
            this.salesOrder_DetailsBLL = salesOrder_DetailsBLL;
            this.product_SPUBLL = product_SPUBLL;
            this.product_SKUBLL = product_SKUBLL;
            this.productBLL = productBLL;
        }

        // private IQueryable<Employees> workers;//所有员工信息
        //private IQueryable<CustomerInfoModel> customerInfoModels;//所有接待信息
        //private IQueryable<CustomerExceptedBuyModel> exceptedBuyModels;//预计购买




        // GET: ManagerExamine
        /// <summary>
        ///店长操作首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        #region 客户审查操作
        /// <summary>
        /// 客户信息审查
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult CustomerExamineView(string sortOrder, string searchString, string currentFilter, int? page) {
            List<CustomerInfoModel> customerInfoModels = new List<CustomerInfoModel>();
            Session["method"] = "N";
            SetEmployee();//获取当前人员信息
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            ViewBag.CustomerExamineCurrentSort = sortOrder;
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            ViewBag.CustomerExamineTrackingResult = String.IsNullOrEmpty(sortOrder) ? "last_desc" : "";
            ViewBag.CustomerExamineID = String.IsNullOrEmpty(sortOrder) ? "first" : "first_desc";
            if (BuildCustomerInfo()!=null)
            {
                customerInfoModels = BuildCustomerInfo().ToList();
            }
            ViewBag.CustomerExaminePeopleName = employeeName;//将当前操作人员传到前端
            ViewBag.storeName = store;//将当前店铺名字传到前端
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.TrackingCurrentFilter = searchString;//获得前端传回来的搜索关键词

            if (!string.IsNullOrEmpty(searchString))
            {
                customerInfoModels = customerInfoModels.Where(w => w.客户电话 == searchString).ToList();//通过客户电话查找
            }
            //Session["Name"] = customerInfoModels.FirstOrDefault();
            #region 排序，默认按ID升序
            switch (sortOrder)
            {
                case "last_desc":
                    customerInfoModels = customerInfoModels.OrderByDescending(w => w.跟进人).ThenBy(w => w.ID).ToList();
                    break;
                case "first":
                    customerInfoModels = customerInfoModels.OrderBy(w => w.ID).ToList();
                    break;
                case "first_desc":
                    customerInfoModels = customerInfoModels.OrderByDescending(w => w.ID).ToList();
                    break;
                default:
                    customerInfoModels = customerInfoModels.OrderBy(w => w.跟进人).ThenBy(w => w.ID).ToList();
                    break;
            }

            #endregion
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.CustomerExamineManager  = storeEmployeesBLL.GetModel(p => p.姓名 == employeeName).是否店长;//给前端传入当前操作人职位
            return View(customerInfoModels.ToPagedList(pageNumber, pageSize));


        }

        /// <summary>
        /// 审查客户档案，指定跟进人
        /// </summary>
        /// <param name="id">客户接待ID</param>
        /// <returns></returns>
        public ActionResult CustomerTrackCustomerEditView(int id) {

            Session["method"] = "N";
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SetEmployee();
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            CustomerInfoModel customerInfo = new CustomerInfoModel();
            if (BuildCustomerInfo()!=null)
            {
               customerInfo = BuildCustomerInfo().First(p => p.ID == id);
            }
           

            if (customerInfo.接待人 == employeeName || customerInfo.跟进人 == employeeName || storeEmployeesBLL.GetModel(p => p.姓名 == employeeName).职务ID == 3)
            {
                @ViewBag.TrackPeople = customerInfo.客户姓名;
                customerInfo.更新人 = employeeName;
                customerInfo.更新日期 = DateTime.Now;
                ViewBag.Position = storeEmployeesBLL.GetModel(p => p.姓名 == employeeName).职务ID;
                var storeEmployees = storeEmployeesBLL.GetModels(p => p.店铺ID == storeID).ToList();

                SelectList EmployeeListList = new SelectList(storeEmployees, "ID", "姓名");
                ViewBag.ExamineEmployeeOptions = EmployeeListList;
                return View(customerInfo);
            }

            return Content("<script>alert('无操作权限！！');window.history.go(-1);</script>");


        }

        /// <summary>
        /// 指定跟进人
        /// </summary>
        /// <param name="id">客户接待ID</param>
        /// <param name="跟进人">跟进人名</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomerTrackCustomerEdit(int id, string 跟进人) {
            SetEmployee();
            if (positionID != 3)
            {
                return Content("违规操作！");
            }
            string[] property = new string[1];
            property[0] = "跟进人ID";
            var model = customerInfoBLL.GetModel(p => p.ID == id);
            try
            {
                int eid = int.Parse(跟进人);
                model.跟进人ID = eid;
            }
            catch (Exception e)
            {

                return Content(""+e.Message);
            }
            
            Session["method"] = "Y";
            customerInfoBLL.Modify(model, property);
            return RedirectToAction("CustomerExamineView");
        }
        /// <summary>
        /// 客户追踪日志审查
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerTrackLogExamineView(int? id, string sortOrder, string PhoneSearchString, string NameSearchString, string currentFilter, int? page) {
            List<CustomerTrackingModel> customerTrackingModels = new List<CustomerTrackingModel>();
            Session["method"] = "N";
            SetEmployee();//获取当前人员信息
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            ViewBag.TrackingCurrentSort = sortOrder;
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            ViewBag.TrackingDate = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.TrackingResult = sortOrder == "last" ? "last_desc" : "last";
            if (id != null && id != 0)
            {
                if (BuildTrackingInfo(id, employeeID)!=null)
                {
                customerTrackingModels = BuildTrackingInfo(id, employeeID);//获取当前人员可查看的跟进信息
                ViewBag.Reception = customerInfoBLL.GetModel(p => p.ID == id).接待序号;//将接待序号传到前端
                }
                
            }
            else
            {
                if (BuildTrackingInfo(0, employeeID)!=null)
                {
                customerTrackingModels = BuildTrackingInfo(0, employeeID);
                }
               
            }
            if (customerTrackingModels == null)
            {
                return Content("当前操作人并无关联的跟进信息或无进入权限！");
            }
            
            ViewBag.trackingPeopleName = employeeName;//将当前操作人员传到前端
            ViewBag.storeName = store;//将当前店铺名字传到前端
            string searchString = null;
            if (PhoneSearchString != null)
            {
                searchString = PhoneSearchString;
            }
            if (NameSearchString != null)
            {
                searchString = NameSearchString;
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.TrackingCurrentFilter = searchString;//获得前端传回来的搜索关键词

            if (!string.IsNullOrEmpty(searchString) && PhoneSearchString != null)
            {
                customerTrackingModels = customerTrackingModels.Where(w => w.客户电话 == searchString && w.店铺 == store).ToList();//通过客户电话查找
            }
            if (!string.IsNullOrEmpty(searchString) && NameSearchString != null)
            {
                customerTrackingModels = customerTrackingModels.Where(w => w.跟进人 == searchString && w.店铺 == store).ToList();//通过跟进人查找
            }
            //Session["Name"] = customerInfoModels.FirstOrDefault();
            #region 排序，默认按ID升序
            switch (sortOrder)
            {
                case "first_desc":
                    customerTrackingModels = customerTrackingModels.OrderByDescending(w => w.跟进时间).ToList();
                    break;
                case "last_desc":
                    customerTrackingModels = customerTrackingModels.OrderByDescending(w => w.跟进结果).ToList();
                    break;
                case "last":
                    customerTrackingModels = customerTrackingModels.OrderBy(w => w.跟进结果).ToList();
                    break;
                default:
                    customerTrackingModels = customerTrackingModels.OrderBy(w => w.跟进时间).ToList();
                    break;
            }

            #endregion
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            Thread.Sleep(3000);
            return View(customerTrackingModels.ToPagedList(pageNumber, pageSize));

        }

        /// <summary>
        /// 客户追踪日志审查。添加审查状态
        /// </summary>
        /// <param name="id">追踪日志ID</param>
        /// <param name="examineType">审查状态</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomerTrackLogExamine(int id, string examineType) {

            var model = customerTrackingBLL.GetModel(p => p.id == id);
            var s = JsonConvert.DeserializeObject<String>(examineType);
            model.店长审核 = s;
            string[] property = new string[1];
            property[0] = "店长审核";
            try
            {
                customerTrackingBLL.Modify(model, property);
                Session["method"] = "Y";
            }
            catch (Exception e)
            {

                return Json(e.Message);
            }

            return Json(model.店长审核);
        }



        #endregion


        #region 设计案审查操作
        /// <summary>
        /// 设计跟踪日志审查
        /// </summary>
        /// <returns></returns>
        public ActionResult DesignTrackLogExamineView(string sortOrder, string searchString, string currentFilter, int? page)
        {
           
            Session["method"] = "N";
            SetEmployee();//获取当前人员信息
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            ViewBag.DesignTrackExamineCurrentSort = sortOrder;
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            ViewBag.DesignTrackExamineTrackingResult = String.IsNullOrEmpty(sortOrder) ? "last_desc" : "";
            ViewBag.DesignTrackExamineID = String.IsNullOrEmpty(sortOrder) ? "first" : "first_desc";
            List<DesignTackLogViewModel> DesignTrackInfoModels = new List<DesignTackLogViewModel>();
            if (BuildDesignTrackLogInfo(null)!=null)
            {
            DesignTrackInfoModels = BuildDesignTrackLogInfo(null).ToList();
            }
            

            ViewBag.DesignTrackExaminePeopleName = employeeName;//将当前操作人员传到前端
            ViewBag.storeName = store;//将当前店铺名字传到前端
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.DesignTrackCurrentFilter = searchString;//获得前端传回来的搜索关键词

            if (!string.IsNullOrEmpty(searchString))
            {
                DesignTrackInfoModels = DesignTrackInfoModels.Where(w => w.联系方式 == searchString).ToList();//通过客户电话查找
            }
            //Session["Name"] = customerInfoModels.FirstOrDefault();
            #region 排序，默认按ID升序
            switch (sortOrder)
            {
                case "last_desc":
                    DesignTrackInfoModels = DesignTrackInfoModels.OrderByDescending(w => w.店长审查).ThenBy(w => w.Id).ToList();
                    break;
                case "first":
                    DesignTrackInfoModels = DesignTrackInfoModels.OrderBy(w => w.Id).ToList();
                    break;
                case "first_desc":
                    DesignTrackInfoModels = DesignTrackInfoModels.OrderByDescending(w => w.Id).ToList();
                    break;
                default:
                    DesignTrackInfoModels = DesignTrackInfoModels.OrderBy(w => w.店长审查).ThenBy(w => w.Id).ToList();
                    break;
            }

            #endregion
            int pageSize = 10;
            int pageNumber = (page ?? 1);
           
            return View(DesignTrackInfoModels.ToPagedList(pageNumber, pageSize));




        }

        /// <summary>
        /// 设计跟踪日志审查
        /// </summary>
        /// <returns></returns>
       [HttpPost]
        public ActionResult DesignTrackExamine(int id, string examineType) {
            Session["method"] = "Y";
            try
            {
                var model = designTrackingLogBLL.GetModel(p => p.id == id);
                var str = JsonConvert.DeserializeObject<String>(examineType);
                if (str == "true")
                {
                    model.店长审查 = true;
                }
                else if (str == "false")
                {
                    model.店长审查 = false;
                }
                else
                {
                    return Json("非法数据传输！操作失败");
                }
                model.更新人 = employeeName;
                model.更新日期 = DateTime.Now;
                designTrackingLogBLL.Modify(model);
            }
            catch (Exception e)
            {

                return Json("设计案跟进日志操作异常，操作失败！");
            }
           
            return Json("提交成功！");
        }

        /// <summary>
        /// 设计案申请审查
        /// </summary>
        /// <returns></returns>
        public ActionResult DesignApplyExamineView(string sortOrder, string searchString, string currentFilter, int? page) {
            List<DesignSubmitModel> customersubModels = new List<DesignSubmitModel>();
            Session["method"] = "N";
            SetEmployee();//获取当前人员信息
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            ViewBag.DesignApplyrExamineCurrentSort = sortOrder;
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            ViewBag.DesignApplyExamineTrackingResult = String.IsNullOrEmpty(sortOrder) ? "last_desc" : "";
            ViewBag.DesignApplyExamineID = String.IsNullOrEmpty(sortOrder) ? "first" : "first_desc";
            List<DesignSubmitModel> DesignApplyInfoModels = new List<DesignSubmitModel>();
            if (BuildDesignSubInfo(null)!=null)
            {
          DesignApplyInfoModels = BuildDesignSubInfo(null).ToList();
            }
           

            ViewBag.DesignApplyExaminePeopleName = employeeName;//将当前操作人员传到前端
            ViewBag.storeName = store;//将当前店铺名字传到前端
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.DesignApplyCurrentFilter = searchString;//获得前端传回来的搜索关键词

            if (!string.IsNullOrEmpty(searchString))
            {
                DesignApplyInfoModels = DesignApplyInfoModels.Where(w => w.联系方式 == searchString).ToList();//通过客户电话查找
            }
            //Session["Name"] = customerInfoModels.FirstOrDefault();
            #region 排序，默认按ID升序
            switch (sortOrder)
            {
                case "last_desc":
                    DesignApplyInfoModels = DesignApplyInfoModels.OrderByDescending(w => w.审批状态).ThenBy(w => w.Id).ToList();
                    break;
                case "first":
                    DesignApplyInfoModels = DesignApplyInfoModels.OrderBy(w => w.Id).ToList();
                    break;
                case "first_desc":
                    DesignApplyInfoModels = DesignApplyInfoModels.OrderByDescending(w => w.Id).ToList();
                    break;
                default:
                    DesignApplyInfoModels = DesignApplyInfoModels.OrderBy(w => w.审批状态).ThenBy(w => w.Id).ToList();
                    break;
            }

            #endregion
            int pageSize = 10;
            int pageNumber = (page ?? 1);
           
            return View(DesignApplyInfoModels.ToPagedList(pageNumber, pageSize));


        }

        /// <summary>
        /// 设计案申请审查
        /// </summary>
        /// <returns></returns>
        public ActionResult DesignApplyExamine(int id,string examineType) {
            Session["method"] = "Y";
            SetEmployee();
            try
            {
                var model = designSubmitBLL.GetModel(p => p.id == id);
                var str = JsonConvert.DeserializeObject<String>(examineType);
                if (str == "true")
                {
                    model.店长审核 = true;
                }
                else if (str == "false")
                {
                    model.店长审核 = false;
                }
                else
                {
                    return Json("发现非法数据,操作失败！");
                }
                model.店长 = employeeID;
                model.店长审核时间 = DateTime.Now;
                model.更新人 = employeeName;
                model.更新日期 = DateTime.Now;
                designSubmitBLL.Modify(model);
            }
            catch (Exception e)
            {

                return Json("店长设计案审查，数据更新异常！");
            }
           
            return Json("操作成功！");
        }
        /// <summary>
        /// 设计案完结审查
        /// </summary>
        /// <returns></returns>
        public ActionResult DesignResultExamineView(string sortOrder, string searchString, string currentFilter, int? page) {
            Session["method"] = "N";
            SetEmployee();//获取当前人员信息
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            ViewBag.EmployeesID = employeeID;
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            ViewBag.DesignResultCurrentSort = sortOrder;
            ViewBag.DesignResultDate = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.DesignResult = sortOrder == "last" ? "last_desc" : "last";
            List<DesignResultViewModel> designResultModels = new List<DesignResultViewModel>();
            //构建设计表信息  
            if (BuildResultInfo(employeeID)!=null)
            {
              designResultModels = BuildResultInfo(employeeID).ToList();
            }
           
            if (designResultModels == null)
            {
                return Content("<script>alert('当前操作人并无关联的设计信息或无进入权限！');window.history.go(-1);</script>");
            }
            ViewBag.DesignPeopleName = employeeName;//将当前操作人员传到前端
            ViewBag.DesignstoreName = store;//将当前店铺名字传到前端
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.DesignCurrentFilter = searchString;//获得前端传回来的搜索关键词

            if (!string.IsNullOrEmpty(searchString))
            {
                designResultModels = designResultModels.Where(w => w.客户编号 == searchString).ToList();
            }
            #region 排序，默认按ID升序
            switch (sortOrder)
            {
                case "first_desc":
                    designResultModels = designResultModels.OrderByDescending(w => w.制单日期).ToList();
                    break;
                case "last_desc":
                    designResultModels = designResultModels.OrderByDescending(w => w.审批状态).ToList();
                    break;
                case "last":
                    designResultModels = designResultModels.OrderBy(w => w.审批状态).ToList();
                    break;
                default:
                    designResultModels = designResultModels.OrderBy(w => w.制单日期).ToList();
                    break;
            }

            #endregion

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(designResultModels.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult DesignResultExamine(int id, string examineType) {
            Session["method"] = "Y";
            SetEmployee();
            try
            {
                var model = designResultBLL.GetModel(p => p.id == id);
                var str = JsonConvert.DeserializeObject<String>(examineType);
                if (str == "true")
                {
                    model.店长审核 = true;
                }
                else if (str == "false")
                {
                    model.店长审核 = false;
                }
                else
                {
                    return Json("发现非法数据,操作失败！");
                }
                model.店长 = employeeID;
                model.店长确认日期 = DateTime.Now;
                model.更新人 = employeeName;
                model.更新日期 = DateTime.Now;
                designResultBLL.Modify(model);
            }
            catch (Exception e)
            {

                return Json("店长设计案审查，数据更新异常！");
            }

            return Json("操作成功！");
        }

        /// <summary>
        /// 指定设计案设计师
        /// </summary>
        /// <param name="id">设计案提交ID</param>
        /// <returns></returns>
        public ActionResult ChangeApplyDesignerView(int id) {
            Session["method"] = "N";
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SetEmployee();
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            DesignSubmitModel model = new DesignSubmitModel();
            var m = designSubmitBLL.GetModel(p => p.id == id);
            model.Id = m.id;
            model.备注 = m.备注;
            model.客户喜好 = m.客户喜好或忌讳;
            model.客户在意品牌或已购买品牌 = m.客户在意品牌或已购买品牌;
            model.客户姓名 = m.客户姓名;
            model.客户提问与要求 = model.客户提问与要求;
            model.家具空间 = m.家具空间;
            model.家庭成员 = m.家庭成员;
            model.项目提交时间 = m.项目提交时间;
            model.楼盘具体位置 = m.楼盘具体位置;
            if (m.设计人员!=null)
            {
                var name = storeEmployeesBLL.GetModel(p => p.ID == m.设计人员).姓名;
                model.设计人员 = name;
            }
           
            var storeEmployees = storeEmployeesBLL.GetModels(p => p.店铺ID == storeID&&p.是否设计师==true&&p.停用标志!=true).ToList();

            SelectList EmployeeListList = new SelectList(storeEmployees, "ID", "姓名");
            ViewBag.ExamineEmployeeOptions = EmployeeListList;

            ViewBag.DesignPeople = m.客户姓名;
            return View(model);
        }


        public ActionResult ApllyDesignerEdit(DesignSubmitModel designSubmitModel)
        {
            SetEmployee();
            if (positionID != 3)
            {
                return Content("违规操作！");
            }
            string[] property = new string[1];
            property[0] = "设计人员";
            var model = designSubmitBLL.GetModel(p => p.id == designSubmitModel.Id);
            try
            {
                int did = int.Parse(designSubmitModel.设计人员);
                model.设计人员 = did;
            }
            catch (Exception e)
            {

                return Content(""+e.Message);
            }
           
            Session["method"] = "Y";
            designSubmitBLL.Modify(model, property);
           
            return RedirectToAction("ApplyDesignView");
        }

        /// <summary>
        /// 诚意/意向客户确认
        /// </summary>
        /// <returns></returns>
        public ActionResult SincerityCustomerConfirmView() {

            return View();

        }

        #endregion
        /// <summary>
        /// 设置当前操作人员及店铺信息
        /// </summary>
        private void SetEmployee()
        {

            string userName = HttpContext.User.Identity.Name;
            if (userName != null)
            {
                var employees = HttpContext.Session["Employee"] as Employees;
                employeeID = employees.ID;
                employeeName = employees.姓名;
                store = employees.店铺;
                storeID = storeBLL.GetModel(p => p.名称 == store).ID;
                positionID = storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID;
            }
        }

        /// <summary>
        /// 根据当前操作人员或职位或接待id查找跟进信息
        /// </summary>
        /// <param name="customerInfoID">接待id</param>
        /// <param name="employeeName">当前操作人员姓名</param>
        /// <returns>跟进信息</returns>s
        private List<CustomerTrackingModel> BuildTrackingInfo(int? customerInfoID, int employeeId)
        {
            List<销售_意向追踪日志> mt = new List<销售_意向追踪日志>();
            var e = storeEmployeesBLL.GetModel(p => p.ID == employeeID);
            if (customerInfoID != 0)
            {
                mt = customerTrackingBLL.GetModels(p => p.接待记录ID == customerInfoID).ToList();//查看当前客户的跟进信息
            }
            else if (e.职务ID == 3)
            {
                mt = customerTrackingBLL.GetModels(p => p.店铺ID == e.店铺ID).ToList();//店长查看(只有自己店铺的)
            }

            else
            {
                mt = customerTrackingBLL.GetModels(p => p.跟进人 == employeeID).ToList();//店员查看所有(只有自己跟进的)
            }


            List<CustomerTrackingModel> customerTrackingModels = new List<CustomerTrackingModel>();
            foreach (var item in mt)
            {
                CustomerTrackingModel customerTrackingModel = new CustomerTrackingModel
                {
                    Id = item.id,
                    备注 = item.备注,
                    店长审核 = item.店长审核,
                    接待序号 = customerInfoBLL.GetModel(p => p.ID == item.接待记录ID).接待序号,
                    客户姓名 = customerInfoBLL.GetModel(p => p.ID == item.接待记录ID).客户姓名,
                    客户电话 = customerInfoBLL.GetModel(p => p.ID == item.接待记录ID).客户电话,
                    接待ID = item.接待记录ID,
                    是否申请设计师 = item.是否申请设计师,
                    跟进人 = storeEmployeesBLL.GetModel(p => p.ID == item.跟进人).姓名,
                    跟进内容 = item.跟进内容,
                    跟进方式 = item.跟进方式,
                    跟进时间 = item.跟进时间,
                   
                   
                    店铺 = storeBLL.GetModel(p => p.ID == item.店铺ID).名称
                };
                switch (item.跟进结果)
                {
                    case "成交":
                        customerTrackingModel.跟进结果 = CustomerTrackResult.成交;
                        break;
                    case "申请设计":
                        customerTrackingModel.跟进结果 = CustomerTrackResult.申请设计;
                        break;
                    case "观察":
                        customerTrackingModel.跟进结果 = CustomerTrackResult.观察;
                        break;
                    case "放弃":
                        customerTrackingModel.跟进结果 = CustomerTrackResult.放弃;
                        break;
                    case "继续跟进":
                        customerTrackingModel.跟进结果 = CustomerTrackResult.继续跟进;
                        break;
                    default:
                        break;
                }
                
                customerTrackingModels.Add(customerTrackingModel);
            }

            return customerTrackingModels;

        }

        /// <summary>
        /// 构建客户信息
        /// </summary>
        private IQueryable<CustomerInfoModel> BuildCustomerInfo()
        {
            List<CustomerInfoModel> customerInfoModelsList = new List<CustomerInfoModel>();

           
                var customer = customerInfoBLL.GetModels(p => p.店铺ID == storeID);//查询当前店铺所有顾客接待信息
                if (customer != null)
                {
                    foreach (var item in customer)
                    {
                        CustomerInfoModel customerInfo = new CustomerInfoModel();
                        try
                        {


                            customerInfo.ID = item.ID;
                            customerInfo.店铺 = storeBLL.GetModel(p => p.ID == item.店铺ID).名称;
                            customerInfo.接待人 = storeEmployeesBLL.GetModel(p => p.ID == item.接待人ID).姓名;
                            customerInfo.接待序号 = item.接待序号;
                            customerInfo.接待日期 = item.接待日期.ToString("d");
                            customerInfo.主导者 = item.主导者;
                            customerInfo.主导者喜好风格 = item.主导者喜好风格;
                            customerInfo.使用空间 = item.使用空间;
                            customerInfo.出店时间 = item.出店时间;
                            customerInfo.制单日期 = item.制单日期;
                            customerInfo.同行人 = item.同行人;
                            customerInfo.如何得知品牌 = item.如何得知品牌;
                            customerInfo.安装地址 = item.安装地址;
                            customerInfo.客户姓名 = item.客户姓名;
                            customerInfo.客户建议 = item.客户建议;
                            customerInfo.客户来源 = item.客户来源;
                            customerInfo.客户电话 = item.客户电话;
                            customerInfo.客户着装 = item.客户着装;
                            customerInfo.客户类别 = item.客户类别;
                            customerInfo.客户类型 = item.客户类型;
                            customerInfo.客户职业 = item.客户职业;
                            customerInfo.家庭成员 = item.家庭成员;
                            customerInfo.年龄段 = item.年龄段;
                            customerInfo.性别 = item.性别;
                            customerInfo.是否有意向 = item.是否有意向;
                            if (item.更新人 != null)
                            {
                                customerInfo.更新人 = storeEmployeesBLL.GetModel(p => p.ID == item.更新人).姓名;
                            }

                            customerInfo.更新日期 = item.更新日期;
                            customerInfo.来店次数 = item.来店次数;
                            customerInfo.比较品牌 = item.比较品牌;
                            customerInfo.特征 = item.特征;
                            customerInfo.社交软件 = item.社交软件;
                            customerInfo.装修情况 = item.装修情况;
                            customerInfo.装修进度 = item.装修进度;
                            customerInfo.装修风格 = item.装修风格;
                            customerInfo.设计师 = item.设计师;
                            if (item.跟进人ID != null)
                            {
                                customerInfo.跟进人 = storeEmployeesBLL.GetModel(p => p.ID == item.跟进人ID).姓名;
                            }

                            customerInfo.返点 = item.返点;
                            customerInfo.进店时长 = item.进店时长;
                            customerInfo.进店时间 = item.进店时间;
                            customerInfo.预报价折扣 = item.预报价折扣;
                            customerInfo.预算金额 = item.预算金额;
                            customerInfo.预计使用时间 = item.预计使用时间;


                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }

                        customerInfoModelsList.Add(customerInfo);
                    }
                }
           
            return customerInfoModelsList.AsEnumerable().AsQueryable();
        }

        /// <summary>
        /// /// <summary>
        /// 构建软装服务设计提交信息
        /// </summary>
        /// </summary>
        /// <param name="id">接待id</param>
        /// <returns>设计案提交表信息</returns>
        public IQueryable<DesignSubmitModel> BuildDesignSubInfo(int? id)
        {
            List<销售_设计案提交表> designSubModelList = new List<销售_设计案提交表>();
            if (storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID == 3)
            {   //店长可以查看所有信息
                designSubModelList = designSubmitBLL.GetModels(p => p.店铺ID==storeID).ToList();
            }

            else if (id != 0 && id != null)
            {   //根据接待ID查询当前客户的设计提交表
                designSubModelList = designSubmitBLL.GetModels(p => p.接待记录ID == id).ToList();
            }
            else
            {
                //查询当前销售人员的设计提交表
                designSubModelList = designSubmitBLL.GetModels(p => p.销售人员 == employeeID).ToList();
            }
            List<DesignSubmitModel> designSubmitModelList = new List<DesignSubmitModel>();
            foreach (var item in designSubModelList)
            {
                DesignSubmitModel designSubmitModel = new DesignSubmitModel();
                designSubmitModel.Id = item.id;
                designSubmitModel.客户喜好 = item.客户喜好或忌讳;
                designSubmitModel.客户在意品牌或已购买品牌 = item.客户在意品牌或已购买品牌;
                designSubmitModel.客户姓名 = item.客户姓名;
                designSubmitModel.客户提问与要求 = item.客户提问与要求;
                designSubmitModel.家具空间 = item.家具空间;
                designSubmitModel.家庭成员 = item.家庭成员;
                if (item.店长!=null)
                {
                    designSubmitModel.店长 = storeEmployeesBLL.GetModel(p => p.ID == item.店长).姓名;
                }
                
                designSubmitModel.店长审核 = item.店长审核;
                designSubmitModel.接待记录ID = item.接待记录ID;
                if (item.整体软装配饰 == null)
                {
                    item.整体软装配饰 = false;
                }
                designSubmitModel.整体软装配饰 = item.整体软装配饰.Value;
                designSubmitModel.楼盘具体位置 = item.楼盘具体位置;
                designSubmitModel.职业 = item.职业;
                designSubmitModel.联系方式 = item.联系方式;
                designSubmitModel.装修进度 = item.装修进度;
                designSubmitModel.装修风格 = item.装修风格;
                if (item.设计人员!=null)
                {
                    designSubmitModel.设计人员 = storeEmployeesBLL.GetModel(p => p.ID == item.设计人员).姓名;
                }
                designSubmitModel.设计人员审核 = item.设计人员审核;
                designSubmitModel.销售人员 = storeEmployeesBLL.GetModel(p => p.ID == item.销售人员).姓名 ;
                designSubmitModel.面积大小 = item.面积大小;

                designSubmitModel.预算 = item.预算;
                designSubmitModel.项目提交时间 = item.项目提交时间;
                designSubmitModel.项目量房时间 = item.项目量房时间;
                designSubmitModel.项目预计完成时间 = item.项目预计完成时间;
                designSubmitModel.备注 = item.备注;
                if (item.店长 != null && item.店长审核 == true && item.设计人员 != null && item.设计人员审核 == true && item.销售人员 != null)
                {
                    designSubmitModel.审批状态 = true;
                }
                designSubmitModelList.Add(designSubmitModel);
            }
            return designSubmitModelList.AsQueryable();
        }


        /// <summary>
        /// 初始化设计案跟进日志
        /// </summary>
        /// <returns></returns>
        private List<DesignTackLogViewModel> BuildDesignTrackLogInfo(int? id)
        {
           
           
            List<DesignTackLogViewModel> designTackLogViewModels = new List<DesignTackLogViewModel>();
            try
            {
               List<销售_设计案跟进日志> designTackLogList =new List<销售_设计案跟进日志>() ;
             if (positionID==3)
              {
                    designTackLogList = designTackLogList = designTrackingLogBLL.GetModels(p => p.店铺ID==storeID).ToList();
               }
                else if (id == 0 || id == null)
                {
                    return null;
                }
                else
                {
                    designTackLogList = designTrackingLogBLL.GetModels(p => p.设计案提交表id == id).ToList();
                }
                foreach (var item in designTackLogList)
                {
                    DesignTackLogViewModel designTackLogViewModel = new DesignTackLogViewModel();
                    designTackLogViewModel.Id = item.id;
                    designTackLogViewModel.参与人员 = item.参与人员;
                    designTackLogViewModel.备注 = item.备注;
                    designTackLogViewModel.客户姓名 = designSubmitBLL.GetModel(p => p.id == item.设计案提交表id).客户姓名;
                    designTackLogViewModel.联系方式 = designSubmitBLL.GetModel(p => p.id == item.设计案提交表id).联系方式;
                    designTackLogViewModel.设计师 = item.设计师;
                    designTackLogViewModel.设计案提交表id = item.设计案提交表id;
                    designTackLogViewModel.设计案需求提交时间 = item.设计案需求提交时间;
                    designTackLogViewModel.跟进日期 = item.跟进日期;
                    designTackLogViewModel.进度描述 = item.进度描述;
                    designTackLogViewModel.销售人员 = item.销售人员;
                    designTackLogViewModel.需要的支持 = item.需要的支持;
                    designTackLogViewModel.预计签约时间 = item.预计签约时间;
                    designTackLogViewModel.楼盘具体位置 = designSubmitBLL.GetModel(p => p.id == item.设计案提交表id).楼盘具体位置;
                    designTackLogViewModel.店长审查 = item.店长审查;
                    designTackLogViewModels.Add(designTackLogViewModel);
                }

            }
            catch (Exception e)
            {

                throw e;
            }
            return designTackLogViewModels;

        }


        /// <summary>
        /// 构建设计完结单信息
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        private IQueryable<DesignResultViewModel> BuildResultInfo(int employeeID)
        {
            var role = storeEmployeesBLL.GetModel(p => p.ID == employeeID);
            List<DesignResultViewModel> designResultViews = new List<DesignResultViewModel>();
            List<设计_设计案完结单> models = new List<设计_设计案完结单>();
          
            if (role.是否店长 == true)
            {
                models = designResultBLL.GetModels(p => p.店铺ID == storeID).ToList();
            }
            else
            {
                return null;
            }
            foreach (var item in models)
            {
                DesignResultViewModel designResultViewModel = new DesignResultViewModel();
                designResultViewModel.Id = item.id;
                designResultViewModel.制单日期 = item.制单日期;
                designResultViewModel.合计成交金额 = item.合计成交金额;
                var files = design_ProjectDrawingsBLL.GetModels(p => p.设计案提交表ID == item.设计案提交表ID && p.文件类型 == 8);
                List<String> listPath = new List<string>();
                if (files.Count() > 0)
                {
                    foreach (var ite in files)
                    {
                        listPath.Add(ite.存储路径);
                    }
                    designResultViewModel.完成效果图路径 = listPath;
                }
                designResultViewModel.实际完成时间 = item.实际完成时间;
                designResultViewModel.实际完成空间 = item.实际完成空间;
                designResultViewModel.审批状态 = item.审批状态;
                designResultViewModel.客户编号 = item.客户单号;//合同编号
                designResultViewModel.店长审核 = item.店长审核;
                designResultViewModel.设计师确认 = item.设计师确认;
                designResultViewModel.销售审核 = item.销售审核;
                designResultViewModel.设计案提交表ID = item.设计案提交表ID;
                designResultViewModel.销售人员 = storeEmployeesBLL.GetModel(p => p.ID == item.销售人员).姓名;
                designResultViewModel.销售人员确认日期 = item.销售人员确认日期;
                designResultViewModel.销售单号 = item.销售单号;//订单编号
                designResultViewModel.单据编号 = item.单据编号;//单据编号

                designResultViewModel.店长 = storeEmployeesBLL.GetModel(p => p.ID == item.店长).姓名;
                designResultViewModel.店长审核日期 = item.店长确认日期;
                designResultViewModel.延期或无法完成原因 = item.延期或无法完成原因;
                designResultViewModel.建议 = item.建议;
                designResultViewModel.更新人 = item.更新人;
                designResultViewModel.计划完成时间 = item.计划完成时间;
                designResultViewModel.计划完成空间 = item.计划完成空间;
                designResultViewModel.设计师 = storeEmployeesBLL.GetModel(p => p.ID == item.设计师).姓名;
                designResultViewModel.设计师确认 = item.设计师确认;
                designResultViewModel.设计师确认日期 = item.设计师确认日期;
                designResultViewModel.更新日期 = item.更新日期;
                int sellID;//销售订单ID
                if (item.设计案提交表ID != null && item.设计案提交表ID != 0)
                {

                    var phoneNumber = designSubmitBLL.GetModel(w => w.id == item.设计案提交表ID).联系方式;
                    try
                    {
                        sellID = salesOrderBLL.GetModel(p => p.客户联系方式 == phoneNumber && p.店铺ID == storeID).ID;//根据联系方式查找相应的客户的订单ID

                        designResultViewModel.客户编号 = salesOrderBLL.GetModel(p => p.ID == sellID).合同编号;//合同编号

                        designResultViewModel.设计案提交表ID = item.设计案提交表ID;
                        designResultViewModel.销售单号 = salesOrderBLL.GetModel(p => p.ID == sellID).订单编号;//订单编号
                        designResultViewModel.单据编号 = salesOrderBLL.GetModel(p => p.ID == sellID).单据编号;//单据编号

                    }
                    catch (Exception e)
                    {
                        return null;
                    }

                    var lis = salesOrder_DetailsBLL.GetModels(p => p.单据ID == sellID).ToList();//根据订单ID查询订单详细
                    List<DesignResult_OrderDetailViewModel> list = new List<DesignResult_OrderDetailViewModel>();
                    foreach (var ite in lis)
                    {
                        DesignResult_OrderDetailViewModel model = new DesignResult_OrderDetailViewModel();
                        var spuid = product_SKUBLL.GetModel(p => p.ID == ite.SKU_ID).SPU_ID;
                        var productid = product_SPUBLL.GetModel(p => p.ID == spuid).商品ID;
                        var product = productBLL.GetModel(p => p.ID == productid);
                        model.产品型号 = product.编号;
                        model.单位 = product.计量单位;
                        model.成交价 = ite.零售小计;
                        model.数量 = ite.数量;
                        model.空间 = "";
                        list.Add(model);
                    }
                    designResultViewModel.DesignResult_OrderDetailViewModel = list;
                }
                designResultViews.Add(designResultViewModel);
            }
            return designResultViews.AsQueryable();
        }
    }
}