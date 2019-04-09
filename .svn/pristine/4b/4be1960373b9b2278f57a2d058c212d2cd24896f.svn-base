using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;
using ChicStoreManagement.WEB.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ChicStoreManagement.WEB.Controllers
{
    public class DesignResultController : Controller
    {
        private IDesignResultBLL designResultBLL;
        private IDesignResult_DealListingBLL designResult_DealListingBLL;
        private IDesignSubmitBLL designSubmitBLL;
        private IDesign_ProjectDrawingsBLL design_ProjectDrawingsBLL;
        private ICustomerInfoBLL customerInfoBLL;
        private IStoreBLL storeBLL;
        private IStoreEmployeesBLL storeEmployeesBLL;
        private ISalesOrderBLL salesOrderBLL;
        private ISalesOrder_detailsBLL salesOrder_DetailsBLL;
        private IProduct_SPUBLL product_SPUBLL;
        private IProduct_SKUBLL product_SKUBLL;
        private IProductBLL productBLL;
      

        private int employeeID;
        private string employeeName;
        private string store;
        private int storeID;
        public DesignResultController(IDesignResultBLL designResultBLL, IDesignResult_DealListingBLL designResult_DealListingBLL, IDesignSubmitBLL designSubmitBLL, IDesign_ProjectDrawingsBLL design_ProjectDrawingsBLL, ICustomerInfoBLL customerInfoBLL, IStoreBLL storeBLL, IStoreEmployeesBLL storeEmployeesBLL, ISalesOrderBLL salesOrderBLL, ISalesOrder_detailsBLL salesOrder_DetailsBLL, IProduct_SPUBLL product_SPUBLL, IProduct_SKUBLL product_SKUBLL, IProductBLL productBLL)
        {
            this.designResultBLL = designResultBLL;
            this.designResult_DealListingBLL = designResult_DealListingBLL;
            this.designSubmitBLL = designSubmitBLL;
            this.design_ProjectDrawingsBLL = design_ProjectDrawingsBLL;
            this.customerInfoBLL = customerInfoBLL;
            this.storeBLL = storeBLL;
            this.storeEmployeesBLL = storeEmployeesBLL;
            this.salesOrderBLL = salesOrderBLL;
            this.salesOrder_DetailsBLL = salesOrder_DetailsBLL;
            this.product_SPUBLL = product_SPUBLL;
            this.product_SKUBLL = product_SKUBLL;
            this.productBLL = productBLL;
        }

        public ActionResult DesignResultIndex( string sortOrder, string searchString, string currentFilter, int? page,int? applyid)
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
            if (BuildResultInfo(employeeID) !=null)
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
        /// 添加设计案完结清单-未确认完结
        /// </summary>
        /// <param name="subid">设计案提交ID</param>
        /// <returns></returns>
        public ActionResult AddDesignResultView(int subid) {
            Session["method"] = "N";
            SetEmployee();
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            if (storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID!=4 && storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID!=3)
            {
                return Content("<script>alert('您不具有操作权限！不能进行完结操作！');window.history.go(-1);</script>");
            }
            if (designResultBLL.GetModel(p=>p.设计案提交表ID==subid)!=null)
            {
                return Content("<script>alert('该设计已经完结！不可重复提交！');window.history.go(-1);</script>");
            }
            DesignResultViewModel designResultViewModel = new DesignResultViewModel();
            designResultViewModel.计划完成时间 = designSubmitBLL.GetModel(p => p.id == subid).项目预计完成时间;
            designResultViewModel.计划完成空间 = designSubmitBLL.GetModel(p => p.id == subid).家具空间;

            int sellID;//销售订单ID
            if (subid>0)
            {

                var phoneNumber = designSubmitBLL.GetModel(w => w.id == subid).联系方式;
                try
                {
                    sellID = salesOrderBLL.GetModel(p => p.客户联系方式 == phoneNumber && p.店铺ID == storeID).ID;//根据联系方式查找相应的客户的订单ID

                    designResultViewModel.客户编号 = salesOrderBLL.GetModel(p => p.ID == sellID).合同编号;//合同编号

                    designResultViewModel.设计案提交表ID = subid;
                    designResultViewModel.销售单号 = salesOrderBLL.GetModel(p => p.ID == sellID).订单编号;//订单编号
                    designResultViewModel.单据编号 = salesOrderBLL.GetModel(p => p.ID == sellID).单据编号;//单据编号

                }
                catch (Exception e)
                {
                    return Content("<script>alert('没有与客户相关订单信息！不能进行完结操作！请仔细查阅！');window.history.go(-1);</script>");
                }

                
                /*designResultViewModel.设计_设计案完结单_家具成交单 =*/
                var lis=salesOrder_DetailsBLL.GetModels(p => p.单据ID == sellID).ToList();//根据订单ID查询订单详细
                List<DesignResult_OrderDetailViewModel> list = new List<DesignResult_OrderDetailViewModel>();
                foreach (var item in lis)
                {
                    DesignResult_OrderDetailViewModel model = new DesignResult_OrderDetailViewModel();
                    var spuid = product_SKUBLL.GetModel(p => p.ID == item.SKU_ID).SPU_ID;
                    var productid = product_SPUBLL.GetModel(p => p.ID == spuid).商品ID;
                    var product = productBLL.GetModel(p => p.ID == productid);
                    model.产品型号 = product.编号;
                    model.单位 = product.计量单位;
                    model.成交价 = item.零售小计;
                    model.数量 = item.数量;
                    model.空间 = "";
                    list.Add(model);
                }
                designResultViewModel.DesignResult_OrderDetailViewModel = list;
                var files = design_ProjectDrawingsBLL.GetModels(p => p.设计案提交表ID == subid && p.文件类型 == 8);
                if (files.Count()>0)
                {
                    designResultViewModel.完成效果图 = true;
                    List<String> listPath = new List<string>();
                    foreach (var item in files)
                    {
                        listPath.Add(item.存储路径);
                    }
                    designResultViewModel.完成效果图路径 = listPath;
                }
                else
                {
                    designResultViewModel.完成效果图 = false;
                }
                var mid = designSubmitBLL.GetModel(p => p.id == subid).店长;
                designResultViewModel.店长 =storeEmployeesBLL.GetModel(p=>p.ID==mid).姓名 ;
                var emid = designSubmitBLL.GetModel(p => p.id == subid).销售人员;
                designResultViewModel.销售人员 = storeEmployeesBLL.GetModel(p => p.ID == emid).姓名;
               
            }
            return View(designResultViewModel);
        }

        /// <summary>
        /// 设计案完结单增加操作
        /// </summary>
        /// <param name="designResultViewModel"></param>
        /// <returns></returns>
        public ActionResult AddDesignResult(DesignResultViewModel designResultViewModel ) {
            Session["method"] = "Y";
            SetEmployee();
            if (storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID != 4 && storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID != 3)
            {
                return Content("<script>alert('您不具有操作权限！不能进行完结操作！');window.history.go(-1);</script>");
            }
            设计_设计案完结单 model = new 设计_设计案完结单
            {
                制单人 = employeeName,
                制单日期 = DateTime.Now,
                合计成交金额 = designResultViewModel.合计成交金额,
                实际完成时间 = designResultViewModel.实际完成时间,
                实际完成空间 = designResultViewModel.实际完成空间,
                审批状态 = designResultViewModel.审批状态,
                延期或无法完成原因 = designResultViewModel.延期或无法完成原因,
                建议 = designResultViewModel.建议,
                更新人 = employeeName,
                计划完成时间 = designResultViewModel.计划完成时间,
                计划完成空间 = designResultViewModel.计划完成空间,
                销售人员 = employeeID,
                销售人员确认日期 = DateTime.Now,
                店长 = designSubmitBLL.GetModel(p => p.id == designResultViewModel.设计案提交表ID).店长,
                设计师= designSubmitBLL.GetModel(p => p.id == designResultViewModel.设计案提交表ID).设计人员,
                设计案提交表ID = designResultViewModel.设计案提交表ID,
                销售单号 = designResultViewModel.销售单号,
                客户单号 = designResultViewModel.客户编号,
                单据编号 = designResultViewModel.单据编号,
                更新日期 = DateTime.Now,
                 店铺ID=storeID

            };
            if (ModelState.IsValid)
            {
                designResultBLL.Add(model);
                Session["method"] = "N";
            }
            else
            {

                List<string> sb = new List<string>();
                //获取所有错误的Key
                List<string> Keys = ModelState.Keys.ToList();
                //获取每一个key对应的ModelStateDictionary
                foreach (var key in Keys)
                {
                    var errors = ModelState[key].Errors.ToList();
                    //将错误描述添加到sb中
                    foreach (var error in errors)
                    {
                        sb.Add(error.ErrorMessage);
                    }
                }
                string msg = "添加完结单信息出错：";
                foreach (var item in sb)
                {
                    msg += item.ToString() + "<br/>";
                }
                return Content(msg);
            }

            return RedirectToAction("DesignResultIndex");
        }

        /// <summary>
        /// 设计案完结详细
        /// </summary>
        /// <param name="id">完结单ID</param>
        /// <returns></returns>
        public ActionResult DesignResultInfoView(int ?id,int? applyid) {
            Session["method"] = "N";
            SetEmployee();
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            设计_设计案完结单 model = new 设计_设计案完结单();
            if (id>0)
            {
            model =designResultBLL.GetModel(p => p.id == id);
            }
            else if (applyid>0)
            {
                model = designResultBLL.GetModel(p => p.设计案提交表ID == applyid);
            }
            else
            {
                model = null;
                
            }
            if (model==null)
            {
                return Content("<script>alert('暂无该设计服务的完结信息！');window.history.go(-1);</script>");
            }
            DesignResultViewModel designResultViewModel = new DesignResultViewModel();
            designResultViewModel.Id = model.id;
            designResultViewModel.制单人 = model.制单人;
            designResultViewModel.制单日期 = model.制单日期;
            designResultViewModel.单据编号 = model.单据编号;
            designResultViewModel.合计成交金额 = model.合计成交金额;
            designResultViewModel.实际完成时间 = model.实际完成时间;
            designResultViewModel.实际完成空间 = model.实际完成空间;
            designResultViewModel.审批状态 = model.审批状态;
            designResultViewModel.客户编号 = model.客户单号;
            designResultViewModel.店长 = storeEmployeesBLL.GetModel(p => p.ID == model.店长).姓名;
            designResultViewModel.店长审核日期 = model.店长确认日期;
            designResultViewModel.延期或无法完成原因 = model.延期或无法完成原因;
            designResultViewModel.建议 = model.建议;
            designResultViewModel.更新人 = model.更新人;
            designResultViewModel.计划完成时间 = model.计划完成时间;
            designResultViewModel.计划完成空间 = model.计划完成空间;
            designResultViewModel.设计师 = storeEmployeesBLL.GetModel(p => p.ID == model.设计师).姓名;
            designResultViewModel.设计师确认日期 = model.设计师确认日期;
            designResultViewModel.设计案提交表ID = model.设计案提交表ID;
            designResultViewModel.销售人员 = storeEmployeesBLL.GetModel(p => p.ID == model.销售人员).姓名;
            designResultViewModel.销售人员确认日期 = model.销售人员确认日期;
            designResultViewModel.销售单号 = model.销售单号;
            designResultViewModel.更新日期 = model.更新日期;
            
            var phoneNumber = designSubmitBLL.GetModel(w => w.id == model.设计案提交表ID).联系方式;
            var  sellID = salesOrderBLL.GetModel(p => p.客户联系方式 == phoneNumber && p.店铺ID == storeID).ID;
            var lis = salesOrder_DetailsBLL.GetModels(p => p.单据ID == sellID).ToList();//根据订单ID查询订单详细
            List<DesignResult_OrderDetailViewModel> list = new List<DesignResult_OrderDetailViewModel>();
            foreach (var item in lis)
            {
                DesignResult_OrderDetailViewModel m = new DesignResult_OrderDetailViewModel();
                var spuid = product_SKUBLL.GetModel(p => p.ID == item.SKU_ID).SPU_ID;
                var productid = product_SPUBLL.GetModel(p => p.ID == spuid).商品ID;
                var product = productBLL.GetModel(p => p.ID == productid);
                m.产品型号 = product.编号;
                m.单位 = product.计量单位;
                m.成交价 = item.零售小计;
                m.数量 = item.数量;
                m.空间 = "";
                list.Add(m);
            }
            designResultViewModel.DesignResult_OrderDetailViewModel = list;
            var files = design_ProjectDrawingsBLL.GetModels(p => p.设计案提交表ID == model.设计案提交表ID && p.文件类型 == 8);
            if (files.Count() > 0)
            {
                designResultViewModel.完成效果图 = true;
                List<String> listPath = new List<string>();
                foreach (var item in files)
                {
                    listPath.Add(item.存储路径);
                }
                designResultViewModel.完成效果图路径 = listPath;
            }
            else
            {
                designResultViewModel.完成效果图 = false;
            }

            var submodel = designSubmitBLL.GetModel(p => p.id == model.设计案提交表ID);
            designResultViewModel.楼盘具体位置 = submodel.楼盘具体位置;
            designResultViewModel.客户姓名 = submodel.客户姓名;
            designResultViewModel.联系方式 = submodel.联系方式;
            ViewBag.Person = submodel.客户姓名;
            return View(designResultViewModel);
        }

        /// <summary>
        /// 修改设计案完结单
        /// </summary>
        /// <param name="id">设计案完结单ID</param>
        /// <returns></returns>
        public ActionResult EditDesignResultView(int id) {
            Session["method"] = "N";
            SetEmployee();
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            if (storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID != 4 && storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID != 3)
            {
                return Content("<script>alert('您不具有操作权限！不能进行完结操作！');window.history.go(-1);</script>");
            }
            设计_设计案完结单 model = new 设计_设计案完结单();
            try
            {
                model = designResultBLL.GetModel(p => p.id == id);
            }
            catch (Exception)
            {

               return Content("<script>alert('没有与客户相关订单信息！不能进行完结操作！请仔细查阅！');window.history.go(-1);</script>");
            }
            DesignResultViewModel designResultViewModel = new DesignResultViewModel
            {
                Id = model.id,
                制单人 = model.制单人,
                制单日期 = model.制单日期,
                单据编号 = model.单据编号,
                合计成交金额 = model.合计成交金额,
                实际完成时间 = model.实际完成时间,
                实际完成空间 = model.实际完成空间,
                审批状态 = model.审批状态,
                客户编号 = model.客户单号,
                店长 = storeEmployeesBLL.GetModel(p => p.ID == model.店长).姓名,
                店长审核日期 = model.店长确认日期,
                延期或无法完成原因 = model.延期或无法完成原因,
                建议 = model.建议,
                更新人 = model.更新人,
                计划完成时间 = model.计划完成时间,
                计划完成空间 = model.计划完成空间,
                设计师 = storeEmployeesBLL.GetModel(p => p.ID == model.设计师).姓名,
                设计师确认日期 = model.设计师确认日期,
                设计案提交表ID = model.设计案提交表ID,
                销售人员 = storeEmployeesBLL.GetModel(p => p.ID == model.销售人员).姓名,
                销售人员确认日期 = model.销售人员确认日期,
                销售单号 = model.销售单号
            };
            var files = design_ProjectDrawingsBLL.GetModels(p => p.设计案提交表ID == model.设计案提交表ID && p.文件类型 == 8);
            if (files.Count() > 0)
            {
                designResultViewModel.完成效果图 = true;
            }
                return View(designResultViewModel);
        }

        /// <summary>
        /// 修改设计案完结单
        /// </summary>
        /// <param name="designResultViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDesignResult(DesignResultViewModel designResultViewModel ) {
            Session["method"] = "Y";
            SetEmployee();
            if (storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID != 4 && storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID != 3)
            {
                return Content("<script>alert('您不具有操作权限！不能进行完结操作！');window.history.go(-1);</script>");
            }
            设计_设计案完结单 model = new 设计_设计案完结单();
            model = designResultBLL.GetModel(p => p.id == designResultViewModel.Id);
            model.id = designResultViewModel.Id;
            model.制单人 = designResultViewModel.制单人;
            model.制单日期 = designResultViewModel.制单日期;
            model.单据编号 = designResultViewModel.单据编号;
            model.合计成交金额 = designResultViewModel.合计成交金额;
            model.实际完成时间 = designResultViewModel.实际完成时间;
            model.实际完成空间 = designResultViewModel.实际完成空间;
            model.审批状态 = designResultViewModel.审批状态;
            model.客户单号 = designResultViewModel.客户编号;
            model.店长 = storeEmployeesBLL.GetModel(p=>p.停用标志==false&&p.店铺ID==storeID&&p.是否店长==true).ID;
            
            model.店长确认日期 = designResultViewModel.店长审核日期;
            model.延期或无法完成原因 = designResultViewModel.延期或无法完成原因;
            model.建议 = designResultViewModel.建议;
            model.更新人 = employeeName;
            model.计划完成时间 = designResultViewModel.计划完成时间;
            model.计划完成空间 = designResultViewModel.计划完成空间;
            model.设计师 = storeEmployeesBLL.GetModel(p => p.停用标志 == false && p.店铺ID == storeID && p.是否设计师 == true).ID; ;
            model.设计师确认日期 = designResultViewModel.设计师确认日期;
            model.设计案提交表ID = designResultViewModel.设计案提交表ID;
            model.销售人员 = employeeID;
            model.销售人员确认日期 = designResultViewModel.销售人员确认日期;
            model.销售单号 = designResultViewModel.销售单号;
            model.更新日期 = DateTime.Now;
            if (ModelState.IsValid)
            {
                designResultBLL.Modify(model);
                Session["method"] = "N";
            }
            else
            {

                List<string> sb = new List<string>();
                //获取所有错误的Key
                List<string> Keys = ModelState.Keys.ToList();
                //获取每一个key对应的ModelStateDictionary
                foreach (var key in Keys)
                {
                    var errors = ModelState[key].Errors.ToList();
                    //将错误描述添加到sb中
                    foreach (var error in errors)
                    {
                        sb.Add(error.ErrorMessage);
                    }
                }
                string msg = "修改完结单信息出错：";
                foreach (var item in sb)
                {
                    msg += item.ToString() + "<br/>";
                }
                return Content(msg);
            }
            return RedirectToAction("DesignResultIndex");
        }
        /// <summary>
        /// 构建设计完结单信息
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        private IQueryable<DesignResultViewModel> BuildResultInfo(int employeeID)
        {
            var role=storeEmployeesBLL.GetModel(p => p.ID == employeeID);
            List<DesignResultViewModel> designResultViews = new List<DesignResultViewModel>();
            List<设计_设计案完结单> models = new List<设计_设计案完结单>();

            if (role.是否销售==true)
            {
                models = designResultBLL.GetModels(p => p.销售人员==employeeID).ToList();
            }
            if (role.是否店长==true)
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
                if (files.Count()>0)
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
                designResultViewModel.销售人员 = storeEmployeesBLL.GetModel(p=>p.ID==item.销售人员).姓名;
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
                int sellID;//销售订单ID
                if (item.设计案提交表ID > 0)
                {

                    var phoneNumber = designSubmitBLL.GetModel(w => w.id == item.设计案提交表ID).联系方式;
                    try
                    {
                        sellID = salesOrderBLL.GetModel(p => p.客户联系方式 == phoneNumber && p.店铺ID == storeID).ID;//根据联系方式查找相应的客户的订单ID

                        designResultViewModel.客户编号 = salesOrderBLL.GetModel(p => p.ID == sellID).合同编号;//合同编号

                        designResultViewModel.设计案提交表ID = item.设计案提交表ID;
                        designResultViewModel.销售单号 = salesOrderBLL.GetModel(p => p.ID == sellID).订单编号;//订单编号
                        designResultViewModel.单据编号 = salesOrderBLL.GetModel(p => p.ID == sellID).单据编号;//单据编号

                  

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
                    catch (Exception e)
                    {

                        designResultViewModel = null;
                    }
                }
                if (designResultViewModel!=null)
                {
                  designResultViews.Add(designResultViewModel);
                }
                    
            }
            return designResultViews.AsQueryable();
        }



        /// <summary>
        /// 设置当前的用户信息
        /// </summary>
        public void SetEmployee()
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
    }
}