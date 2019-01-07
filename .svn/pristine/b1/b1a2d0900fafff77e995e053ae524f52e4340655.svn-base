using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;
using ChicStoreManagement.WEB.ViewModel;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicStoreManagement.WEB.Controllers
{
    /// <summary>
    /// 设计师审查
    /// </summary>
    public class DesignerExamineController : Controller
    {
        private readonly ICustomerInfoBLL customerInfoBLL;
        private readonly ICustomerTrackingBLL customerTrackingBLL;
        private readonly IDesign_CustomerExceptedBuyBLL design_CustomerExceptedBuyBLL;
        private readonly IDesignSubmitBLL designSubmitBLL;
        private readonly IExceptedBuyBLL exceptedBuyBLL;
        private readonly IProductCodeBLL productCodeBLL;
        private readonly IStoreBLL storeBLL;
        private readonly IPositionBLL positionBLL;
        private readonly IStoreEmployeesBLL storeEmployeesBLL;
        private readonly IProductBLL productBLL;
        private readonly IProduct_SeriesBLL product_SeriesBLL;
        private readonly IProduct_BrandBLL product_BrandBLL;
        private readonly IProduct_ClassfiyBLL product_ClassfiyBLL;
        private ISalesOrderBLL salesOrderBLL;
        private ISalesOrder_detailsBLL salesOrder_DetailsBLL;
        private IProduct_SPUBLL product_SPUBLL;
        private IProduct_SKUBLL product_SKUBLL;
        private IDesignResultBLL designResultBLL;
        private IDesignResult_DealListingBLL designResult_DealListingBLL;
        private IDesign_ProjectDrawingsBLL design_ProjectDrawingsBLL;


        private int employeeID;
        private string employeeName;
        private string store;
        private int storeID;

        public DesignerExamineController(ICustomerInfoBLL customerInfoBLL, ICustomerTrackingBLL customerTrackingBLL, IDesign_CustomerExceptedBuyBLL design_CustomerExceptedBuyBLL, IDesignSubmitBLL designSubmitBLL, IExceptedBuyBLL exceptedBuyBLL, IProductCodeBLL productCodeBLL, IStoreBLL storeBLL, IPositionBLL positionBLL, IStoreEmployeesBLL storeEmployeesBLL, IProductBLL productBLL, IProduct_SeriesBLL product_SeriesBLL, IProduct_BrandBLL product_BrandBLL, IProduct_ClassfiyBLL product_ClassfiyBLL, ISalesOrderBLL salesOrderBLL, ISalesOrder_detailsBLL salesOrder_DetailsBLL, IProduct_SPUBLL product_SPUBLL, IProduct_SKUBLL product_SKUBLL, IDesignResultBLL designResultBLL, IDesignResult_DealListingBLL designResult_DealListingBLL,IDesign_ProjectDrawingsBLL design_ProjectDrawingsBLL)
        {
            this.customerInfoBLL = customerInfoBLL;
            this.customerTrackingBLL = customerTrackingBLL;
            this.design_CustomerExceptedBuyBLL = design_CustomerExceptedBuyBLL;
            this.designSubmitBLL = designSubmitBLL;
            this.exceptedBuyBLL = exceptedBuyBLL;
            this.productCodeBLL = productCodeBLL;
            this.storeBLL = storeBLL;
            this.positionBLL = positionBLL;
            this.storeEmployeesBLL = storeEmployeesBLL;
            this.productBLL = productBLL;
            this.product_SeriesBLL = product_SeriesBLL;
            this.product_BrandBLL = product_BrandBLL;
            this.product_ClassfiyBLL = product_ClassfiyBLL;
            this.salesOrderBLL = salesOrderBLL;
            this.salesOrder_DetailsBLL = salesOrder_DetailsBLL;
            this.product_SPUBLL = product_SPUBLL;
            this.product_SKUBLL = product_SKUBLL;
            this.designResultBLL = designResultBLL;
            this.designResult_DealListingBLL = designResult_DealListingBLL;
            this.design_ProjectDrawingsBLL = design_ProjectDrawingsBLL;
        }




        // GET: DesignerExamine
        /// <summary>
        /// 设计案申请审查-设计师
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult DesignApplyExamineIndex(string sortOrder, string searchString, string currentFilter, int? page)
        {

            Session["method"] = "N";
            SetEmployee();//获取当前人员信息
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            ViewBag.EmployeesID = employeeID;
            ViewBag.DesignSubCurrentSort = sortOrder;
            ViewBag.DesignSubDate = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.DesignSubResult = sortOrder == "last" ? "last_desc" : "last";
            List<DesignSubmitModel> designSubmitModels = new List<DesignSubmitModel>();
            //构建设计表信息  
            if (BuildDesignSubInfo(employeeID)!=null)
            {
                designSubmitModels = BuildDesignSubInfo(employeeID).ToList();

            }

            if (designSubmitModels == null)
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
                designSubmitModels = designSubmitModels.Where(w => w.联系方式 == searchString).ToList();
            }
            #region 排序，默认按ID升序
            switch (sortOrder)
            {
                case "first_desc":
                    designSubmitModels = designSubmitModels.OrderByDescending(w => w.项目提交时间).ToList();
                    break;
                case "last_desc":
                    designSubmitModels = designSubmitModels.OrderByDescending(w => w.审批状态).ToList();
                    break;
                case "last":
                    designSubmitModels = designSubmitModels.OrderBy(w => w.审批状态).ToList();
                    break;
                default:
                    designSubmitModels = designSubmitModels.OrderBy(w => w.项目提交时间).ToList();
                    break;
            }

            #endregion

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(designSubmitModels.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 设计案申请审查-设计师
        /// </summary>
        /// <param name="id"></param>
        /// <param name="examine"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DesignerApplyExamine(int id, string examine) {

            SetEmployee();
            if (storeEmployeesBLL.GetModel(p=>p.ID==employeeID).是否设计师 != true)
            {
                return Json("违规操作！");
            }
          
            var model = designSubmitBLL.GetModel(p => p.id == id);
            var isPass = JsonConvert.DeserializeObject<String>(examine);
            if (isPass == "true")
            {
                model.设计人员审核 = true;
                model.设计人员审核时间 = DateTime.Now;
            }
            if(isPass=="false")
            {
                model.设计人员审核 = false;
                model.设计人员审核时间 = DateTime.Now;
                
            }
            Session["method"] = "Y";
            try
            {
                string[] property = new string[2];
                property[0] = "设计人员审核";
                property[1] = "设计人员审核时间";
                designSubmitBLL.Modify(model, property);
            }
            catch (Exception e)
            {

                return Json("违规操作！"); 
            }
            
           
            return Json("提交成功！");
        }

        /// <summary>
        /// 设计案完结审查-设计师
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult DesignResultExamineIndex(string sortOrder, string searchString, string currentFilter, int? page)
        {
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DesignerResultExamine(int id,string examine) {

            SetEmployee();
            if (storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师 != true)
            {
                return Json("违规操作！");
            }

            var model = designResultBLL.GetModel(p => p.id == id);
            var isPass = JsonConvert.DeserializeObject<String>(examine);
            if (isPass == "true")
            {
                model.设计师确认 = true;
                model.设计师确认日期 = DateTime.Now;
            }
            if (isPass == "false")
            {
                model.设计师确认 = false;
                model.设计师确认日期 = DateTime.Now;

            }
            Session["method"] = "Y";
            try
            {
                string[] property = new string[2];
                property[0] = "设计师确认";
                property[1] = "设计师确认日期";
                designResultBLL.Modify(model, property);
            }
            catch (Exception e)
            {

                return Json("违规操作！");
            }


            return Json("提交成功！");
        }
        /// <summary>
        /// 设置当前的用户信息
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
            }
        }

        /// <summary>
        /// 设计案申请单-设计师
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private IQueryable<DesignSubmitModel> BuildDesignSubInfo(int id)
        {
            if (storeEmployeesBLL.GetModel(p=>p.ID == employeeID).是否设计师 == true)
            {
                var models = designSubmitBLL.GetModels(p => p.设计人员 == employeeID);
                List<DesignSubmitModel> list = new List<DesignSubmitModel>();
                foreach (var item in models)
                {

                    DesignSubmitModel designSubmitModel = new DesignSubmitModel();
                    designSubmitModel.Id = item.id;
                    designSubmitModel.客户喜好 = item.客户喜好或忌讳;
                    designSubmitModel.客户在意品牌或已购买品牌 = item.客户在意品牌或已购买品牌;
                    designSubmitModel.客户姓名 = item.客户姓名;
                    designSubmitModel.客户提问与要求 = item.客户提问与要求;
                    designSubmitModel.家具空间 = item.家具空间;
                    designSubmitModel.家庭成员 = item.家庭成员;
                    designSubmitModel.店长 = storeEmployeesBLL.GetModel(p => p.ID == item.店长).姓名;
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
                    designSubmitModel.设计人员 = storeEmployeesBLL.GetModel(p => p.ID == item.设计人员).姓名;
                    designSubmitModel.设计人员审核 = item.设计人员审核;
                    designSubmitModel.销售人员 = storeEmployeesBLL.GetModel(p => p.ID == item.销售人员).姓名;
                    designSubmitModel.面积大小 = item.面积大小;

                    designSubmitModel.预算 = item.预算.ToString();
                    designSubmitModel.项目提交时间 = item.项目提交时间;
                    designSubmitModel.项目量房时间 = item.项目量房时间;
                    designSubmitModel.项目预计完成时间 = item.项目预计完成时间;
                    designSubmitModel.备注 = item.备注;
                    if (item.店长 != null && item.店长审核 == true && item.设计人员 != null && item.设计人员审核 == true)
                    {
                        designSubmitModel.审批状态 = true;
                    }
                    list.Add(designSubmitModel);

                }
                return list.AsQueryable();
            }
            else { return null; }
        }


        /// <summary>
        /// 设计案完结信息-设计师
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        private IQueryable<DesignResultViewModel> BuildResultInfo(int employeeID)
        {
            var role = storeEmployeesBLL.GetModel(p => p.ID == employeeID);
            List<DesignResultViewModel> designResultViews = new List<DesignResultViewModel>();
            List<设计_设计案完结单> models = new List<设计_设计案完结单>();
            if (role.是否设计师 == true)
            {
                var lis= designSubmitBLL.GetModels(p => p.设计人员 == employeeID);
                foreach (var item in lis)
                {
                    设计_设计案完结单 model =new  设计_设计案完结单();
                    model = designResultBLL.GetModel(p => p.设计案提交表ID == item.id);
                    if (model!=null)
                    {
                        models.Add(model);
                    }
                   
                }
            }
            else
            {
                return null;
            }
            if (models==null||models.Count==0)
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

                designResultViewModel.设计案提交表ID = item.设计案提交表ID;
                designResultViewModel.销售人员 = storeEmployeesBLL.GetModel(p => p.ID == item.销售人员).姓名;
                designResultViewModel.销售人员确认日期 = item.销售人员确认日期;
                designResultViewModel.销售单号 = item.销售单号;//订单编号
                designResultViewModel.单据编号 = item.单据编号;//单据编号
                designResultViewModel.店长审核 = item.店长审核;
                designResultViewModel.设计师确认 = item.设计师确认;
                designResultViewModel.销售审核 = item.销售审核;
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
                var m = designSubmitBLL.GetModel(p => p.id == item.设计案提交表ID);
                designResultViewModel.客户姓名 = m.客户姓名;
                designResultViewModel.楼盘具体位置 = m.楼盘具体位置;
                designResultViewModel.联系方式 = m.联系方式;
               
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