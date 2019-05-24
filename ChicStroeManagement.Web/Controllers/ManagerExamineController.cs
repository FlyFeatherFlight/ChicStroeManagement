﻿using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;
using ChicStoreManagement.WEB.ViewModel;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections;
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
        private ISalesRecordBLL salesRecordBLL;

        public ManagerExamineController(ICustomerInfoBLL customerInfoBLL, IExceptedBuyBLL exceptedBuyBLL, IStoreEmployeesBLL storeEmployeesBLL, IStoreBLL storeBLL, IProductCodeBLL productCodeBLL, ICustomerTrackingBLL customerTrackingBLL, IPositionBLL positionBLL, IDesignSubmitBLL designSubmitBLL, IDesignTrackingLogBLL designTrackingLogBLL, IDesign_ProjectDrawingsBLL design_ProjectDrawingsBLL, IDesign_CustomerExceptedBuyBLL design_CustomerExceptedBuyBLL, IDesignResultBLL designResultBLL, IDesignResult_DealListingBLL designResult_DealListingBLL, ISalesOrderBLL salesOrderBLL, ISalesOrder_detailsBLL salesOrder_DetailsBLL, IProduct_SPUBLL product_SPUBLL, IProduct_SKUBLL product_SKUBLL, IProductBLL productBLL, ISalesRecordBLL salesRecordBLL)
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
            this.salesRecordBLL = salesRecordBLL;
        }





        // GET: ManagerExamine
        /// <summary>
        ///店长操作首页
        /// </summary>
        /// <returns></returns>
        public ActionResult ManagerExamineIndex()
        {
            Session["method"] = "N";
            //获取当前人员信息
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
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
        public ActionResult CustomerExamineView( string searchString,int? EmployeeID, string currentFilter, bool? IsExcept, bool? IsDeal, string startDate, string EndDate) {
            List<CustomerInfoModel> customerInfoModels = new List<CustomerInfoModel>();
            Session["method"] = "N";
            //获取当前人员信息
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;
            
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            var models = BuildCustomerInfoModels();
            if (models!=null)
            {
                customerInfoModels = models.ToList();
            }
            ViewBag.CustomerExaminePeopleName = em.姓名;//将当前操作人员传到前端
            ViewBag.storeName = em.店铺;//将当前店铺名字传到前端
           
          

            if (searchString!=null)
            {
                customerInfoModels = customerInfoModels.Where(w => w.客户电话 == searchString).ToList();//通过客户电话查找
            }
            if (EmployeeID != null)
            {
                customerInfoModels = customerInfoModels.Where(w => w.接待人ID == EmployeeID).ToList();//通过客户电话查找
            }
            if (IsExcept==true)
            {
                customerInfoModels = customerInfoModels.Where(w => w.是否有意向 == true).ToList();   
            }
            if (IsDeal==true)
            {
                customerInfoModels = customerInfoModels.Where(w => w.是否成交 == true).ToList();
            }
            if (startDate != null && startDate != "")
            {
                DateTime startTime = Convert.ToDateTime(startDate);
                customerInfoModels = customerInfoModels.Where(w => w.接待日期 > startTime).ToList();
            }
            if (EndDate != null && EndDate != "")
            {
                DateTime endTime = Convert.ToDateTime(EndDate);
                customerInfoModels = customerInfoModels.Where(w => w.接待日期 < endTime).ToList();

            }
            //Session["Name"] = customerInfoModels.FirstOrDefault();

            ViewBag.CustomerExamineManager  =em.是否店长;//给前端传入当前操作人职位
            customerInfoModels.OrderByDescending(p=>p.接待日期);
            return Json(customerInfoModels);


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

            //获取当前人员信息
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;
           
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            CustomerInfoModel customerInfo = new CustomerInfoModel();
            var models = BuildCustomerInfoModels();
            if (models != null)
            {
               customerInfo = models.First(p => p.ID == id);
            }
           

            if (customerInfo.接待人ID == em.ID || customerInfo.跟进人ID == em.ID || em.职务ID == 3)
            {
                @ViewBag.TrackPeople = customerInfo.客户姓名;
                customerInfo.更新人 = em.姓名;
                customerInfo.更新日期 = DateTime.Now;
                ViewBag.Position = em.职务ID;
                var storeEmployees = storeEmployeesBLL.GetModels(p => p.店铺ID == em.店铺ID).ToList();

                SelectList EmployeeListList = new SelectList(storeEmployees, "ID", "姓名");
                ViewBag.ExamineEmployeeOptions = EmployeeListList;
                return View(customerInfo);
            }

            return Content("<script>alert('无操作权限！！');window.history.go(-1);</script>");


        }

        /// <summary>
        /// 获得当前店铺的销售人员
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEmployees() {
            var em = SetEmployee();
            var models = storeEmployeesBLL.GetModels(p => p.店铺ID == em.店铺ID).ToList();
            var tripObj = new ArrayList();
            foreach (var item in models)
            {
                if (item.停用标志!=true)
                {
                    var Auditor = new string[] { item.ID.ToString(), item.姓名 };//只保存ID和姓名
                    tripObj.Add(Auditor);
                }

               
            }
            return Json(tripObj);
        }

        /// <summary>
        /// 指定跟进人
        /// </summary>
        /// <param name="id">客户接待ID</param>
        /// <param name="跟进人">跟进人名</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomerTrackCustomerEdit(int id, string 跟进人) {
           var em= SetEmployee();
            if (em.职务ID != 3)
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
                customerInfoBLL.Modify(model, property);
            }
            catch (Exception e)
            {

                return Content(""+e.Message);
            }
            
            Session["method"] = "Y";
           
            return RedirectToAction("ManagerExamineIndex");
        }
        /// <summary>
        /// 客户追踪日志审查
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerTrackLogExamineView(int? id, string sortOrder, string PhoneSearchString, string NameSearchString, string currentFilter, int? page) {
            List<CustomerTrackingModel> customerTrackingModels = new List<CustomerTrackingModel>();
            Session["method"] = "N";
            //获取当前人员信息
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;
            ViewBag.CustomerCurrentSort = sortOrder;
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            ViewBag.TrackingDate = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.TrackingResult = sortOrder == "last" ? "last_desc" : "last";
            if (id != null && id != 0)
            {
                if (BuildTrackingInfo(id, em.ID)!=null)
                {
                customerTrackingModels = BuildTrackingInfo(id, em.ID);//获取当前人员可查看的跟进信息
                ViewBag.Reception = customerInfoBLL.GetModel(p => p.ID == id).接待序号;//将接待序号传到前端
                }
                
            }
            else
            {
                if (BuildTrackingInfo(0, em.ID)!=null)
                {
                customerTrackingModels = BuildTrackingInfo(0, em.ID);
                }
               
            }
            if (customerTrackingModels == null)
            {
                return Content("当前操作人并无关联的跟进信息或无进入权限！");
            }
            
            ViewBag.trackingPeopleName = em.姓名;//将当前操作人员传到前端
            ViewBag.storeName = em.店铺;//将当前店铺名字传到前端
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
                customerTrackingModels = customerTrackingModels.Where(w => w.客户电话 == searchString && w.店铺==em.店铺).ToList();//通过客户电话查找
            }
            if (!string.IsNullOrEmpty(searchString) && NameSearchString != null)
            {
                customerTrackingModels = customerTrackingModels.Where(w => w.跟进人 == searchString && w.店铺 == em.店铺).ToList();//通过跟进人查找
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


        #region  销售成交数据
        /// <summary>
        /// 成交客户名单
        /// </summary>
        /// <returns></returns>
        public ActionResult DealCustomerView()
        {
            Session["method"] = "N";
            //获取当前人员信息
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            return View();
        }

        /// <summary>
        /// 成交客户名单JSON数据
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="currentFilter"></param>
        /// <param name="IsExcept"></param>
        /// <param name="IsDeal"></param>
        /// <param name="startDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public ActionResult DealCustomerDate(string searchString, int? EmployeeID, string startDate, string EndDate)
        {
            List<CustomerInfoModel> customerInfoModels = new List<CustomerInfoModel>();
            Session["method"] = "N";
            //获取当前人员信息
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            var models = BuildCustomerInfoModels();
            if (models != null)
            {
                customerInfoModels = models.Where(p=>p.是否成交 == true).ToList();
            }
            ViewBag.CustomerExaminePeopleName = em.姓名;//将当前操作人员传到前端
            ViewBag.storeName = em.店铺;//将当前店铺名字传到前端



            if (searchString != null)
            {
                customerInfoModels = customerInfoModels.Where(w => w.客户电话 == searchString).ToList();//通过客户电话查找
            }
            if (EmployeeID != null)
            {
                customerInfoModels = customerInfoModels.Where(w => w.接待人ID == EmployeeID).ToList();//通过客户电话查找
            }
            
            if (startDate != null && startDate != "")
            {
                DateTime startTime = Convert.ToDateTime(startDate);
                customerInfoModels = customerInfoModels.Where(w => w.接待日期 > startTime).ToList();
            }
            if (EndDate != null && EndDate != "")
            {
                DateTime endTime = Convert.ToDateTime(EndDate);
                customerInfoModels = customerInfoModels.Where(w => w.接待日期 < endTime).ToList();

            }
            //Session["Name"] = customerInfoModels.FirstOrDefault();

            ViewBag.CustomerExamineManager = em.是否店长;//给前端传入当前操作人职位
            customerInfoModels.OrderByDescending(p => p.接待日期);
            return Json(customerInfoModels);


        }

        /// <summary>
        /// 销售成交记录列表页
        /// </summary>
        /// <param name="id">接待ID</param>
        /// <param name="page"></param>
        /// <param name="searchString">查询条件销售单号</param>
        /// <param name="employeesID">销售ID</param>
        /// <param name="IsDeal">是否全款</param>
        /// <param name="startDate">查询条件，开始时间</param>
        /// <param name="EndDate">查询条件，结束时间</param>
        /// <returns></returns>
        public ActionResult SalesRecordsIndex( int? page, string searchString, int? employeesID, bool? IsDeal, string startDate, string EndDate)
        {
            Session["method"] = "N";
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
          
            List<SalesRecordsViewModel> salesRecordsModels = new List<SalesRecordsViewModel>();
            salesRecordsModels = BuildSalesRecordsModels();//将顾客接待信息数据优化
            int pageSize = 25;
            int pageNumber = (page ?? 1);
           
            if (salesRecordsModels.Count <= 0)
            {
                return View(salesRecordsModels.ToPagedList(pageNumber, pageSize));
            }

           

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Replace(" ", "");
                salesRecordsModels = salesRecordsModels.Where(w => w.合同单号 == searchString).ToList();//通过销售单号查找
                ViewBag.SearchString = searchString;
            }
            if (employeesID > 0)
            {
                salesRecordsModels = salesRecordsModels.Where(w => w.销售人员ID == employeesID).ToList();//通过销售人员查找
                ViewBag.EmploeeID = employeesID;
            }
            if (IsDeal == true)
            {
                salesRecordsModels = salesRecordsModels.Where(w => w.是否全款 == true).ToList();
            }
            if (startDate != null && startDate!="")
            {
                DateTime startTime = Convert.ToDateTime(startDate);
                salesRecordsModels = salesRecordsModels.Where(w => w.销售日期 > startTime).ToList();
                ViewBag.StartDate = startDate;
            }
            if (EndDate != null && EndDate!="")
            {
                DateTime endTime = Convert.ToDateTime(EndDate);
                salesRecordsModels = salesRecordsModels.Where(w => w.销售日期 < endTime).ToList();
                ViewBag.EndDate = endTime;
            }

            salesRecordsModels = salesRecordsModels.OrderByDescending(p => p.销售日期).ToList();
            
            return View(salesRecordsModels.ToPagedList(pageNumber, pageSize));

        }
        /// <summary>
        /// 销售成交数据详情页面
        /// </summary>
        /// <param name="id">销售成交数据ID</param>
        /// <returns></returns>
        public ActionResult SalesRecordInfoView(int id)
        {
            var em = SetEmployee();
            ViewBag.Store = em.店铺;
            ViewBag.Employee = em.姓名;
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
           
            var model = salesRecordBLL.GetModel(p => p.ID == id);
            var m = BuildSalesRecordsModel(model);
          
            return View(m);
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
            //获取当前人员信息
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;
            ViewBag.CustomerCurrentSort = sortOrder;
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            ViewBag.DesignTrackExamineTrackingResult = String.IsNullOrEmpty(sortOrder) ? "last_desc" : "";
            ViewBag.DesignTrackExamineID = String.IsNullOrEmpty(sortOrder) ? "first" : "first_desc";
            List<DesignTackLogViewModel> DesignTrackInfoModels = new List<DesignTackLogViewModel>();
            if (BuildDesignTrackLogInfo(null)!=null)
            {
            DesignTrackInfoModels = BuildDesignTrackLogInfo(null).ToList();
            }
            

            ViewBag.DesignTrackExaminePeopleName = em.姓名;//将当前操作人员传到前端
            ViewBag.storeName = em.店铺;//将当前店铺名字传到前端
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
            var em = SetEmployee();
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
                model.更新人 = em.姓名;
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
            
            //获取当前人员信息
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;
            ViewBag.CustomerCurrentSort = sortOrder;
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            ViewBag.DesignApplyExamineTrackingResult = String.IsNullOrEmpty(sortOrder) ? "last_desc" : "";
            ViewBag.DesignApplyExamineID = String.IsNullOrEmpty(sortOrder) ? "first" : "first_desc";
            List<DesignSubmitModel> DesignApplyInfoModels = new List<DesignSubmitModel>();
            if (BuildDesignSubInfo(null)!=null)
            {
          DesignApplyInfoModels = BuildDesignSubInfo(null).ToList();
            }
           

            ViewBag.DesignApplyExaminePeopleName = em.姓名;//将当前操作人员传到前端
            ViewBag.storeName = em.店铺;//将当前店铺名字传到前端
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
            var em=SetEmployee();
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
                model.店长 = em.ID;
                model.店长审核时间 = DateTime.Now;
                model.更新人 = em.姓名;
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
            //获取当前人员信息
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;
            ViewBag.CustomerCurrentSort = sortOrder;
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            ViewBag.DesignResultDate = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.DesignResult = sortOrder == "last" ? "last_desc" : "last";
            List<DesignResultViewModel> designResultModels = new List<DesignResultViewModel>();
            //构建设计表信息  
            if (BuildResultInfo(em.ID)!=null)
            {
              designResultModels = BuildResultInfo(em.ID).ToList();
            }
           
            if (designResultModels == null)
            {
                return Content("<script>alert('当前操作人并无关联的设计信息或无进入权限！');window.history.go(-1);</script>");
            }
            ViewBag.DesignPeopleName = em.姓名;//将当前操作人员传到前端
            ViewBag.DesignstoreName = em.店铺;//将当前店铺名字传到前端
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.DesignResultCurrentFilter = searchString;//获得前端传回来的搜索关键词

            if (!string.IsNullOrEmpty(searchString))
            {
                designResultModels = designResultModels.Where(w => w.客户编号 == searchString).ToList();
            }
            #region 排序，默认按ID升序
            switch (sortOrder)
            {
                case "first_desc":
                    designResultModels = designResultModels.OrderByDescending(w => w.Id).ToList();
                    break;
                case "last_desc":
                    designResultModels = designResultModels.OrderByDescending(w => w.审批状态).ToList();
                    break;
                case "last":
                    designResultModels = designResultModels.OrderBy(w => w.审批状态).ToList();
                    break;
                default:
                    designResultModels = designResultModels.OrderByDescending(w => w.Id).ToList();
                    break;
            }

            #endregion

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(designResultModels.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult DesignResultExamine(int id, string examineType) {
            Session["method"] = "Y";
            var em = SetEmployee();
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
                model.店长 = em.ID;
                model.店长确认日期 = DateTime.Now;
                model.更新人 = em.姓名;
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

            //获取当前人员信息
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;
   
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            DesignSubmitModel model = new DesignSubmitModel();
            var m = designSubmitBLL.GetModel(p => p.id == id);
            model.Id = m.id;
            model.备注 = m.备注;
            model.客户喜好 = m.客户喜好或忌讳;
            model.客户在意品牌或已购买品牌 = m.客户在意品牌或已购买品牌;
            model.客户姓名 = m.客户姓名;
            model.客户提问与要求 = model.客户提问与要求;
            model.家具空间 = m.家具空间;
            model.家居使用者 = m.家居使用者;
            model.项目提交时间 = m.项目提交时间;
            model.楼盘具体位置 = m.楼盘具体位置;
            if (m.设计人员!=null)
            {
                var name = storeEmployeesBLL.GetModel(p => p.ID == m.设计人员).姓名;
                model.设计人员 = name;
            }
           
            var storeEmployees = storeEmployeesBLL.GetModels(p => p.店铺ID == em.店铺ID&&p.是否设计师==true&&p.停用标志!=true).ToList();

            SelectList EmployeeListList = new SelectList(storeEmployees, "ID", "姓名");
            ViewBag.ExamineEmployeeOptions = EmployeeListList;

            ViewBag.DesignPeople = m.客户姓名;
            return View(model);
        }


        public ActionResult ApllyDesignerEdit(DesignSubmitModel designSubmitModel)
        {
            var em=SetEmployee();
            if (em.职务ID != 3)
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
           
            return RedirectToAction("DesignApplyInfoView", "DesignScheme", new { id=model.id});
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
        /// 
        private Employees SetEmployee()
        {

            string userName = HttpContext.User.Identity.Name;
            if (userName != null)
            {
                var employees = HttpContext.Session["Employee"] as Employees;
                return employees;
            }
            else
            {
                return null;
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
            var em = SetEmployee();
            List<销售_意向追踪日志> mt = new List<销售_意向追踪日志>();
            var e = storeEmployeesBLL.GetModel(p => p.ID == em.ID);
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
                mt = customerTrackingBLL.GetModels(p => p.跟进人 == em.ID).ToList();//店员查看所有(只有自己跟进的)
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
        /// 构造客户信息单个数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private CustomerInfoModel BuildCustomerInfoModel(销售_接待记录 model)
        {

            CustomerInfoModel customerInfo = new CustomerInfoModel();
            try
            {


                customerInfo.ID = model.ID;
                customerInfo.店铺 = storeBLL.GetModel(p => p.ID == model.店铺ID).名称;
                customerInfo.店铺ID = model.店铺ID;
                customerInfo.接待人 = storeEmployeesBLL.GetModel(p => p.ID == model.接待人ID).姓名;
                customerInfo.接待人ID = model.接待人ID;
                customerInfo.接待序号 = model.接待序号;
                customerInfo.接待日期 = model.接待日期;
                customerInfo.主导者 = model.主导者;
                customerInfo.主导者喜好风格 = model.主导者喜好风格;
                customerInfo.使用空间 = model.使用空间;
                customerInfo.出店时间 = model.出店时间;
                customerInfo.制单日期 = model.制单日期;
                customerInfo.同行人 = model.同行人;
                customerInfo.如何得知品牌 = model.如何得知品牌;
                customerInfo.安装地址 = model.安装地址;
                customerInfo.客户姓名 = model.客户姓名;
                customerInfo.客户建议 = model.客户建议;
                customerInfo.客户来源 = model.客户来源;
                customerInfo.客户电话 = model.客户电话;
                customerInfo.客户着装 = model.客户着装;
                customerInfo.客户类别 = model.客户类别;
                customerInfo.客户类型 = model.客户类型;
                customerInfo.客户职业 = model.客户职业;
                customerInfo.目的 = model.客户目的;
                customerInfo.家居使用者 = model.家居使用者;
                customerInfo.年龄段 = model.年龄段;
                customerInfo.性别 = model.性别;
                customerInfo.是否有意向 = model.是否有意向;
                if (model.更新人 != null)
                {
                    customerInfo.更新人ID = model.更新人.Value;
                    customerInfo.更新人 = storeEmployeesBLL.GetModel(p => p.ID == model.更新人).姓名;
                }

                customerInfo.更新日期 = model.更新日期;
                customerInfo.来店次数 = model.来店次数;
                customerInfo.比较品牌 = model.比较品牌;
                customerInfo.特征 = model.特征;
                customerInfo.社交软件 = model.社交软件;
                customerInfo.装修情况 = model.装修情况;
                customerInfo.装修进度 = model.装修进度;
                customerInfo.装修风格 = model.装修风格;
                customerInfo.设计师 = model.设计师;
                if (model.跟进人ID != null)
                {
                    customerInfo.跟进人ID = model.跟进人ID.Value;
                    customerInfo.跟进人 = storeEmployeesBLL.GetModel(p => p.ID == model.跟进人ID).姓名;
                }

                customerInfo.返点 = model.返点;
                customerInfo.进店时长 = model.进店时长;
                customerInfo.进店时间 = model.进店时间;
                customerInfo.预报价折扣 = model.预报价折扣;
                customerInfo.预算金额 = model.预算金额;
                customerInfo.预计使用时间 = model.预计使用时间;
                customerInfo.不喜欢产品 = model.不喜欢产品;
                customerInfo.不喜欢元素 = model.不喜欢元素;
                customerInfo.主卧预算备注 = model.卧室预算备注;
                customerInfo.主卧预算家具 = model.卧室预算家具;
                customerInfo.主卧预算金额 = model.卧室预算金额;
                customerInfo.餐厅预算备注 = model.餐厅预算备注;
                customerInfo.餐厅预算家具 = model.餐厅预算家具;
                customerInfo.餐厅预算金额 = model.餐厅预算金额;
                customerInfo.其它空间备注 = model.其它空间备注;
                customerInfo.其它空间家具 = model.其它空间家具;
                customerInfo.其它空间预算 = model.其它空间预算;
                customerInfo.喜欢产品 = model.喜欢产品;
                customerInfo.喜欢元素 = model.喜欢元素;
                customerInfo.销售讲解 = model.销售讲解;
                customerInfo.客厅预算备注 = model.客厅预算备注;
                customerInfo.客厅预算家具 = model.客厅预算家具;
                customerInfo.客厅预算金额 = model.客厅预算金额;
                customerInfo.户型大小 = model.户型大小;
                customerInfo.比较品牌产品 = model.比较品牌产品;
                customerInfo.比较品牌产品备注 = model.比较品牌产品备注;
                customerInfo.是否关闭 = model.是否关闭;
                customerInfo.是否成交 = model.是否成交;
                customerInfo.关闭备注 = model.关闭备注;
                var traklog = customerTrackingBLL.GetModels(p => p.接待记录ID == model.ID);
                if (traklog.Count() > 0)
                {
                    customerInfo.当前状态 = traklog.OrderByDescending(p => p.id).First().跟进结果;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return customerInfo;
        }



        /// <summary>
        /// 初始化客户接待信息
        /// </summary>
        private IQueryable<CustomerInfoModel> BuildCustomerInfoModels()
        {
            var em = SetEmployee();
            List<CustomerInfoModel> customerInfoModelsList = new List<CustomerInfoModel>();


            List<销售_接待记录> customer = new List<销售_接待记录>();
            
                customer = customerInfoBLL.GetModels(p => p.店铺ID == em.店铺ID).ToList();//查询当前店铺所有顾客接待信息

        
            

            foreach (var item in customer)
            {
                var model = BuildCustomerInfoModel(item);

                customerInfoModelsList.Add(model);
            }
            return customerInfoModelsList.AsEnumerable().AsQueryable();
        }

        /// <summary>
        /// 构建软装服务设计提交信息
        /// </summary>
        /// </summary>
        /// <param name="id">接待id</param>
        /// <returns>设计案提交表信息</returns>
        public IQueryable<DesignSubmitModel> BuildDesignSubInfo(int? id)
        {
            var em = SetEmployee();
            List<销售_设计案提交表> designSubModelList = new List<销售_设计案提交表>();
            if (em.职务ID == 3)
            {   //店长可以查看所有信息
                designSubModelList = designSubmitBLL.GetModels(p => p.店铺ID==em.店铺ID).ToList();
            }

            else if (id != 0 && id != null)
            {   //根据接待ID查询当前客户的设计提交表
                designSubModelList = designSubmitBLL.GetModels(p => p.接待记录ID == id).ToList();
            }
            else
            {
                //查询当前销售人员的设计提交表
                designSubModelList = designSubmitBLL.GetModels(p => p.销售人员 == em.ID).ToList();
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
                designSubmitModel.家居使用者 = item.家居使用者;
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
                if (item.店长 != null && item.店长审核 == true && item.设计人员 != null && item.设计人员审核 == true )
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

            var em = SetEmployee();
            List<DesignTackLogViewModel> designTackLogViewModels = new List<DesignTackLogViewModel>();
            try
            {
               List<销售_设计案跟进日志> designTackLogList =new List<销售_设计案跟进日志>() ;
             if (em.职务ID==3)
              {
                    designTackLogList = designTackLogList = designTrackingLogBLL.GetModels(p => p.店铺ID==em.店铺ID).ToList();
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
            var em = SetEmployee();
            List<DesignResultViewModel> designResultViews = new List<DesignResultViewModel>();
            List<设计_设计案完结单> models = new List<设计_设计案完结单>();
          
            if (role.是否店长 == true)
            {
                models = designResultBLL.GetModels(p => p.店铺ID == em.店铺ID).ToList();
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
                var files = design_ProjectDrawingsBLL.GetModels(p => p.接待ID == item.接待ID&& p.文件类型 == 8);
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
                        sellID = salesOrderBLL.GetModel(p => p.客户联系方式 == phoneNumber && p.店铺ID == em.店铺ID).ID;//根据联系方式查找相应的客户的订单ID

                        designResultViewModel.客户编号 = salesOrderBLL.GetModel(p => p.ID == sellID).合同编号;//合同编号

                        designResultViewModel.设计案提交表ID = item.设计案提交表ID;
                        designResultViewModel.销售单号 = salesOrderBLL.GetModel(p => p.ID == sellID).订单编号;//订单编号
                        designResultViewModel.单据编号 = salesOrderBLL.GetModel(p => p.ID == sellID).单据编号;//单据编号
                        designResultViewModel.接待ID = item.接待ID;

                    var lis = salesOrder_DetailsBLL.GetModels(p => p.单据ID == sellID).ToList();//根据订单ID查询订单详细
                    List<DesignResult_OrderDetailViewModel> list = new List<DesignResult_OrderDetailViewModel>();
                    foreach (var ite in lis)
                    {
                        DesignResult_OrderDetailViewModel model = new DesignResult_OrderDetailViewModel();
                        var spuid = product_SKUBLL.GetModel(p => p.ID == ite.SKU_ID).SPU_ID;
                        var productid = product_SPUBLL.GetModel(p => p.SPUID == spuid).商品ID;
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
                    catch (Exception e)
                    {
                        designResultViewModel=null;
                    }
                }
                if (designResultViewModel != null)
                {
                    designResultViews.Add(designResultViewModel);
                }
            }
            return designResultViews.AsQueryable();
        }


        /// <summary>
        /// 构建单个销售成交记录实体
        /// </summary>
        /// <returns></returns>
        private SalesRecordsViewModel BuildSalesRecordsModel(销售_接待成交单 model)
        {
            SalesRecordsViewModel salesRecords = new SalesRecordsViewModel();
            salesRecords.ID = model.ID;
            salesRecords.制单人员ID = model.制单人员ID;
            salesRecords.制单日期 = model.制单日期;
            salesRecords.合同单号 = model.合同单号;
            salesRecords.折扣 = model.折扣;
            salesRecords.接待记录ID = model.接待记录ID;
            salesRecords.更新人ID = model.更新人ID;
            salesRecords.更新日期 = model.更新日期;
            salesRecords.订库样 = model.订库样;
            salesRecords.销售人员ID = model.销售人员ID;
            salesRecords.销售人员 = storeEmployeesBLL.GetModel(p => p.ID == model.销售人员ID).姓名;
            salesRecords.销售单号 = model.销售单号;
            salesRecords.销售日期 = model.销售日期;
            salesRecords.销售金额 = model.销售金额;
            salesRecords.备注 = model.备注;
            salesRecords.是否全款 = model.是否全款;
            salesRecords.更新人 = storeEmployeesBLL.GetModel(p => p.ID == model.更新人ID).姓名;
            var m = customerInfoBLL.GetModel(p => p.ID == model.接待记录ID);
            salesRecords.客户姓名 = m.客户姓名;
            salesRecords.客户联系方式 = "[" + m.客户电话 + " ][" + m.社交软件 + "]";
            return salesRecords;
        }

        /// <summary>
        ///构建销售成交实体集 
        /// </summary>
        /// <returns></returns>
        private List<SalesRecordsViewModel> BuildSalesRecordsModels()
        {
            var em = SetEmployee();
            var models = salesRecordBLL.GetModels(p => p.店铺ID == em.店铺ID);
            List<SalesRecordsViewModel> lis = new List<SalesRecordsViewModel>();
            foreach (var item in models)
            {
                var m = BuildSalesRecordsModel(item);
                lis.Add(m);
            }
            return lis;
        }

    }
}