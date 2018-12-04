﻿using ChicStoreManagement.IBLL;
using ChicStoreManagement.WEB.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Newtonsoft.Json;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.Controllers
{
    public class DesignSchemeController : Controller
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


        private int employeeID;
        private string employeeName;
        private string store;
        private int storeID;

        public DesignSchemeController(ICustomerInfoBLL customerInfoIBLL, IDesign_CustomerExceptedBuyBLL design_CustomerExceptedBuyBLL, IDesignSubmitBLL designSubmitBLL, IStoreBLL storeBLL, IPositionBLL positionBLL, IStoreEmployeesBLL storeEmployeesBLL, IExceptedBuyBLL exceptedBuyBLL, IProductCodeBLL productCodeBLL, ICustomerTrackingBLL customerTrackingBLL)
        {
            this.customerInfoBLL = customerInfoIBLL;
            this.design_CustomerExceptedBuyBLL = design_CustomerExceptedBuyBLL;
            this.designSubmitBLL = designSubmitBLL;
            this.storeBLL = storeBLL;
            this.positionBLL = positionBLL;
            this.storeEmployeesBLL = storeEmployeesBLL;
            this.exceptedBuyBLL = exceptedBuyBLL;
            this.productCodeBLL = productCodeBLL;
            this.customerTrackingBLL = customerTrackingBLL;
        }

        /// <summary>
        /// 设计方案
        /// </summary>
        /// <param name="id">接待记录id</param>
        /// <returns></returns>
        // GET: DesignScheme
        public ActionResult DesignSchemeIndex(int? id, string sortOrder, string searchString, string currentFilter, int? page)
        {

            Session["method"] = "N";
            SetEmployee();//获取当前人员信息
            var costomerModels = BuildCustomerInfo();
            ViewBag.EmployeesID = employeeID;
            ViewBag.DesignSubCurrentSort = sortOrder;
            ViewBag.DesignSubDate = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.DesignSubResult = sortOrder == "last" ? "last_desc" : "last";
            List<DesignSubmitModel> designSubmitModels = new List<DesignSubmitModel>();
            //构建设计表信息  
            designSubmitModels = BuildDesignSubInfo(id).ToList();
            if (id != 0 && id != null)
            {
                ViewBag.DreceptionID = id;
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
        ///当前跟进人的名录
        /// </summary>
        /// <returns></returns>
        public ActionResult DoingApplyDesignView(string sortOrder, string searchString, string currentFilter, int? page)
        {
            //Session["method"] = "N";
            //SetEmployee();
            //ViewBag.CustomerDesignCurrentSort = sortOrder;
            //ViewBag.CustomerDesignDate = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            //ViewBag.CustomerDesignName = sortOrder == "last" ? "last_desc" : "last";
            //var customerInfoModels = BuildCustomerInfo();//将顾客接待信息数据优化

            //List<BaseCustomerInfo> model = new List<BaseCustomerInfo>();
            //foreach (var item in customerInfoModels)
            //{
            //    BaseCustomerInfo baseCustomerInfo = new BaseCustomerInfo();
            //    baseCustomerInfo.客户姓名 = item.客户姓名;
            //    baseCustomerInfo.接待序号 = item.接待序号;
            //    baseCustomerInfo.接待记录ID = item.ID;
            //    baseCustomerInfo.联系方式 = item.客户电话;
            //    baseCustomerInfo.接待日期 = item.接待日期;
            //    model.Add(baseCustomerInfo);
            //}
            //if (searchString != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}
            //ViewBag.CustomerDesignCurrentFilter = searchString;//获得前端传回来的搜索关键词

            //var models = model.AsQueryable();
            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    models = models.Where(w => w.联系方式 == searchString);//通过姓名查找
            //}
            ////Session["Name"] = customerInfoModels.FirstOrDefault();
            //#region 排序，默认按ID升序
            //switch (sortOrder)
            //{
            //    case "first_desc":
            //        models = models.OrderByDescending(w => w.接待日期);
            //        break;
            //    case "last_desc":
            //        models = models.OrderByDescending(w => w.客户姓名);
            //        break;
            //    case "last":
            //        models = models.OrderBy(w => w.客户姓名);
            //        break;
            //    default:
            //        models = models.OrderBy(w => w.接待日期);
            //        break;
            //}

            //#endregion
            //int pageSize = 10;
            //int pageNumber = (page ?? 1);
            //ViewBag.CustomerDesignName = employeeName;//给前端传入当前操作人
            //ViewBag.CustomerDesignPosition = storeEmployeesBLL.GetModel(p => p.姓名 == employeeName).职务ID;//给前端传入当前操作人职位


            //return View(models.ToPagedList(pageNumber, pageSize));
            return View();
        }

        #region 设计案申请模块

        /// <summary>
        /// 申请设计
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplyDesignView(int? recepitonID)
        {
            Session["method"] = "N";
            SetEmployee();
            List<销售_接待记录> customerInfos = new List<销售_接待记录>();
            DesignSubmitModel designSubmitModel = new DesignSubmitModel();
            if (recepitonID != null && recepitonID != 0)
            {
                var model = customerInfoBLL.GetModel(p => p.ID == recepitonID);
                designSubmitModel.客户姓名 = model.客户姓名;
                designSubmitModel.家庭成员 = model.家庭成员;
                designSubmitModel.职业 = model.家庭成员;
                designSubmitModel.楼盘具体位置 = model.安装地址;
                designSubmitModel.职业 = model.客户职业;
                designSubmitModel.联系方式 = model.客户电话;
                designSubmitModel.客户喜好 = model.主导者喜好风格;
                designSubmitModel.客户在意品牌或已购买品牌 = model.比较品牌;
                designSubmitModel.装修风格 = model.装修风格;
                designSubmitModel.装修进度 = model.装修进度;
                designSubmitModel.预算 = model.预算金额;
                designSubmitModel.接待记录ID = model.ID;
                designSubmitModel.销售人员 = employeeName;
            }
            else if (employeeID != 0)
            {
                customerInfos = customerInfoBLL.GetModels(p => p.接待人ID == employeeID || p.跟进人ID == employeeID).ToList();
                SelectList customerInfosSelectListItem = new SelectList(customerInfos, "客户电话", "客户电话");
                ViewBag.CustomerPhoneNumber = customerInfosSelectListItem;

            }
            ViewBag.EmployeeName = employeeName;
            return View(designSubmitModel);

        }

        /// <summary>
        /// 提交设计案申请
        /// </summary>
        /// <param name="designSubmitModel"></param>
        /// <param name="id">接待记录id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DesignApply(DesignSubmitModel designSubmitModel)
        {
            if (Session["method"].ToString() == "Y")
            {
                string str = string.Format("<script>alert('重复操作！');parent.location.href='TrackLogIndex';</script>");
                return Content(str);
            }
            SetEmployee();

            designSubmitModel.接待记录ID = customerInfoBLL.GetModel(p => p.客户电话 == designSubmitModel.联系方式).ID;


            designSubmitModel.更新人 = employeeName;
            designSubmitModel.更新日期 = DateTime.Now;
            try
            {
                销售_设计案提交表 model = new 销售_设计案提交表
                {
                    接待记录ID = designSubmitModel.接待记录ID,
                    客户姓名 = designSubmitModel.客户姓名,
                    联系方式 = designSubmitModel.联系方式,
                    职业 = designSubmitModel.职业,
                    家庭成员 = designSubmitModel.家庭成员,
                    楼盘具体位置 = designSubmitModel.楼盘具体位置,
                    面积大小 = designSubmitModel.面积大小,
                    装修风格 = designSubmitModel.装修风格,
                    装修进度 = designSubmitModel.装修进度,
                    预算 = designSubmitModel.预算,
                    客户喜好或忌讳 = designSubmitModel.客户喜好,
                    客户在意品牌或已购买品牌 = designSubmitModel.客户在意品牌或已购买品牌,
                    客户提问与要求 = designSubmitModel.客户提问与要求,
                    整体软装配饰 = designSubmitModel.整体软装配饰,
                    家具空间 = designSubmitModel.家具空间,
                    项目提交时间 = designSubmitModel.项目提交时间,
                    项目量房时间 = designSubmitModel.项目量房时间,
                    项目预计完成时间 = designSubmitModel.项目预计完成时间,
                    销售人员 = designSubmitModel.销售人员,
                    备注 = designSubmitModel.备注,
                    更新人 = designSubmitModel.更新人,
                    更新日期 = designSubmitModel.更新日期
                };
                if (ModelState.IsValid)
                {
                    designSubmitBLL.Add(model);

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
                    string s = "添加设计案申请信息出错：";
                    foreach (var item in sb)
                    {
                        s += item.ToString() + "<br/>";
                    }
                    return Content("<script>alert('" + s + "');window.history.go(-1);</script>");
                }

            }
            catch (Exception e)
            {
                return Content("<script>alert('信息异常：" + e + "。');window.history.go(-1);</script>");
            }
            return RedirectToAction("DesignSchemeIndex", new { id = designSubmitModel.接待记录ID });
        }
        /// <summary>
        /// 设计案申请修改(店长审核之后不可修改！)
        /// </summary>
        /// <param name="id">设计案申请id</param>
        /// <returns></returns>
        public ActionResult EditDesignApply(int id)
        {
            Session["method"] = "N";
            if (designSubmitBLL.GetModel(p => p.id == id).店长审核 != null && designSubmitBLL.GetModel(p => p.id == id).店长审核 == true)
            {
                return Content("该记录已被审核通过，不可修改！");
            }
            销售_设计案提交表 model = new 销售_设计案提交表();
            model = designSubmitBLL.GetModel(p => p.id == id);
            DesignSubmitModel designSubmitModel = new DesignSubmitModel
            {
                Id = model.id,
                客户喜好 = model.客户喜好或忌讳,
                客户在意品牌或已购买品牌 = model.客户在意品牌或已购买品牌,
                客户姓名 = model.客户姓名,
                客户提问与要求 = model.客户提问与要求,
                家具空间 = model.家具空间,
                家庭成员 = model.家庭成员,
                店长 = model.店长,
                店长审核 = model.店长审核,
                接待记录ID = model.接待记录ID,

                楼盘具体位置 = model.楼盘具体位置,
                职业 = model.职业,
                联系方式 = model.联系方式,
                装修进度 = model.装修进度,
                装修风格 = model.装修风格,
                设计人员 = model.设计人员,
                设计人员审核 = model.设计人员审核,
                销售人员 = model.销售人员,
                面积大小 = model.面积大小,
                预算 = model.预算,
                项目提交时间 = model.项目提交时间,
                项目量房时间 = model.项目量房时间,
                项目预计完成时间 = model.项目预计完成时间,
                备注 = model.备注
            };
            if (model.整体软装配饰 == null)
            {
                designSubmitModel.整体软装配饰 = false;
            }
            else
            {
                designSubmitModel.整体软装配饰 = model.整体软装配饰;
            }
            if (model.店长 != null && model.设计人员审核 == true && model.设计人员 != null && model.设计人员审核 == true && model.销售人员 != null)
            {
                designSubmitModel.审批状态 = true;
            }
            return View(designSubmitModel);
        }

        /// <summary>
        /// 设计案申请修改(店长审核之后不可修改！)
        /// </summary>
        /// <param name="designSubmitModel">设计案申请实体</param>
        /// <returns></returns>
        public ActionResult DesignApplyEdit(DesignSubmitModel designSubmitModel)
        {

            if (Session["method"].ToString() == "Y")
            {
                string str = string.Format("<script>alert('重复操作！');parent.location.href='TrackLogIndex';</script>");
                return Content(str);
            }
            SetEmployee();
            try
            {
                销售_设计案提交表 model = new 销售_设计案提交表
                {
                    id = designSubmitModel.Id,
                    接待记录ID = designSubmitModel.接待记录ID,
                    客户姓名 = designSubmitModel.客户姓名,
                    联系方式 = designSubmitModel.联系方式,
                    职业 = designSubmitModel.职业,
                    家庭成员 = designSubmitModel.家庭成员,
                    楼盘具体位置 = designSubmitModel.楼盘具体位置,
                    面积大小 = designSubmitModel.面积大小,
                    装修风格 = designSubmitModel.装修风格,
                    装修进度 = designSubmitModel.装修进度,
                    预算 = designSubmitModel.预算,
                    客户喜好或忌讳 = designSubmitModel.客户喜好,
                    客户在意品牌或已购买品牌 = designSubmitModel.客户在意品牌或已购买品牌,
                    客户提问与要求 = designSubmitModel.客户提问与要求,
                    整体软装配饰 = designSubmitModel.整体软装配饰,
                    家具空间 = designSubmitModel.家具空间,
                    项目提交时间 = designSubmitModel.项目提交时间,
                    项目量房时间 = designSubmitModel.项目量房时间,
                    项目预计完成时间 = designSubmitModel.项目预计完成时间,
                    销售人员 = employeeName,
                    备注 = designSubmitModel.备注,
                    更新人 = employeeName,
                    更新日期 = DateTime.Now,
                    店长 = designSubmitBLL.GetModel(p => p.id == designSubmitModel.Id).店长,
                    店长审核 = designSubmitBLL.GetModel(p => p.id == designSubmitModel.Id).店长审核,
                    设计人员 = designSubmitBLL.GetModel(p => p.id == designSubmitModel.Id).设计人员,
                    设计人员审核 = designSubmitModel.设计人员审核
                };
                if (ModelState.IsValid)
                {
                    designSubmitBLL.Modify(model);
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
                    string s = "添加设计案申请信息出错：";
                    foreach (var item in sb)
                    {
                        s += item.ToString() + "<br/>";
                    }
                    return Content("<script>alert('" + s + "');window.history.go(-1);</script>");
                }

            }
            catch (Exception e)
            {
                return Content("<script>alert('信息异常：" + e + "。');window.history.go(-1);</script>");
            }
            return RedirectToAction("DesignSchemeIndex", new { id = designSubmitModel.接待记录ID });
        }

        /// <summary>
        /// 设计申请提交表详细信息
        /// </summary>
        /// <param name="id">设计提交案id</param>
        /// <returns></returns>
        public ActionResult DesignApplyInfoView(int id)
        {
            Session["method"] = "N";
            SetEmployee();//获取当前人员信息\
            销售_设计案提交表 model = new 销售_设计案提交表();
            model = designSubmitBLL.GetModel(p => p.id == id);
            DesignSubmitModel submitModel = new DesignSubmitModel
            {
                Id = model.id,
                备注 = model.备注,
                客户喜好 = model.客户喜好或忌讳,
                客户在意品牌或已购买品牌 = model.客户在意品牌或已购买品牌,
                客户姓名 = model.客户姓名,
                客户提问与要求 = model.客户提问与要求,
                家具空间 = model.家具空间,
                家庭成员 = model.家庭成员,
                店长 = model.店长,
                店长审核 = model.店长审核,
                接待记录ID = model.接待记录ID,

                更新人 = model.更新人,
                更新日期 = model.更新日期,
                楼盘具体位置 = model.楼盘具体位置,
                职业 = model.职业,
                联系方式 = model.联系方式,
                装修进度 = model.装修进度,
                装修风格 = model.装修风格,
                设计人员 = model.设计人员,
                设计人员审核 = model.设计人员审核,
                销售人员 = model.销售人员,
                面积大小 = model.面积大小,
                项目提交时间 = model.项目提交时间,
                项目量房时间 = model.项目量房时间,
                项目预计完成时间 = model.项目预计完成时间,
                预算 = model.预算,

            };
            if (model.整体软装配饰 == null)
            {
                submitModel.整体软装配饰 = false;
            }
            else
            {
                submitModel.整体软装配饰 = model.整体软装配饰;
            }
            if (model.设计人员审核 != false && model.设计人员审核 != null && model.设计人员 != null && model.店长 != null && model.店长审核 != false && model.店长审核 != null)
            {
                submitModel.审批状态 = true;
            }



            // submitModel.销售_设计案提交表_客户意向产品明细 = BuildExceptedBuy(submitModel.接待记录ID);
            return View(submitModel);
        }
        /// <summary>
        /// 添加设计进度日志
        /// </summary>
        /// <returns></returns>
        public ActionResult AddDesignTrackLogView()
        {
            return View();
        }
        #endregion




        #region 设计案申请_意向产品明细
        /// <summary>
        /// 设计案—客户意向产品明细
        /// </summary>
        /// <param name="id">设计案提交表ID</param>
        /// <returns></returns>
        public ActionResult Design_ExceptedBuyIndex(int id)
        {
            Session["method"] = "N";
            var lis = BuildDesign_ExceptedBuy(id);
            return View(lis);
        }
        /// <summary>
        /// 添加客户意向明细
        /// </summary>
        /// <param name="id">设计案提交表ID</param>
        /// <returns></returns>
        public ActionResult AddDesign_ExceptedBuyView(int id) {
            Session["method"] = "N";
            Design_CustomerExceptedBuy model = new Design_CustomerExceptedBuy
            {
                设计提交案 = id
            };
            return View();
        }

        public ActionResult AddDesign_ExceptedBuy(string design_exceptedBuy) {
            Session["method"] = "Y";
            List<Design_CustomerExceptedBuy> list =JsonConvert.DeserializeObject<List<Design_CustomerExceptedBuy>>(design_exceptedBuy);
            if (list.Count==0)
            {
                return Json("<script>alert('不存在你跟进的客户，你不能执行预购操作！');window.history.go(-1);</script>");
            }


            foreach (var item in list)
            {
                销售_设计案提交表_客户意向产品明细 model = new 销售_设计案提交表_客户意向产品明细();
                model.产品名称 = item.产品;
                model.尺寸 = item.尺寸;
                model.接待记录ID = designSubmitBLL.GetModel(p => p.id == item.设计提交案).接待记录ID;
                model.数量 = item.数量;
                model.更新人 = employeeName;
                model.更新日期 = DateTime.Now;
                model.空间 = item.空间;
                model.系列 = item.系列;
                model.设计提交案 = item.设计提交案;
                model.配置 = item.配置;
                model.单位 = item.单位;
                if (ModelState.IsValid)
                {
                    design_CustomerExceptedBuyBLL.Add(model);
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
                    return Json(s);
                }
            }
            return Json("操作成功！");
        }
        public ActionResult DelDesign_ExceptedBuy(int id) {
            try
            {
                design_CustomerExceptedBuyBLL.DelBy(p => p.id == id);
            }
            catch (Exception)
            {

                return Json("操作异常！");
            }
            

            return Json("删除成功！");
        }
        #endregion
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
        /// 构建客户信息
        /// </summary>
        /// <returns>客户信息</returns>
        private IQueryable<CustomerInfoModel> BuildCustomerInfo()
        {

            List<CustomerInfoModel> customerInfoModelsList = new List<CustomerInfoModel>();
            List<销售_接待记录> customer = new List<销售_接待记录>();
            if (storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID == 3)
            {
                customer = customerInfoBLL.GetModels(p => p.店铺ID == storeID).ToList();//查询当前店铺所有顾客接待信息
            }
            else
            {
                customer = customerInfoBLL.GetModels(p => p.店铺ID == storeID && p.跟进人ID == employeeID).ToList();//查询当前跟进顾客接待信息
            }

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
        private IQueryable<DesignSubmitModel> BuildDesignSubInfo(int? id)
        {
            List<销售_设计案提交表> designSubModelList = new List<销售_设计案提交表>();
            if (storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID == 3)
            {   //店长可以查看所有信息
                designSubModelList = designSubmitBLL.GetModels(p => true).ToList();
            }

            else if (id != 0 && id != null)
            {   //根据接待ID查询当前客户的设计提交表
                designSubModelList = designSubmitBLL.GetModels(p => p.接待记录ID == id).ToList();
            }
            else
            {
                //查询当前销售人员的设计提交表
                designSubModelList = designSubmitBLL.GetModels(p => p.销售人员 == employeeName).ToList();
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
                designSubmitModel.店长 = item.店长;
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
                designSubmitModel.设计人员 = item.设计人员;
                designSubmitModel.设计人员审核 = item.设计人员审核;
                designSubmitModel.销售人员 = item.销售人员;
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
        /// 根据接待id查询产品信息
        /// </summary>
        private List<Design_CustomerExceptedBuy> BuildDesign_ExceptedBuy(int id)
        {
            if (id == 0)
            {
                return null;
            }
            List<Design_CustomerExceptedBuy> models = new List<Design_CustomerExceptedBuy>();
            List<销售_设计案提交表_客户意向产品明细> exceptedBuy = new List<销售_设计案提交表_客户意向产品明细>();
            try
            {
                exceptedBuy = design_CustomerExceptedBuyBLL.GetModels(p => p.设计提交案 == id).ToList();
            }
            catch (Exception e)
            {

                exceptedBuy = null;
            }

            if (exceptedBuy != null)
            {
                foreach (var item in exceptedBuy)
                {
                    Design_CustomerExceptedBuy exceptedBuyModel = new Design_CustomerExceptedBuy
                    {
                        Id = item.id,
                        产品 = item.产品名称,
                        尺寸 = item.尺寸,
                        单位 = item.单位,
                        数量 = item.数量,
                        配置 = item.配置,
                        设计提交案 = item.设计提交案,
                        接待记录ID = item.接待记录ID,
                        更新人 = item.更新人,
                        更新日期 = item.更新日期,
                        空间 = item.空间,
                        系列 = item.系列
                    };
                    //exceptedBuyModel.编号 = productCodeBLL
                    models.Add(exceptedBuyModel);
                }
                return models;
            }
            return null;

        }
    }
}