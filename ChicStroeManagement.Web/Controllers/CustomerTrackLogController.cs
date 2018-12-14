using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;
using ChicStoreManagement.WEB.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicStoreManagement.Controllers
{
    /// <summary>
    /// 客户跟进日志 
    /// </summary>
    public class CustomerTrackLogController : Controller
    {

        private readonly ICustomerInfoBLL customerInfoBLL;
        private readonly IExceptedBuyBLL exceptedBuyBLL;
        private readonly IStoreEmployeesBLL storeEmployeesBLL;
        private readonly IStoreBLL storeBLL;
        private readonly IProductCodeBLL productCodeBLL;
        private readonly ICustomerTrackingBLL customerTrackingBLL;
        private readonly IPositionBLL positionBLL;

        public CustomerTrackLogController(ICustomerInfoBLL customerInfoBLL, IExceptedBuyBLL exceptedBuyBLL, IStoreEmployeesBLL storeEmployeesBLL, IStoreBLL storeBLL, IProductCodeBLL productCodeBLL, ICustomerTrackingBLL customerTrackingBLL, IPositionBLL positionBLL)
        {
            this.customerInfoBLL = customerInfoBLL;
            this.exceptedBuyBLL = exceptedBuyBLL;
            this.storeEmployeesBLL = storeEmployeesBLL;
            this.storeBLL = storeBLL;
            this.productCodeBLL = productCodeBLL;
            this.customerTrackingBLL = customerTrackingBLL;
            this.positionBLL = positionBLL;
        }



        private int employeeID;//员工id
        private string employeeName;//员工姓名
        private string store;//当前店铺名称
        private int storeID;//当前店铺id
        // private IQueryable<Employees> workers;//所有员工信息
        private IQueryable<CustomerInfoModel> customerInfoModels;//所有接待信息
        private IQueryable<CustomerExceptedBuyModel> exceptedBuyModels;//预计购买




        // GET: CustomerTrackLog
        /// <summary>
        /// 跟进日志
        /// </summary>
        /// <param name="id">接待id</param>
        /// <param name="sortOrder">排序关键字</param>
        /// <param name="searchString">搜索词</param>
        /// <param name="currentFilter"></param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public ActionResult TrackLogIndex(int? id, string sortOrder, string searchString, string currentFilter, int? page)
        {
            List<CustomerTrackingModel> customerTrackingModels = new List<CustomerTrackingModel>();
            Session["method"] = "N";
            SetEmployee();//获取当前人员信息

            ViewBag.TrackingCurrentSort = sortOrder;
            ViewBag.TrackingDate = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.TrackingResult = sortOrder == "last" ? "last_desc" : "last";
            if (id != null && id != 0)
            {
                customerTrackingModels = BuildTrackingInfo(id);//获取当前客户的跟进信息
                ViewBag.Reception = customerInfoBLL.GetModel(p => p.ID == id).接待序号;//将接待序号传到前端
            }
            else
            {
                customerTrackingModels = BuildTrackingInfo(0);//获取当前人员可查看的跟进信息
            }
            if (customerTrackingModels == null)
            {
                return Content("当前操作人并无关联的跟进信息或无进入权限！");
            }
            BuildCustomerInfo();//将顾客接待信息数据优化
            
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
                customerTrackingModels = customerTrackingModels.Where(w => w.客户电话==searchString).ToList();//通过客户电话查找
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

            return View(customerTrackingModels.ToPagedList(pageNumber, pageSize));
        }



        /// <summary>
        /// 添加跟进日志
        /// </summary>
        /// <param name="reception">接待序号</param>
        /// <param name="trackingPeopleName">跟进人姓名</param>
     
        /// <returns></returns>
        public ActionResult AddTrackLogView(string reception)
        {
            Session["method"] = "N";
            SetEmployee();
            CustomerTrackingModel customerTrackingModel = new CustomerTrackingModel
            {
                店铺 = store,
                跟进人 = employeeName
            };
            if (reception != null)
            {
                customerTrackingModel.接待序号 = reception;
                customerTrackingModel.接待ID = customerInfoBLL.GetModel(p=>p.接待序号==reception).ID;
            }
            var trackingPeopleId = storeEmployeesBLL.GetModel(p => p.姓名 == employeeName).ID;
            var customerModels = customerInfoBLL.GetModels(p => p.跟进人ID == trackingPeopleId);

            if (customerModels.Count() == 0)//如果没有查到匹配的跟进信息
            {
                string str = string.Format("<script>alert('当前没有匹配到任何您可以进行跟进的客户,请让店长为您添加！');parent.location.href='TrackLogIndex';</script>");
                return Content(str);
            }

            ViewBag.ReceptionSer = customerTrackingModel.接待序号;



            return View(customerTrackingModel);
        }

        public ActionResult TrackLogAdd(CustomerTrackingModel customerTrackingModel)
        {
            if (Session["method"].ToString() == "Y")
            {
                string str = string.Format("<script>alert('重复操作！');parent.location.href='TrackLogIndex';</script>");
                return Content(str);
            }
            try
            {


                销售_意向追踪日志 trackLog = new 销售_意向追踪日志();


                trackLog.店铺ID = storeBLL.GetModel(P => P.名称 == customerTrackingModel.店铺).ID;
                    trackLog.备注 = customerTrackingModel.备注;
                trackLog.店长审核 = customerTrackingModel.店长审核;
                trackLog.接待记录ID = customerInfoBLL.GetModel(p => p.接待序号 == customerTrackingModel.接待序号).ID;
                trackLog.是否申请设计师 = customerTrackingModel.是否申请设计师;
                    trackLog.跟进人 = storeEmployeesBLL.GetModel(p => p.姓名 == customerTrackingModel.跟进人).ID;
                trackLog.跟进内容 = customerTrackingModel.跟进内容;
                    trackLog.跟进方式 = customerTrackingModel.跟进方式;
                trackLog.跟进时间 = customerTrackingModel.跟进时间;
                    
                
             
                trackLog.跟进结果 = Enum.GetName(typeof(CustomerTrackResult), customerTrackingModel.跟进结果).ToString();
                if (ModelState.IsValid)
                {
                    customerTrackingBLL.Add(trackLog);
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
                    string s = "添加日志出错：";
                    foreach (var item in sb)
                    {
                        s += item.ToString() + "<br/>";
                    }
                    return Content("<script>alert('" + s + "');window.history.go(-1);</script>");
                }
            }
            catch (Exception ex)
            {

                return Content("添加跟进日志时错误:"+ex.ToString());
            }
            return RedirectToAction("TrackLogIndex",new { id=customerTrackingModel.接待ID});
        }
        /// <summary>
        /// 修改跟进日志
        /// </summary>
        /// <returns></returns>
        public ActionResult EditTrackLogView(int id)
        {
            Session["method"] = "N";
            var model = customerTrackingBLL.GetModel(p => p.id == id);
            if (model == null)
            {
                string str = string.Format("<script>alert('数据已更改，未查到跟进日志信息！');parent.location.href='TrackLogIndex';</script>");
                return Content(str);
            }

            CustomerTrackingModel customerTrackingModel = new CustomerTrackingModel
            {
                Id = model.id,
                备注 = model.备注,
                店铺 = storeBLL.GetModel(p => p.ID == model.店铺ID).名称,
                店长审核 = model.店长审核,
                接待序号 = customerInfoBLL.GetModel(p => p.ID == model.接待记录ID).接待序号,
                接待ID = model.接待记录ID,
                是否申请设计师 = model.是否申请设计师,
                跟进人 = storeEmployeesBLL.GetModel(p => p.ID == model.跟进人).姓名,
                跟进内容 = model.跟进内容,
                跟进方式 = model.跟进方式,
                跟进时间 = model.跟进时间,
               
                客户电话=customerInfoBLL.GetModel(p=>p.ID==model.接待记录ID).客户电话
            };
            switch (model.跟进结果)
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

            return View(customerTrackingModel);
        }


        public ActionResult TrackLogEdit(CustomerTrackingModel customerTrackingModel)
        {

            if (Session["method"].ToString() == "Y")
            {
                string str = string.Format("<script>alert('重复操作！');parent.location.href='TrackLogIndex';</script>");
                return Content(str);
            }
            try
            {


                销售_意向追踪日志 trackLog = new 销售_意向追踪日志
                {
                    id = customerTrackingModel.Id,
                    店铺ID = storeBLL.GetModel(P => P.名称 == customerTrackingModel.店铺).ID,
                    备注 = customerTrackingModel.备注,
                    店长审核 = customerTrackingModel.店长审核,
                    接待记录ID = customerInfoBLL.GetModel(p => p.接待序号 == customerTrackingModel.接待序号).ID,
                    是否申请设计师 = customerTrackingModel.是否申请设计师,
                    跟进人 = storeEmployeesBLL.GetModel(p => p.姓名 == customerTrackingModel.跟进人).ID,
                    跟进内容 = customerTrackingModel.跟进内容,
                    跟进方式 = customerTrackingModel.跟进方式,
                    跟进时间 = customerTrackingModel.跟进时间,
                    跟进结果 = customerTrackingModel.跟进结果.ToString()
                };
                if (ModelState.IsValid)
                {
                    customerTrackingBLL.Modify(trackLog);
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
                    string s = "修改日志出错：";
                    foreach (var item in sb)
                    {
                        s += item.ToString() + "<br/>";
                    }
                    return Content("<script>alert('" + s + "');window.history.go(-1);</script>");
                }
            }
            catch (Exception e)
            {

                return Content("<script>alert('修改日志异常：" + e + "');window.history.go(-1);</script>");
            }
            return RedirectToAction("TrackLogIndex");
        }


        /// <summary>
        /// 根据id删除相应跟踪日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DelTrakRecord(int id)
        {
            var model = customerTrackingBLL.GetModel(p => p.id == id);
            if (model == null)
            {
                string str = string.Format("<script>alert('数据已被更改，未查到跟进日志信息！');parent.location.href='TrackLogIndex';</script>");
                return Content(str);
            }
           

            try
            {

                if (ModelState.IsValid)
                {
                    customerTrackingBLL.DelBy(p => p.id == id);
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
                    string s = "删除日志出错：";
                    foreach (var item in sb)
                    {
                        s += item.ToString() + "<br/>";
                    }
                    return Content("<script>alert('" +s + "');window.history.go(-1);</script>");
                }
            }
            catch (Exception e)
            {

                return Content("<script>alert('删除日志异常：" + e + "');window.history.go(-1);</script>");
            }

            return View();
        }




        /// <summary>
        /// 根据当前操作人员或职位或接待id查找跟进信息
        /// </summary>
        /// <param name="id">接待id</param>
        
        /// <returns>跟进信息</returns>s
        private List<CustomerTrackingModel> BuildTrackingInfo(int? id)
        {
            List<销售_意向追踪日志> mt = new List<销售_意向追踪日志>();
            var e = storeEmployeesBLL.GetModel(p => p.ID == employeeID);
            if (id != 0)
            {
                mt = customerTrackingBLL.GetModels(p => p.接待记录ID == id).ToList();//查看当前客户的跟进信息
            }
            else if (e.职务ID == 3)
            {
                mt = customerTrackingBLL.GetModels(p => p.店铺ID == e.店铺ID).ToList();//店长查看(只有自己店铺的)
            }

            else
            {
                mt = customerTrackingBLL.GetModels(p => p.跟进人 == employeeID).ToList();//店员查看(只有自己跟进的)
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
                    接待ID =item.接待记录ID,
                    是否申请设计师 = item.是否申请设计师,
                    跟进人 = storeEmployeesBLL.GetModel(p => p.ID == item.跟进人).姓名,
                    跟进内容 = item.跟进内容,
                    跟进方式 = item.跟进方式,
                    跟进时间 = item.跟进时间,
              
                    店铺 = storeBLL.GetModel(p => p.ID == item.店铺ID).名称
                };
                customerTrackingModels.Add(customerTrackingModel);
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

            }

            return customerTrackingModels;

        }


        /// <summary>
        /// 根据接待id查询产品信息
        /// </summary>
        private void BuildExceptedBuy(int id)
        {
            if (id == 0)
            {
                return;
            }
            List<CustomerExceptedBuyModel> models = new List<CustomerExceptedBuyModel>();
            var exceptedBuy = exceptedBuyBLL.GetListBy(p => p.接待ID == id);
            if (exceptedBuy != null)
            {
                foreach (var item in exceptedBuy)
                {
                    CustomerExceptedBuyModel exceptedBuyModel = new CustomerExceptedBuyModel();
                    exceptedBuyModel.ID = item.ID;
                    exceptedBuyModel.型号 = productCodeBLL.GetModel(p => p.ID == item.商品型号ID).型号;
                    exceptedBuyModel.备注 = item.备注;
                    exceptedBuyModel.接待 = id;
                    models.Add(exceptedBuyModel);
                }
                exceptedBuyModels = models.AsEnumerable().AsQueryable();
            }


        }
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
            }
        }

        /// <summary>
        /// 初始化客户接待信息
        /// </summary>
        private void BuildCustomerInfo()
        {
            List<CustomerInfoModel> customerInfoModelsList = new List<CustomerInfoModel>();

            if (customerInfoModels == null)
            {
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


                customerInfoModels = customerInfoModelsList.AsEnumerable().AsQueryable();
            }

        }
    }
}