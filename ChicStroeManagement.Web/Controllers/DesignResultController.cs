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


        private int employeeID;
        private string employeeName;
        private string store;
        private int storeID;
        public DesignResultController(IDesignResultBLL designResultBLL, IDesignResult_DealListingBLL designResult_DealListingBLL, IDesignSubmitBLL designSubmitBLL, IDesign_ProjectDrawingsBLL design_ProjectDrawingsBLL, ICustomerInfoBLL customerInfoBLL, IStoreBLL storeBLL, IStoreEmployeesBLL storeEmployeesBLL, ISalesOrderBLL salesOrderBLL, ISalesOrder_detailsBLL salesOrder_DetailsBLL, IProduct_SPUBLL product_SPUBLL, IProduct_SKUBLL product_SKUBLL)
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
        }

        public ActionResult DesignResultIndex( string sortOrder, string searchString, string currentFilter, int? page)
        {
            Session["method"] = "N";
            SetEmployee();//获取当前人员信息
            
            
            ViewBag.EmployeesID = employeeID;
            ViewBag.DesignResultCurrentSort = sortOrder;
            ViewBag.DesignResultDate = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.DesignResult = sortOrder == "last" ? "last_desc" : "last";
            List<DesignResultViewModel> designResultModels = new List<DesignResultViewModel>();
            //构建设计表信息  
            designResultModels = BuildResultInfo(employeeID).ToList();
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
        public ActionResult AddDesignResultView(int? subid) {
            Session["method"] = "N";
            SetEmployee();
            if (storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID!=4 && storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID!=3)
            {
                return Content("<script>alert('您不具有操作权限！不能进行完结操作！');window.history.go(-1);</script>");
            }
       
            DesignResultViewModel designResultViewModel = new DesignResultViewModel();
            int salsID;
            if (subid!=null&&subid!=0)
            {

                var phoneNumber = designSubmitBLL.GetModel(w => w.id == subid).联系方式;
                try
                {
                    salsID = salesOrderBLL.GetModel(p => p.客户联系方式 == phoneNumber && p.店铺ID == storeID).ID;//根据联系方式查找相应的客户的订单

                    designResultViewModel.客户编号 = salesOrderBLL.GetModel(p => p.ID == salsID).合同编号;//合同编号

                    designResultViewModel.设计案提交表ID = subid;
                    designResultViewModel.销售单号 = salesOrderBLL.GetModel(p => p.ID == salsID).订单编号;//订单编号
                    designResultViewModel.单据编号 = salesOrderBLL.GetModel(p => p.ID == salsID).单据编号;//单据编号

                }
                catch (Exception e)
                {
                    return Content("<script>alert('没有与客户相关订单信息！不能进行完结操作！请仔细查阅！');window.history.go(-1);</script>");
                }
                designResultViewModel.计划完成时间 = designSubmitBLL.GetModel(p => p.id == subid).项目预计完成时间;
                designResultViewModel.计划完成空间 = designSubmitBLL.GetModel(p => p.id == subid).家具空间;
                设计_设计案完结单_家具成交单 model = new 设计_设计案完结单_家具成交单();
                /*designResultViewModel.设计_设计案完结单_家具成交单 =*/
                var lis=salesOrder_DetailsBLL.GetModels(p => p.单据ID == salsID).ToList();
                foreach (var item in lis)
                {
                    var spuid = product_SKUBLL.GetModel(p => p.ID == item.SKU_ID).SPU_ID;
                    var productid = product_SPUBLL.GetModel(p => p.ID == spuid).商品ID;
                   ///
                   ////
                   ///
                  
                }
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
                更新人 = designResultViewModel.更新人,
                计划完成时间 = designResultViewModel.计划完成时间,
                计划完成空间 = designResultViewModel.计划完成空间,
                设计师 = employeeName,
                设计师确认日期 = DateTime.Now,
                设计案提交表ID = designResultViewModel.设计案提交表ID,
                销售单号 = designResultViewModel.销售单号,
                客户单号 = designResultViewModel.客户编号,
                单据编号 = designResultViewModel.单据编号
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
        /// 修改设计案完结单
        /// </summary>
        /// <param name="id">设计案完结单ID</param>
        /// <returns></returns>
        public ActionResult EditDesignResultView(int id) {
            Session["method"] = "N";
            SetEmployee();
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
                店长 = model.店长,
                店长审核日期 = model.店长确认日期,
                延期或无法完成原因 = model.延期或无法完成原因,
                建议 = model.建议,
                更新人 = model.更新人,
                计划完成时间 = model.计划完成时间,
                计划完成空间 = model.计划完成空间,
                设计师 = model.设计师,
                设计师确认日期 = model.设计师确认日期,
                设计案提交表ID = model.设计案提交表ID,
                销售人员 = model.销售人员,
                销售人员确认日期 = model.销售人员确认日期,
                销售单号 = model.销售单号
            };

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
            model.id = designResultViewModel.Id;
            model.制单人 = designResultViewModel.制单人;
            model.制单日期 = designResultViewModel.制单日期;
            model.单据编号 = designResultViewModel.单据编号;
            model.合计成交金额 = designResultViewModel.合计成交金额;
            model.实际完成时间 = designResultViewModel.实际完成时间;
            model.实际完成空间 = designResultViewModel.实际完成空间;
            model.审批状态 = designResultViewModel.审批状态;
            model.客户单号 = designResultViewModel.客户编号;
            model.店长 = designResultViewModel.店长;
            model.店长确认日期 = designResultViewModel.店长审核日期;
            model.延期或无法完成原因 = designResultViewModel.延期或无法完成原因;
            model.建议 = designResultViewModel.建议;
            model.更新人 = designResultViewModel.更新人;
            model.计划完成时间 = designResultViewModel.计划完成时间;
            model.计划完成空间 = designResultViewModel.计划完成空间;
            model.设计师 = designResultViewModel.设计师;
            model.设计师确认日期 = designResultViewModel.设计师确认日期;
            model.设计案提交表ID = designResultViewModel.设计案提交表ID;
            model.销售人员 = designResultViewModel.销售人员;
            model.销售人员确认日期 = designResultViewModel.销售人员确认日期;
            model.销售单号 = designResultViewModel.销售单号;
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
            var positionid=storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID;
            List<DesignResultViewModel> designResultViews = new List<DesignResultViewModel>(); List<设计_设计案完结单> models = new List<设计_设计案完结单>();
            if (positionid==4||positionid==5)
            {
                models = designResultBLL.GetModels(p => p.设计师 == employeeName).ToList();
            }
            foreach (var item in models)
            {
                DesignResultViewModel designResultViewModel = new DesignResultViewModel();
                designResultViewModel.Id = item.id;
                designResultViewModel.制单日期 = item.制单日期;
                designResultViewModel.合计成交金额 = item.合计成交金额;

                if (design_ProjectDrawingsBLL.GetModel(p => p.设计案提交表ID == item.设计案提交表ID && p.文件类型 == 8) != null)
                {
                    designResultViewModel.完成效果图路径 = design_ProjectDrawingsBLL.GetModel(p => p.设计案提交表ID == item.设计案提交表ID && p.文件类型 == 8).存储路径;
                }
                designResultViewModel.实际完成时间 = item.实际完成时间;
                designResultViewModel.实际完成空间 = item.实际完成空间;
                designResultViewModel.审批状态 = item.审批状态;
                designResultViewModel.客户编号 = salesOrderBLL.GetModel(p => p.客户联系方式 == designSubmitBLL.GetModel(w => w.id == item.设计案提交表ID).联系方式 && p.店铺ID == customerInfoBLL.GetModel(r => r.ID == designSubmitBLL.GetModel(w => w.id == item.设计案提交表ID).接待记录ID).店铺ID).合同编号;//合同编号

                designResultViewModel.设计案提交表ID = item.设计案提交表ID;
                designResultViewModel.销售人员 = item.销售人员;
                designResultViewModel.销售人员确认日期 = item.销售人员确认日期;
                designResultViewModel.销售单号 = salesOrderBLL.GetModel(p => p.客户联系方式 == designSubmitBLL.GetModel(w => w.id == item.设计案提交表ID).联系方式 && p.店铺ID == customerInfoBLL.GetModel(r => r.ID == designSubmitBLL.GetModel(w => w.id == item.设计案提交表ID).接待记录ID).店铺ID).订单编号;//订单编号
                designResultViewModel.单据编号 = salesOrderBLL.GetModel(p => p.客户联系方式 == designSubmitBLL.GetModel(w => w.id == item.设计案提交表ID).联系方式 && p.店铺ID == customerInfoBLL.GetModel(r => r.ID == designSubmitBLL.GetModel(w => w.id == item.设计案提交表ID).接待记录ID).店铺ID).单据编号;//单据编号

                designResultViewModel.店长 = item.店长;
                designResultViewModel.店长审核日期 = item.店长确认日期;
                designResultViewModel.延期或无法完成原因 = item.延期或无法完成原因;
                designResultViewModel.建议 = item.建议;
                designResultViewModel.更新人 = item.更新人;
                designResultViewModel.计划完成时间 = item.计划完成时间;
                designResultViewModel.计划完成空间 = item.计划完成空间;
                designResultViewModel.设计师 = item.设计师;
                designResultViewModel.设计师确认日期 = item.设计师确认日期;
                designResultViewModel.设计_设计案完结单_家具成交单 = designResult_DealListingBLL.GetModels(p => p.设计案完结单 == item.id).ToList();

                designResultViews.Add(designResultViewModel);
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