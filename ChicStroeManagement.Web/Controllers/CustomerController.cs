﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;
using ChicStoreManagement.WEB.ViewModel;
using Newtonsoft.Json;
using PagedList;

namespace ChicStoreManagement.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerInfoBLL customerInfoBLL;
        private readonly IExceptedBuyBLL exceptedBuyBLL;
        private readonly IStoreEmployeesBLL storeEmployeesBLL;
        private readonly IStoreBLL storeBLL;
        private readonly IProductCodeBLL productCodeBLL;
        private readonly ICustomerTrackingBLL customerTrackingBLL;
        private readonly ITrackGoalBLL trackGoalBLL;
        private readonly ISalesRecordBLL salesRecordBLL;
        private readonly ISalesOrderBLL salesOrderBLL;
       private readonly ISalesOrder_detailsBLL salesOrder_DetailsBLL;
        private readonly IProduct_SPUBLL product_SPUBLL;
       private readonly  IProduct_SKUBLL product_SKUBLL;
       
        private readonly ISystemAccountBLL systemAccountBLL;

        public CustomerController(ICustomerInfoBLL customerInfoBLL, IExceptedBuyBLL exceptedBuyBLL, IStoreEmployeesBLL storeEmployeesBLL, IStoreBLL storeBLL, IProductCodeBLL productCodeBLL, ICustomerTrackingBLL customerTrackingBLL, ITrackGoalBLL trackGoalBLL, ISalesRecordBLL salesRecordBLL, ISalesOrderBLL salesOrderBLL, ISalesOrder_detailsBLL salesOrder_DetailsBLL, IProduct_SPUBLL product_SPUBLL, IProduct_SKUBLL product_SKUBLL, ISystemAccountBLL systemAccountBLL)
        {
            this.customerInfoBLL = customerInfoBLL;
            this.exceptedBuyBLL = exceptedBuyBLL;
            this.storeEmployeesBLL = storeEmployeesBLL;
            this.storeBLL = storeBLL;
            this.productCodeBLL = productCodeBLL;
            this.customerTrackingBLL = customerTrackingBLL;
            this.trackGoalBLL = trackGoalBLL;
            this.salesRecordBLL = salesRecordBLL;
            this.salesOrderBLL = salesOrderBLL;
            this.salesOrder_DetailsBLL = salesOrder_DetailsBLL;
            this.product_SPUBLL = product_SPUBLL;
            this.product_SKUBLL = product_SKUBLL;
            this.systemAccountBLL = systemAccountBLL;
        }











        // GET: Customer
        /// <summary>
        /// 展示客户信息
        /// </summary>
        /// <returns></returns>
        public ViewResult CustomerIndex(int? page, string searchString, bool? IsExcept, bool? IsDeal, string startDate, string EndDate)
        {
            Session["method"] = "N";
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            int pageSize = 25;
            int pageNumber = (page ?? 1);
            List<CustomerInfoModel> customerInfoModels = new List<CustomerInfoModel>();


            var m = BuildCustomerInfoModels();//将顾客接待信息数据优化
            if (m.Count() <= 0)
            {
                return View(customerInfoModels.ToPagedList(pageNumber, pageSize));
            }
            customerInfoModels = m.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Replace(" ", "");
                customerInfoModels = customerInfoModels.Where(w => w.客户电话 == searchString).ToList();//通过电话查找
                ViewBag.SearchString = searchString;
            }
            if (IsExcept == true)
            {
                customerInfoModels = customerInfoModels.Where(w => w.是否有意向 == true).ToList();//通过电话查找
                ViewBag.IsExcept = IsExcept;
            }
            if (IsDeal == true)
            {
                customerInfoModels = customerInfoModels.Where(w => w.是否成交 == true).ToList();//通过电话查找
                ViewBag.IsDeal = IsDeal;
            }
            if (startDate != null && startDate != "")
            {
                ViewBag.StartDate = startDate;
                DateTime startTime = Convert.ToDateTime(startDate);
                customerInfoModels = customerInfoModels.Where(w => w.接待日期 > startTime).ToList();
            }
            if (EndDate != null && EndDate != "")
            {
                ViewBag.EndDate = EndDate;
                DateTime endTime = Convert.ToDateTime(EndDate);
                customerInfoModels = customerInfoModels.Where(w => w.接待日期 < endTime).ToList();
            }

            customerInfoModels = customerInfoModels.OrderByDescending(p => p.接待日期).ToList();
           

            ViewBag.employeeName = em.姓名;//给前端传入当前操作人
            
            int? goals = em.跟进目标计划数;
            ViewBag.Goals = goals;//总共跟进数目
            int n = customerInfoBLL.GetModels(p => p.跟进人ID == em.ID).Count();
            ViewBag.AvailableGoals = goals - n;//剩余跟进数目


            return View(customerInfoModels.ToPagedList(pageNumber, pageSize));

        }


        /// <summary>
        /// 添加客户
        /// </summary>
        /// <returns></returns>
        public ViewResult AddCustomerView()
        {
            Session["method"] = "N";
            var em = SetEmployee();
            ViewBag.Store = em.店铺;
            ViewBag.Employee = em.姓名;
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            List<销售_店铺员工档案> employeeList = storeEmployeesBLL.GetModels(p => true).ToList();
            SelectList EmployeeListList = new SelectList(employeeList, "ID", "姓名");
            ViewBag.EmployeeOptions = EmployeeListList;

            var selectAgeList = new List<SelectListItem>() { new SelectListItem() { Value = "00后", Text = "00后" }, new SelectListItem() { Value = "90后", Text = "90后" }, new SelectListItem() { Value = "80后", Text = "80后" }, new SelectListItem() { Value = "70后", Text = "70后" } };
            ViewBag.AgeOptions = selectAgeList;

            var productList = productCodeBLL.GetModels(p => true).ToList();
            SelectList productSelectListItems = new SelectList(productList, "型号", "型号");
            ViewBag.ProductOptions = productSelectListItems;

            CustomerInfoModel model = new CustomerInfoModel
            {
                接待序号 = Guid.NewGuid().ToString("D"),
                制单日期 = DateTime.Now,
                出店时间 = DateTime.Now,
                进店时间 = DateTime.Now,
                预计使用时间 = DateTime.Now,
                接待日期 = DateTime.Now,
                店铺 = em.店铺,
                接待人 = em.姓名
            };

            return View(model);
        }

        /// <summary>
        /// 客户信息添加操作
        /// </summary>
        /// <param name="customerInfoModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomerAdd(CustomerInfoModel customerInfoModel)
        {
            var em = SetEmployee();
            if (Session["method"].ToString() == "Y")
            {
                string str = string.Format("<script>alert('重复操作！');parent.location.href='CustomerIndex';</script>");
                return Content(str);
            }

            SetEmployee();
            销售_接待记录 model = new 销售_接待记录();
            try
            {

                model.店铺ID = em.店铺ID;
                model.接待人ID = em.ID;
                model.接待序号 = customerInfoModel.接待序号;
                model.接待日期 = customerInfoModel.接待日期;
                model.主导者 = customerInfoModel.主导者;
                model.主导者喜好风格 = customerInfoModel.主导者喜好风格;
                model.使用空间 = customerInfoModel.使用空间;
                model.出店时间 = customerInfoModel.出店时间;
                model.制单日期 = DateTime.Now;
                model.同行人 = customerInfoModel.同行人;
                model.如何得知品牌 = customerInfoModel.如何得知品牌;
                model.安装地址 = customerInfoModel.安装地址;
                model.客户姓名 = customerInfoModel.客户姓名;
                model.客户建议 = customerInfoModel.客户建议;
                model.客户来源 = customerInfoModel.客户来源;
                if (customerInfoModel.客户电话 != null)
                {
                    model.客户电话 = customerInfoModel.客户电话.Replace(" ", "");
                }
                model.客户着装 = customerInfoModel.客户着装;
                model.客户类别 = customerInfoModel.客户类别;
                model.客户类型 = customerInfoModel.客户类型;
                model.客户职业 = customerInfoModel.客户职业;
                model.客户目的 = customerInfoModel.目的;
                model.家居使用者 = customerInfoModel.家居使用者;
                model.年龄段 = customerInfoModel.年龄段;
                model.性别 = customerInfoModel.性别;
                model.是否有意向 = customerInfoModel.是否有意向;
                model.是否确认 = customerInfoModel.是否确认;
                model.更新人 = em.ID;
                model.更新日期 = DateTime.Now;
                model.来店次数 = customerInfoModel.来店次数;
                model.比较品牌 = customerInfoModel.比较品牌;
                model.特征 = customerInfoModel.特征;
                model.社交软件 = customerInfoModel.社交软件;
                model.装修情况 = customerInfoModel.装修情况;
                model.装修进度 = customerInfoModel.装修进度;
                model.装修风格 = customerInfoModel.装修风格;
                model.设计师 = customerInfoModel.设计师;
                var goal = em.跟进目标计划数;//分配的跟进人次
                var trackmodel = customerInfoBLL.GetModels(p => p.跟进人ID == em.ID);
                int hgoal = 0;
                if (trackmodel!=null)
                {
                    hgoal = trackmodel.Count();//已经跟进的人次
                }
                
               

                if (goal > hgoal && model.是否有意向 == true)
                {
                    model.跟进人ID = em.ID;//初次添加且当前接待人员跟进指标未满，跟进人为当前接待人员。否则为空。
                }



                model.返点 = customerInfoModel.返点;
                model.进店时长 = Convert.ToInt32((customerInfoModel.出店时间 - customerInfoModel.进店时间).TotalMinutes);
                model.进店时间 = customerInfoModel.进店时间;
                model.预报价折扣 = customerInfoModel.预报价折扣;
                model.预算金额 = customerInfoModel.预算金额;
                model.预计使用时间 = customerInfoModel.预计使用时间;
                model.不喜欢产品 = customerInfoModel.不喜欢产品;
                model.不喜欢元素 = customerInfoModel.不喜欢元素;
                model.卧室预算备注 = customerInfoModel.主卧预算备注;
                model.卧室预算家具 = customerInfoModel.主卧预算家具;
                model.卧室预算金额 = customerInfoModel.主卧预算金额;
                model.餐厅预算备注 = customerInfoModel.餐厅预算备注;
                model.餐厅预算家具 = customerInfoModel.餐厅预算家具;
                model.餐厅预算金额 = customerInfoModel.餐厅预算金额;
                model.其它空间备注 = customerInfoModel.其它空间备注;
                model.其它空间家具 = customerInfoModel.其它空间家具;
                model.其它空间预算 = customerInfoModel.其它空间预算;
                model.喜欢产品 = customerInfoModel.喜欢产品;
                model.喜欢元素 = customerInfoModel.喜欢元素;
                model.来店次数 = customerInfoModel.来店次数;
                model.客厅预算备注 = customerInfoModel.客厅预算备注;
                model.客厅预算家具 = customerInfoModel.客厅预算家具;
                model.客厅预算金额 = customerInfoModel.客厅预算金额;
                model.户型大小 = customerInfoModel.户型大小;
                model.销售讲解 = customerInfoModel.销售讲解;
                model.比较品牌产品 = customerInfoModel.比较品牌产品;
                model.比较品牌产品备注 = customerInfoModel.比较品牌产品备注;
            }
            catch (Exception e)
            {

                return Content(e.Message);
            }
            if (!ModelState.IsValid)
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
                string s = null;
                foreach (var item in sb)
                {
                    s += item.ToString() + ";";
                }
                return Content("<script>alert('" + s + "');window.history.go(-1);</script>");
            }
            //var isHave = customerInfoBLL.GetModel(p => p.客户姓名 == model.客户姓名 && p.接待日期 == model.接待日期);
            //if (isHave!= null)
            //{
            //    return Content("<script>alert('数据已存在！，请勿重复提交');parent.location.href='CustomerIndex';</script>");
            //}
            #region 如果客户已在当前店铺登记过
            try
            {
                var lis1 = customerInfoBLL.GetModels(p => p.店铺ID == em.店铺ID);
                lis1 = lis1.Where(p => p.客户姓名 == model.客户姓名 && p.客户电话 == model.客户电话);
                if (lis1.Count() > 0) //统计客户来店登记次数
                {
                    int num1 = lis1.Count();

                    model.来店次数 = lis1.OrderBy(p => p.ID).First().来店次数 + num1;
                }
            }
            catch (Exception e)
            {

                return Content(e.Message);
            }
            #endregion
            try
            {
                customerInfoBLL.Add(model); ;
            }
            catch (Exception e)
            {

                return Content("<script>alert('" + e.Message + "');window.history.go(-1);</script>");
            }

            Session["method"] = "Y";

            return RedirectToAction("CustomerIndex");
        }
        /// <summary>
        /// 修改客户信息视图
        /// </summary>
        /// <returns></returns>
        public ActionResult EditCustomerView(int id)
        {
            Session["method"] = "N";
            var em = SetEmployee();
            ViewBag.Store = em.店铺;
            ViewBag.Employee = em.姓名;
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerInfoModel customerInfo = new CustomerInfoModel();
            try
            {



                var model = customerInfoBLL.GetModel(p => p.ID == id);
                customerInfo = BuildCustomerInfoModel(model);

                if (customerInfo.接待人ID == em.ID || customerInfo.跟进人ID == em.ID || em.是否店长 == true)
                {
                    if (BuildExceptedBuy(id) != null)
                    {
                        var exceptedBuyModels = BuildExceptedBuy(id);
                        customerInfo.CustomerExceptedBuyModels = exceptedBuyModels;
                       
                    }
                    customerInfo.更新人 = em.姓名;
                    customerInfo.更新日期 = DateTime.Now;
                    return View(customerInfo);
                }

            }
            catch (Exception e)
            {

                return Content("<script>alert('" + e.Message + "');window.history.go(-1);</script>");
            }

            return Content("<script>alert('无操作权限！！');window.history.go(-1);</script>");

        }


        /// <summary>
        /// 提交修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerEdit(CustomerInfoModel customerInfoModel)
        {
            var em = SetEmployee();
            if (Session["method"].ToString() == "Y")
            {
                string str = string.Format("<script>alert('重复操作！');parent.location.href='CustomerIndex';</script>");
                return Content(str);
            }
            if (customerInfoModel == null)
            {
                return Content("<script>alert('违规操作！！');window.history.go(-1);</script>");
            }
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo
            {
                ShortDatePattern = "yyyy/MM/dd"
            };
            var model = customerInfoBLL.GetModel(p => p.ID == customerInfoModel.ID);
            try
            {
                model.接待日期 = Convert.ToDateTime(customerInfoModel.接待日期, dtFormat);
                model.主导者 = customerInfoModel.主导者;
                model.主导者喜好风格 = customerInfoModel.主导者喜好风格;
                model.使用空间 = customerInfoModel.使用空间;
                model.出店时间 = customerInfoModel.出店时间;
                model.同行人 = customerInfoModel.同行人;
                model.如何得知品牌 = customerInfoModel.如何得知品牌;
                model.安装地址 = customerInfoModel.安装地址;
                model.客户姓名 = customerInfoModel.客户姓名;
                model.客户建议 = customerInfoModel.客户建议;
                model.客户来源 = customerInfoModel.客户来源;
                model.客户电话 = customerInfoModel.客户电话;
                model.客户着装 = customerInfoModel.客户着装;
                model.客户类别 = customerInfoModel.客户类别;
                model.客户类型 = customerInfoModel.客户类型;
                model.客户职业 = customerInfoModel.客户职业;
                model.家居使用者 = customerInfoModel.家居使用者;
                model.年龄段 = customerInfoModel.年龄段;
                model.性别 = customerInfoModel.性别;
                model.是否有意向 = customerInfoModel.是否有意向;
                model.是否确认 = customerInfoModel.是否确认;
                model.更新人 = em.ID;
                model.更新日期 = DateTime.Now;
                model.来店次数 = customerInfoModel.来店次数;
                model.客户目的 = customerInfoModel.目的;
                model.比较品牌 = customerInfoModel.比较品牌;
                model.特征 = customerInfoModel.特征;
                model.社交软件 = customerInfoModel.社交软件;
                model.装修情况 = customerInfoModel.装修情况;
                model.装修进度 = customerInfoModel.装修进度;
                model.装修风格 = customerInfoModel.装修风格;
                model.设计师 = customerInfoModel.设计师;
                model.关闭备注 = customerInfoModel.关闭备注;
                model.是否关闭 = customerInfoModel.是否关闭;
                model.是否成交 = customerInfoModel.是否成交;
                model.返点 = customerInfoModel.返点;
                model.进店时长 = Convert.ToInt32((customerInfoModel.出店时间 - customerInfoModel.进店时间).TotalMinutes);
                model.进店时间 = customerInfoModel.进店时间;
                model.预报价折扣 = customerInfoModel.预报价折扣;
                model.预算金额 = customerInfoModel.预算金额;
                model.预计使用时间 = customerInfoModel.预计使用时间;
                model.不喜欢产品 = customerInfoModel.不喜欢产品;
                model.不喜欢元素 = customerInfoModel.不喜欢元素;
                model.卧室预算备注 = customerInfoModel.主卧预算备注;
                model.卧室预算家具 = customerInfoModel.主卧预算家具;
                model.卧室预算金额 = customerInfoModel.主卧预算金额;

                model.其它空间备注 = customerInfoModel.其它空间备注;
                model.其它空间家具 = customerInfoModel.其它空间家具;
                model.其它空间预算 = customerInfoModel.其它空间预算;
                model.喜欢产品 = customerInfoModel.喜欢产品;
                model.喜欢元素 = customerInfoModel.喜欢元素;
                model.餐厅预算备注 = customerInfoModel.餐厅预算备注;
                model.餐厅预算家具 = customerInfoModel.餐厅预算家具;
                model.餐厅预算金额 = customerInfoModel.餐厅预算金额;
                model.客厅预算备注 = customerInfoModel.客厅预算备注;
                model.客厅预算家具 = customerInfoModel.客厅预算家具;
                model.客厅预算金额 = customerInfoModel.客厅预算金额;
                model.户型大小 = customerInfoModel.户型大小;
                model.销售讲解 = customerInfoModel.销售讲解;
                model.比较品牌产品 = customerInfoModel.比较品牌产品;
                model.比较品牌产品备注 = customerInfoModel.比较品牌产品备注;
                if (ModelState.IsValid)
                {
                    customerInfoBLL.Modify(model);
                    Session["method"] = "Y";
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
                    string s = null;
                    foreach (var item in sb)
                    {
                        s += item.ToString() + "<br/>";
                    }
                    return Content("<script>alert('" + s + "');window.history.go(-1);</script>");
                }
            }
            catch (Exception e)
            {

                return Content("<script>alert('" + e.Message + "');window.history.go(-1);</script>"); ;
            }
            return RedirectToAction("CustomerIndex");
        }

        /// <summary>
        /// 申请跟进数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApplyGoals()
        {
            var em = SetEmployee();

            销售_跟进目标数申请表 model = new 销售_跟进目标数申请表
            {
                申请跟进数 = 1,
                员工ID = em.ID,
                店铺ID = em.店铺ID,
                更新日期 = DateTime.Now

            };
            try
            {
                trackGoalBLL.Add(model);
            }
            catch (Exception e)
            {

                return Json(e.Message);
            }

            return Json("申请已提交！");
        }

        /// <summary>
        /// 显示详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowVisitInfo(int id)
        {
            if (id == 0)
            {
                return View("查询ID不存在！");
            }
            var em = SetEmployee();
            ViewBag.Store = em.店铺;
            ViewBag.Employee = em.姓名;
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            var model = customerInfoBLL.GetModel(p => p.ID == id);
            CustomerInfoModel customerInfo = new CustomerInfoModel();
            customerInfo = BuildCustomerInfoModel(model);

            if (BuildExceptedBuy(id) != null)
            {
                var exceptedBuyModels = BuildExceptedBuy(id);
                customerInfo.CustomerExceptedBuyModels = exceptedBuyModels;
            }

            return View(customerInfo);

        }

        #region 预计购买操作
        /// <summary>
        /// 预计购买
        /// </summary>
        /// <param name="id">接待id</param>
        /// <returns></returns>
        public ActionResult ExceptedBuyIndex(int id)
        {
            Session["method"] = "N";
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var em = SetEmployee();
            List<CustomerExceptedBuyModel> exceptedBuyModels = new List<CustomerExceptedBuyModel>();
            if (BuildExceptedBuy(id) != null)
            {
                exceptedBuyModels = BuildExceptedBuy(id).ToList();
            }
            ViewBag.Store = em.店铺;
            ViewBag.Employee = em.姓名;
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            ViewBag.receptionid = id;
            CustomerInfoModel customerInfo = new CustomerInfoModel();
            var m = customerInfoBLL.GetModel(p => p.ID == id);
            customerInfo = BuildCustomerInfoModel(m);

            ViewBag.CustomerName = customerInfoBLL.GetModel(p => p.ID == id).客户姓名;
            var productList = productCodeBLL.GetModels(p => true).ToList();
            try
            {


                SelectList productSelectListItems = new SelectList(productList, "型号", "型号");
                ViewBag.ProductOptions = productSelectListItems;

                if (exceptedBuyModels == null) { return Content("<script>alert('查无数据！！');window.history.go(-1);</script>"); }



            }
            catch (Exception e)
            {

                return Content("<script>alert('" + e.Message + "');window.history.go(-1);</script>"); ;
            }
            if (customerInfo.接待人ID == em.ID || customerInfo.跟进人ID == em.ID || em.是否店长 == true)
            {
                var model = exceptedBuyModels.OrderByDescending(p => p.ID).ToList();

                return View(model);
            }
            return Content("<script>alert('无操作权限！！');window.history.go(-1);</script>");

        }
        /// <summary>
        /// 删除预购
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelExceptedBuyAction(int ExceptedBuyID, int receptionId)
        {
            if (exceptedBuyBLL.GetModel(p => p.ID == ExceptedBuyID) == null)
            {
                string str = string.Format("<script>alert('该数据已被更改，或不存在！');parent.location.href='CustomerIndex';</script>");
                return Content(str);
            }


            exceptedBuyBLL.DelBy(p => p.ID == ExceptedBuyID);
            Session["method"] = "Y";
            var exceptedBuyModels = BuildExceptedBuy(receptionId);

            ViewBag.receptionid = receptionId;
            var productList = productCodeBLL.GetModels(p => true).ToList();
            SelectList productSelectListItems = new SelectList(productList, "型号", "型号");
            ViewBag.ProductOptions = productSelectListItems;

            return Json(exceptedBuyModels);
        }
        /// <summary>
        /// 增加预购
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExceptedBuyAdd(string exceptModel)
        {
            if (Session["method"].ToString() == "Y")
            {
                string str = string.Format("<script>alert('重复操作！');parent.location.href='CustomerIndex';</script>");
                return Json(str);
            }
            List<CustomerExceptedBuyModel> list = JsonConvert.DeserializeObject<List<CustomerExceptedBuyModel>>(exceptModel);
            if (list.Count == 0)
            {
                return Json("<script>alert('不存在你跟进的客户，你不能执行预购操作！');window.history.go(-1);</script>");
            }
            foreach (var item in list)
            {
                销售_接待记录_意向明细 model = new 销售_接待记录_意向明细
                {
                    商品型号ID = productCodeBLL.GetModel(p => p.型号 == item.型号).ID,
                    备注 = item.备注,
                    接待ID = item.接待,
                    空间 = item.空间
                };
                if (ModelState.IsValid)
                {
                    exceptedBuyBLL.Add(model);
                    Session["method"] = "Y";

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
                    string s = null;
                    foreach (var it in sb)
                    {
                        s += it.ToString() + ";";
                    }
                    return Json("<script>alert('" + s + "');window.history.go(-1);</script>");
                }
            }


            //BuildExceptedBuy(model.接待ID);
            //ViewBag.receptionid = model.接待ID;
            //var productList = productCodeBLL.GetModels(p => true).ToList();
            //SelectList productSelectListItems = new SelectList(productList, "型号", "型号");
            //ViewBag.ProductOptions = productSelectListItems;
            //return RedirectToAction("ExceptedBuyIndex", new { id = list.FirstOrDefault().接待 });
            return Json("success");
        }
        /// <summary>
        /// 添加客户意向产品视图
        /// </summary>
        /// <param name="receptionid">接待ID</param>
        /// <returns></returns>
        public ViewResult AddExceptedBuyView(int receptionid)
        {
            Session["method"] = "N";
            ViewBag.AddReceptionid = receptionid;
            var em = SetEmployee();
            ViewBag.Store = em.店铺;
            ViewBag.Employee = em.姓名;
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            var productList = productCodeBLL.GetModels(p => true).ToList();
            SelectList productSelectListItems = new SelectList(productList, "型号", "型号");
            ViewBag.AddProductOptions = productSelectListItems;
            return View();
        }
        /// <summary>
        /// 获取型号
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSelect()
        {
            var productList = productCodeBLL.GetModels(p => true).ToList();
            SelectList productSelectListItems = new SelectList(productList, "型号", "型号");

            return Json(productSelectListItems);
        }
        /// <summary>
        /// 更新预购
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UpdateExceptedBuyAction(string 型号, string 备注, int 接待, int id, string 空间)
        {
            if (Session["method"].ToString() == "Y")
            {
                string str = string.Format("<script>alert('重复操作！');parent.location.href='CustomerIndex';</script>");
                return Content(str);
            }
            销售_接待记录_意向明细 model = new 销售_接待记录_意向明细
            {
                ID = id,
                接待ID = 接待,
                商品型号ID = productCodeBLL.GetModel(p => p.型号 == 型号).ID,
                备注 = 备注,
                空间 = 空间
            };

            if (ModelState.IsValid)
            {

                exceptedBuyBLL.Modify(model);
                Session["method"] = "Y";

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
                string s = null;
                foreach (var item in sb)
                {
                    s += item.ToString() + ";";
                }
                return Content("<script>alert('" + s + "');window.history.go(-1);</script>");
            }
            BuildExceptedBuy(接待);
            ViewBag.AddReceptionid = 接待;
            var productList = productCodeBLL.GetModels(p => true).ToList();
            SelectList productSelectListItems = new SelectList(productList, "型号", "型号");
            ViewBag.ProductOptions = productSelectListItems;
            return RedirectToAction("ExceptedBuyIndex", new { id = 接待 });


        }
        #endregion


        #region  销售成交数据

        /// <summary>
        /// 已成交客户信息列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchString"></param>
        /// <param name="startDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public ActionResult DealCustomerView(int? page, string searchString, string startDate, string EndDate) {

            Session["method"] = "N";
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            int pageSize = 25;
            int pageNumber = (page ?? 1);
            List<CustomerInfoModel> customerInfoModels = new List<CustomerInfoModel>();


            var m = BuildCustomerInfoModels();//将顾客接待信息数据优化
            if (m.Count() <= 0)
            {
                return View(customerInfoModels.ToPagedList(pageNumber, pageSize));
            }
            customerInfoModels = m.Where(p=>p.是否成交==true).ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Replace(" ", "");
                customerInfoModels = customerInfoModels.Where(w => w.客户电话 == searchString).ToList();//通过电话查找
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

            customerInfoModels = customerInfoModels.OrderByDescending(p => p.接待日期).ToList();//按日期排序

            return View(customerInfoModels.ToPagedList(pageNumber, pageSize));

        }
        /// <summary>
        /// 销售成交记录列表页
        /// </summary>
        /// <param name="recID">接待ID</param>
        /// <param name="page"></param>
        /// <param name="searchString">查询条件销售单号</param>
        /// <param name="employeesID"></param>
        /// <param name="IsDeal"></param>
        /// <param name="startDate">查询条件，开始时间</param>
        /// <param name="EndDate">查询条件，结束时间</param>
        /// <returns></returns>
        public ActionResult SalesRecordsIndex(int? page, string searchString, string startDate, string EndDate)
        {
            Session["method"] = "N";
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;
            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;


           
   
          
            List<SalesRecordsViewModel> salesRecordsModels = new List<SalesRecordsViewModel>();
            salesRecordsModels = BuildSalesRecordsModels();//将顾客成交信息数据优化
            int pageSize = 25;
            int pageNumber = (page ?? 1);
            if (salesRecordsModels.Count <= 0)
            {
                ViewBag.Search = searchString;
                return View(salesRecordsModels.ToPagedList(pageNumber, pageSize));
            }
            //salesRecordsModels = salesRecordsModels.Where(p => p.销售人员ID==em.ID).ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Replace(" ", "");
                salesRecordsModels = salesRecordsModels.Where(w => w.合同单号 == searchString).ToList();//通过销售单号查找
            }
         
            if (startDate != null && startDate != "")
            {
                DateTime startTime = Convert.ToDateTime(startDate);
                salesRecordsModels = salesRecordsModels.Where(w => w.销售日期 > startTime).ToList();
                ViewBag.StartDate = startDate;
            }
            if (EndDate != null && EndDate != "")
            {
                DateTime endTime = Convert.ToDateTime(EndDate);
                salesRecordsModels = salesRecordsModels.Where(w => w.销售日期 < endTime).ToList();
                ViewBag.EnDate = EndDate;
            }

            salesRecordsModels = salesRecordsModels.OrderByDescending(p => p.销售日期).ToList();
            
            return View(salesRecordsModels.ToPagedList(pageNumber, pageSize));

        }

        /// <summary>
        /// 销售成交数据添加视图
        /// </summary>
        /// <param name="recId">接待ID</param>
        /// <returns></returns>
        public ActionResult SalesRecordAddView(int recId)
        {
            Session["method"] = "N";
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            var m = customerInfoBLL.GetModel(p => p.ID == recId);
            SalesRecordsViewModel model = new SalesRecordsViewModel();
            model.接待记录ID = recId;
            model.客户姓名 = m.客户姓名;
            model.客户联系方式 = "[" + m.客户电话 + " ][" + m.社交软件 + "]";
            return View(model);
        }

        /// <summary>
        /// 检验合同单号是否存在
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckSalesOrder(string ContractNum) {
            销售_订单 model = new 销售_订单() { };  
            try
            {
                model=salesOrderBLL.GetModel(p => p.合同编号 == ContractNum);
            }
            catch (Exception e)
            {

                return Json(new { success = false, msg = "查询失败！"+e.Message });
            }
             
            if (model==null)
            {
                return Json(new { success = false, msg = "不存在合同编号，添加成交记录失败！" });
            }
            return Json(new { success = true, msg = "查询成功！" });

        }

        /// <summary>
        /// 销售成交数据添加操作
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesRecordAdd(SalesRecordsViewModel salesRecords)
        {
            销售_接待成交单 model = new 销售_接待成交单();
            var em = SetEmployee();
            model.制单人员ID = em.ID;
            model.制单日期 = DateTime.Now;
            model.合同单号 = salesRecords.合同单号;
            model.折扣 = salesRecords.折扣;
            model.接待记录ID = salesRecords.接待记录ID;
            model.更新人ID = em.ID;
            model.更新日期 = DateTime.Now;
            model.订库样 = salesRecords.订库样;
            model.销售人员ID = em.ID;
            model.销售单号 = salesRecords.销售单号;
            model.销售日期 = salesRecords.销售日期.Value;
            model.销售金额 = salesRecords.销售金额.Value;
            model.店铺ID = em.店铺ID;
            model.是否业务业绩 = salesRecords.是否业务业绩;
            model.是否自营业绩 = salesRecords.是否自营业绩;
            model.备注 = salesRecords.备注;
            try
            {
                model=salesRecordBLL.AddReturnModel(model);
                var m = customerInfoBLL.GetModel(p => p.ID == model.接待记录ID);
                string[] property = new string[3];
                property[0] = "是否成交";
                property[1] = "更新人";
                property[2] = "更新日期";
                m.是否成交 = true;
                m.更新人 = em.ID;
                m.更新日期 = DateTime.Now;
                customerInfoBLL.Modify(m, property);
            }
            catch (Exception e)
            {

                return Content("<script>alert('" + e.Message + "');window.history.go(-1);</script>");
            }
            return RedirectToAction("SalesRecordInfoView",new { id=model.ID});
        }

        /// <summary>
        /// 销售成交数据修改
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesRecordEdit(SalesRecordsViewModel salesRecords)
        {
            var model = salesRecordBLL.GetModel(p => p.ID == salesRecords.ID);
            var em = SetEmployee();
            model.合同单号 = salesRecords.合同单号;
            model.备注 = salesRecords.备注;
            model.折扣 = salesRecords.折扣;
            model.是否全款 = salesRecords.是否全款;
            model.更新人ID = em.ID;
            model.更新日期 = DateTime.Now;
            model.订库样 = salesRecords.订库样;
            model.销售单号 = salesRecords.销售单号;
            model.销售日期 = salesRecords.销售日期.Value;
            model.销售金额 = salesRecords.销售金额.Value;
            model.是否业务业绩 = salesRecords.是否业务业绩;
            model.是否自营业绩 = salesRecords.是否自营业绩;
            try
            {
                salesRecordBLL.Modify(model);
            }
            catch (Exception e)
            {

                return Content("<script>alert('数据修改失败!"+e.Message+"');window.history.go(-1);</script>");
            }
            return RedirectToAction("SalesRecordInfoView", new { id = model.ID });
        }

        /// <summary>
        /// 销售成交数据详情页面
        /// </summary>
        /// <param name="id">销售成交数据ID</param>
        /// <param name="isEdit">是否修改</param>
        /// <returns></returns>
        public ActionResult SalesRecordInfoView(int id, bool? isEdit)
        {
            Session["method"] = "N";
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            var model = salesRecordBLL.GetModel(p => p.ID == id);
            var m = BuildSalesRecordsModel(model);
            ViewBag.IsEdit = isEdit;
            return View(m);
        }





        #endregion


        #region 销售订单
        /// <summary>
        /// 销售订单列表视图
        /// </summary>
        /// <param name="emid">员工ID</param>
        /// <param name="str">查询条件（电话号码）</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="role">是否店长</param>
        /// <returns></returns>
        public ActionResult SalesOrdersView(int? emid,string str,DateTime? startDate,DateTime? endDate,bool role,int? recid) {
            var em = SetEmployee();
           
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;
            ViewBag.emID = em.ID;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            List<SalesOrderViewModel> lis = new List<SalesOrderViewModel>();
            if (recid != null)
            {
                lis = BuildSalesOrderModels(em.店铺ID, null, str, startDate, endDate, recid);
            }
            if (role==true)
            {
                if (startDate==null&&endDate==null)
                {
                    DateTime dt = DateTime.Now;
                    DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初
                    DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);//本月月末
                    startDate = startMonth;
                    endDate = endMonth;
                }
                lis = BuildSalesOrderModels(em.店铺ID, emid, str, startDate, endDate,recid);//默认显示当月数据

            }
            
            else
            {
                lis = BuildSalesOrderModels(em.店铺ID, em.ID, str, startDate, endDate,recid);
            }
            return View(lis);
        }

        /// <summary>
        /// 销售订单添加视图
        /// </summary>
        /// <param name="id">接待ID</param>
        /// <returns></returns>
        public ActionResult SalesOrdersAddView(int id) {
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            var customer = customerInfoBLL.GetModel(p=>p.ID==id);
            SalesOrderViewModel model = new SalesOrderViewModel();
            model.客户姓名 = customer.客户姓名;
            model.客户地址 = customer.安装地址;
            model.客户联系方式 = customer.客户电话;
            model.接待ID = id;
            //if (model.客户地址==null)
            //{
            //    return Content("<script>alert('客户信息中没有客户地址，请添加后下订单！');window.history.go(-1);</script>");
            //}
            //if (model.客户联系方式==null)
            //{
            //    return Content("<script>alert('客户信息中没有客户电话，请添加后下订单！');window.history.go(-1);</script>");
            //}
            //if (model.客户姓名==null)
            //{
            //    return Content("<script>alert('客户信息中没有客户姓名，请添加后下订单！');window.history.go(-1);</script>");
            //}
            return View(model);
        }

        /// <summary>
        /// 销售订单添加方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalesOrderAdd(销售_订单 model) {
            var em = SetEmployee();
            model.员工ID = em.ID;
            model.店铺ID = em.店铺ID;
            model.制单销售人ID = em.ID;
            model.制单日期 = DateTime.Now;
            try
            {
                model = salesOrderBLL.AddReturnModel(model);
            }
            catch (Exception e)
            {

                return Json(new { success=false,msg="添加失败！"+e.Message});
            }
            return Json(new { success = true, msg = "添加成功!", data = model });
        }

        /// <summary>
        /// 销售订单修改视图
        /// </summary>
        /// <param name="id">销售订单ID</param>
        /// <returns></returns>
        public ActionResult SalesOrdersEditView(int id) {
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            var model = salesOrderBLL.GetModel(p => p.ID == id);
            SalesOrderViewModel salesOrderViewModel = new SalesOrderViewModel();
            salesOrderViewModel = BuildSalesOrderModel(model);
            return View(salesOrderViewModel);
        }

        /// <summary>
        /// 销售订单修改操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalesOrdersEdit(销售_订单 model) {
            var em = SetEmployee();
            
            model.员工ID = em.ID;
            model.店铺ID = em.店铺ID;
            model.制单销售人ID = em.ID;
            model.制单日期 = DateTime.Now;
            var m = salesOrderBLL.GetModel(p => p.ID == model.ID);
            m = model;
            m.员工ID = em.ID;
           
            try
            {
                salesOrderBLL.Modify(m);
            }
            catch (Exception e)
            {

                return Json(new { success = false, msg = "修改失败！" + e.Message });
            }
            return Json(new { success = true, msg = "修改成功!" });
        }

        /// <summary>
        /// 销售订单删除操作
        /// </summary>
        /// <param name="id">销售订单ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalesOrderDel(int id) {
            try
            {
                salesOrderBLL.DelBy(p => p.ID == id);
            }
            catch (Exception e)
            {

                return Json(new { success=false,msg="删除失败！"+e.Message});
            }
            return Json(new { success = true, msg = "删除成功！"});
        }

        /// <summary>
        /// 销售订单详情视图
        /// </summary>
        /// <param name="id">销售订单ID</param>
        /// <returns></returns>
        public ActionResult SalesOrderInfoView(int id) {
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            var model = salesOrderBLL.GetModel(p => p.ID == id);
            var m =BuildSalesOrderModel(model);
            return View(m);
        }

    

        /// <summary>
        /// 销售订单明细列表视图
        /// </summary>
        /// <param name="id">销售订单ID</param>
        /// <returns></returns>
        public ActionResult SalesOrderDetailsView(int id) {
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            var models = BuildSalesOrdersDetailsModels(id);
            return View(models);

        }

        /// <summary>
        /// 销售明细操作页
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <returns></returns>
        public ActionResult SalesOrderDetailsEditView(int id)
        {
            var em = SetEmployee();
            ViewBag.Store = em.店铺ID;
            ViewBag.Employee = em.姓名;

            ViewBag.IsManager = em.是否店长;
            ViewBag.IsDesigner = em.是否设计师;
            ViewBag.IsEmployee = em.是否销售;
            var models = BuildSalesOrdersDetailsModels(id);
            return View(models);

        }
        /// <summary>
        /// 销售订单明细提交操作
        /// </summary>
        /// <param name="lis"></param>
        /// <returns></returns>
        public ActionResult SalesOrderDetailsAdd(string lis) {
            var list = JsonConvert.DeserializeObject<List<销售_订单明细>>(lis);
            if (list.Count == 0)
            {
                return Json(new { success = false, msg = "订单明细添加失败！" });
            }

            try
            {
                foreach (var item in list)
                {
                    salesOrder_DetailsBLL.Add(item);
                }
            }
            catch (Exception e)
            { 

                return Json(new { success=false ,msg="订单明细添加出错！"+e.Message});
            }
            return Json(new { success = true, msg = "订单明细添加成功！" });
        }

        /// <summary>
        /// 订单明细修改操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalesOrderDetailsEdit(销售_订单明细 model) {
            var em = SetEmployee();
            var m = salesOrder_DetailsBLL.GetModel(p => p.ID == model.ID);
            m = model;
            try
            {
                salesOrder_DetailsBLL.Modify(m);
            }
            catch (Exception e)
            {

                return Json(new { success=false,msg="订单明细修改失败！"+e.Message});
            }
            return Json(new { success = true, msg = "订单明细修改成功！" });

        }

        /// <summary>
        /// 销售订单明细删除操作
        /// </summary>
        /// <param name="id">销售订单明细ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalesOrderDetailsDel(int id) {
            try
            {
                salesOrder_DetailsBLL.DelBy(p => p.ID == id);
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = "订单明细删除失败！" + e.Message });
            }
            return Json(new { success = true, msg = "订单明细删除成功！" });

        }
        #endregion


        /// <summary>
        /// 构建订单实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private SalesOrderViewModel BuildSalesOrderModel(销售_订单 model) {
            SalesOrderViewModel salesOrder = new SalesOrderViewModel();
            salesOrder.ID = model.ID;
            salesOrder.IP = model.IP;
            salesOrder.rid = model.rid;
            salesOrder.制单销售人ID = model.制单销售人ID;
            //salesOrder.制单人姓名 = storeEmployeesBLL.GetModel(p => p.ID == model.制单销售人ID).姓名;
            salesOrder.制单日期 = model.制单日期;
            salesOrder.单据备注 = model.单据备注;
            salesOrder.单据日期 = model.单据日期;
            salesOrder.单据类别ID = model.单据类别ID;
            salesOrder.单据编号 = model.单据编号;
            salesOrder.合同编号 = model.合同编号;
            salesOrder.员工ID = model.员工ID;
            if (model.员工ID!=null)
            {
                salesOrder.员工 = storeEmployeesBLL.GetModel(p => p.ID == model.员工ID).姓名;
            }
            salesOrder.复核人 = model.复核人;
            if (salesOrder.复核人!=null)
            {
                salesOrder.复核人姓名 = systemAccountBLL.GetModel(p => p.ID == salesOrder.复核人).姓名;
            }
            salesOrder.复核日期 = model.复核日期;
            salesOrder.复核标志 = model.复核标志;
            salesOrder.审核人 = model.审核人;
            if (salesOrder.审核人!=null)
            {
                salesOrder.审核人姓名 = systemAccountBLL.GetModel(p => p.ID == salesOrder.审核人).姓名;
            }
            salesOrder.审核日期 = model.审核日期;
            salesOrder.审核标志 = model.审核标志;
            salesOrder.客户地址 = model.客户地址;
            salesOrder.客户姓名 = model.客户姓名;
            salesOrder.客户联系方式 = model.客户联系方式;
            salesOrder.店铺ID = model.店铺ID;
            if (salesOrder!=null)
            {
                salesOrder.店铺 = storeBLL.GetModel(p => p.ID == salesOrder.店铺ID).名称;
            }
            salesOrder.批准人1 = model.批准人1;
            if (salesOrder.批准人1!=null)
            {
                salesOrder.批准人1姓名 = systemAccountBLL.GetModel(p => p.ID == salesOrder.批准人1).姓名;

            }
            salesOrder.批准日期1 = model.批准日期1;
            salesOrder.批准标志1 = model.批准标志1;
            salesOrder.收货人 = model.收货人;
            salesOrder.收货人联系方式 = model.收货人联系方式;
            salesOrder.收货地址 = model.收货地址;
            salesOrder.确认人 = model.确认人;
            if (salesOrder.确认人!=null)
            {
                salesOrder.确认人姓名 = systemAccountBLL.GetModel(p => p.ID == salesOrder.确认人).姓名;

            }
            salesOrder.确认日期 = model.确认日期;
            salesOrder.确认标志 = model.确认标志;
            salesOrder.订单编号 = model.订单编号;
            salesOrder.零售合计 = model.零售合计;
            salesOrder.预付金额_bak = model.预付金额_bak;

            salesOrder.制单销售人ID = model.制单销售人ID;
            if (salesOrder.制单销售人ID != null) {
                salesOrder.制单销售人 = storeEmployeesBLL.GetModel(p => p.ID == model.制单销售人ID).姓名;
            }
            salesOrder.接待ID = model.接待ID;
          
            return salesOrder;
        }

        /// <summary>
        /// 构建订单实体集
        /// </summary>
        /// <param name="id">门店ID</param>
        /// <param name="emid">员工ID</param>
        /// <param name="str">搜索条件</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        private List<SalesOrderViewModel> BuildSalesOrderModels(int id,int? emid,string str,DateTime? startDate,DateTime? endDate,int?recid) {
            var models = salesOrderBLL.GetModels(p => p.店铺ID == id);
            if (models == null)
            {
                return null;
            }
            if (recid!=null)
            {
                models = models.Where(p => p.接待ID == recid);
            }
            
            if (emid!=null)
            {
                if (models == null)
                {
                    return null;
                }
                models = models.Where(p => p.员工ID == emid);
            }
            if (!string.IsNullOrEmpty(str))
            {
                if (models == null)
                {
                    return null;
                }
                models = models.Where(p => p.客户联系方式 == str);
            }
            if (startDate!=null)
            {
                if (models == null)
                {
                    return null;
                }
                models = models.Where(p => p.单据日期 >= startDate);
            }
            if (endDate!=null)
            {
                if (models == null)
                {
                    return null;
                }
                models = models.Where(p => p.单据日期 <= endDate);
            }
            if (models ==null)
            {
                return null;
            }
            List<SalesOrderViewModel> lis = new List<SalesOrderViewModel>();
            foreach (var item in models)
            {
                var model = BuildSalesOrderModel(item);
                lis.Add(model);
            }
            return lis;
        }

        /// <summary>
        /// 构建订单明细实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private SalesOrdersDetailsViewModel BuildSalesOrdersDetailsModel(销售_订单明细 model) {
            SalesOrdersDetailsViewModel salesOrdersDetails = new SalesOrdersDetailsViewModel();
            salesOrdersDetails.ID = model.ID;
            salesOrdersDetails.SKU_ID = model.SKU_ID;
            salesOrdersDetails.交货周期 = model.交货周期;
            salesOrdersDetails.单价 = model.单价;
            salesOrdersDetails.单据ID = model.单据ID;
            salesOrdersDetails.小计 = model.小计;
            salesOrdersDetails.序号 = model.序号;
            salesOrdersDetails.数量 = model.数量;
            salesOrdersDetails.明细备注 = model.明细备注;
            salesOrdersDetails.标准零售价 = model.标准零售价;
            salesOrdersDetails.零售单价 = model.零售单价;
            salesOrdersDetails.零售小计 = model.零售小计;
            salesOrdersDetails.需求日期 = model.需求日期;
            salesOrdersDetails.预计交期 = model.预计交期;
            salesOrdersDetails.默认交期 = model.默认交期;
            var sku = product_SKUBLL.GetModel(p => p.ID == model.SKU_ID);
            var spu = product_SPUBLL.GetModel(p => p.SPUID == sku.SPU_ID);
            salesOrdersDetails.名称 = spu.名称;
            salesOrdersDetails.描述 = sku.描述;
            salesOrdersDetails.系列 = spu.系列;
            salesOrdersDetails.是否进口 = spu.是否进口;
            salesOrdersDetails.规格 = spu.规格;
            salesOrdersDetails.计量单位 = spu.计量单位;
            salesOrdersDetails.商品型号 = spu.型号;
            salesOrdersDetails.商品编号 = spu.编号;
            return salesOrdersDetails;
        }

        /// <summary>
        /// 构建销售订单明细实体集
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<SalesOrdersDetailsViewModel> BuildSalesOrdersDetailsModels(int id) {
            var models = salesOrder_DetailsBLL.GetModels(p => p.单据ID == id);
            if (models==null)
            {
                return null;
            }
            List<SalesOrdersDetailsViewModel> lis = new List<SalesOrdersDetailsViewModel>();
            foreach (var item in models)
            {
                var model = BuildSalesOrdersDetailsModel(item);
                lis.Add(model);
            }
            return lis;
        }
        /// <summary>
        /// 设置当前操作人员及店铺信息
        /// </summary>
        private Employees SetEmployee()
        {
            string userName = HttpContext.User.Identity.Name;
            Employees employees = new Employees();
            if (userName != null)
            {
                employees = HttpContext.Session["Employee"] as Employees;


                return employees;
            }
            else
            {

                return null;

            }
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
            salesRecords.是否业务业绩 = model.是否业务业绩;
            salesRecords.是否自营业绩 = model.是否自营业绩;
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
            List<SalesRecordsViewModel> lis = new List<SalesRecordsViewModel>();
            var models = salesRecordBLL.GetModels(p => p.销售人员ID == em.ID);
            if (models==null)
            {
                return null;
            }
             foreach (var item in models)
            {
                var m = BuildSalesRecordsModel(item);
                lis.Add(m);
            }
          
           
            return lis;
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
                customerInfo.是否确认 = model.是否确认;
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
            customer = customerInfoBLL.GetModels(p => p.接待人ID == em.ID || p.跟进人ID == em.ID).ToList();
            if (customer==null)
            {
                return null;
            }
            foreach (var item in customer)
            {
                var model = BuildCustomerInfoModel(item);

                customerInfoModelsList.Add(model);
            }
            return customerInfoModelsList.AsEnumerable().AsQueryable();
        }

        /// <summary>
        /// 根据接待id查询产品信息
        /// </summary>
        private IQueryable<CustomerExceptedBuyModel> BuildExceptedBuy(int id)
        {
            if (id == 0)
            {
                return null;
            }
            List<CustomerExceptedBuyModel> models = new List<CustomerExceptedBuyModel>();
            var exceptedBuy = exceptedBuyBLL.GetListBy(p => p.接待ID == id);
            if (exceptedBuy != null)
            {
                foreach (var item in exceptedBuy)
                {
                    CustomerExceptedBuyModel exceptedBuyModel = new CustomerExceptedBuyModel
                    {
                        ID = item.ID,
                        型号 = productCodeBLL.GetModel(p => p.ID == item.商品型号ID).型号,
                        备注 = item.备注,
                        空间 = item.空间,
                        接待 = id
                    };
                    models.Add(exceptedBuyModel);
                }

            }
            return models.AsEnumerable().AsQueryable();

        }
    }
}