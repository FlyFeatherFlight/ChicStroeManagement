using ChicStoreManagement.IBLL;
using ChicStoreManagement.WEB.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace ChicStoreManagement.Controllers
{
    public class DesignSchemeController : Controller
    {
        private readonly ICustomerInfoBLL customerInfoBLL;
        private readonly IDesign_CustomerExceptedBuyBLL design_CustomerExceptedBuyBLL;
        private readonly IDesignSubmitBLL designSubmitBLL;
       
        private readonly IStoreBLL storeBLL;
        private readonly IPositionBLL positionBLL;
        private readonly IStoreEmployeesBLL storeEmployeesBLL;
        private int employeeID;
        private string employeeName;
        private string store;
        private int storeID;

        public DesignSchemeController(ICustomerInfoBLL customerInfoIBLL, IDesign_CustomerExceptedBuyBLL design_CustomerExceptedBuyBLL, IDesignSubmitBLL designSubmitBLL, IStoreBLL storeBLL, IPositionBLL positionBLL, IStoreEmployeesBLL storeEmployeesBLL)
        {
            this.customerInfoBLL = customerInfoIBLL;
            this.design_CustomerExceptedBuyBLL = design_CustomerExceptedBuyBLL;
            this.designSubmitBLL = designSubmitBLL;
            this.storeBLL = storeBLL;
            this.positionBLL = positionBLL;
            this.storeEmployeesBLL = storeEmployeesBLL;
        }

        /// <summary>
        /// 设计方案
        /// </summary>
        /// <returns></returns>
        // GET: DesignScheme
        public ActionResult DesignSchemeIndex(int? id, string sortOrder, string searchString, string currentFilter, int? page)
        {

            Session["method"] = "N";
            SetEmployee();//获取当前人员信息
            var costomerModels = BuildCustomerInfo();
            ViewBag.DesignSubCurrentSort = sortOrder;
            ViewBag.DesignSubDate = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.DesignSubResult = sortOrder == "last" ? "last_desc" : "last";
            List<DesignSubmitModel> designSubmitModels = new List<DesignSubmitModel>();
            //构建设计表信息  
           designSubmitModels = BuildDesignSubInfo(id).ToList();
            if (designSubmitModels==null)
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

            if (!string.IsNullOrEmpty(searchString)) {
                designSubmitModels = designSubmitModels.Where(w => w.联系方式 == searchString).ToList();
            }
            #region 排序，默认按ID升序
            switch (sortOrder)
            {
                case "first_desc":
                    designSubmitModels = designSubmitModels.OrderByDescending(w => w.项目提交时间).ToList();
                    break;
                case "last_desc":
                    designSubmitModels = designSubmitModels.OrderByDescending(w => w.有效订单).ToList();
                    break;
                case "last":
                    designSubmitModels = designSubmitModels.OrderBy(w => w.有效订单).ToList();
                    break;
                default:
                    designSubmitModels = designSubmitModels.OrderBy(w => w.项目提交时间).ToList();
                    break;
            }

            #endregion
         
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(designSubmitModels.ToPagedList(pageNumber,pageSize));
        }

        /// <summary>
        /// 添加设计方案
        /// </summary>
        /// <returns></returns>
        public ActionResult AddDesignView()
        {
            return View();
        }

        /// <summary>
        /// 申请设计
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplyDesignView()
        {
                     
            return View();

        }
        /// <summary>
        /// 添加设计进度日志
        /// </summary>
        /// <returns></returns>
        public ActionResult AddDesignTrackLogView()
        {
            return View();
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

        /// <summary>
        /// 构建客户信息
        /// </summary>
        /// <returns>客户信息</returns>
        public IQueryable<CustomerInfoModel> BuildCustomerInfo()
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
        public IQueryable<DesignSubmitModel> BuildDesignSubInfo(int? id) {
            List<销售_设计案提交表> designSubModelList = new List<销售_设计案提交表> ();
            if (id != 0)
            {   //根据接待ID查询当前客户的设计提交表
                designSubModelList = designSubmitBLL.GetModels(p => p.接待记录ID == id).ToList();
            }
            else if (storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID == 3)
            {   //店长可以查看所有信息
                designSubModelList = designSubmitBLL.GetModels(p => true).ToList();
            }
            else {
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
                designSubmitModel.店长确认 = item.店长确认;
                designSubmitModel.接待记录ID = item.接待记录ID;
                designSubmitModel.整体软装配饰 = item.整体软装配饰;
                designSubmitModel.楼盘具体位置 = item.楼盘具体位置;
                designSubmitModel.职业 = item.职业;
                designSubmitModel.联系方式 = item.联系方式;
                designSubmitModel.装修进度 = item.装修进度;
                designSubmitModel.装修风格 = item.装修风格;
                designSubmitModel.设计人员 = item.设计人员;
                designSubmitModel.设计人员确认 = item.设计人员确认;
                designSubmitModel.销售人员 = item.销售人员;
                designSubmitModel.面积大小 = item.面积大小;
                designSubmitModel.项目方案要求 = item.项目方案要求;
                designSubmitModel.预算 = item.预算;
                designSubmitModel.项目提交时间 = item.项目提交时间;
                designSubmitModel.项目量房时间 = item.项目量房时间;
                designSubmitModel.项目预计完成时间 = item.项目预计完成时间;
                designSubmitModel.备注 = item.备注;
                if (item.店长!=null&& item.店长确认==true&&item.设计人员!=null&&item.设计人员确认==true&&item.销售人员!=null) {
                    designSubmitModel.有效订单 = true;
                }
                designSubmitModelList.Add(designSubmitModel);
            }
            return designSubmitModelList.AsQueryable();
        }
    }
}