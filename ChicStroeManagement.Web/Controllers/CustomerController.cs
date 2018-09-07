using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ChicStoreManagement.IBLL;
using ChicStoreManagement.WEB.ViewModel;
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

        private int employeeID;//员工id
        private string employeeName;//员工姓名
        private string store;//当前店铺
        private int storeID;//当前店铺id
                            // private IQueryable<Employees> workers;//所有员工信息
        private IQueryable<CustomerInfoModel> customerInfoModels;
        private IQueryable<CustomerExceptedBuyModel> exceptedBuyModels;
        public CustomerController(ICustomerInfoBLL customerInfoBLL, IExceptedBuyBLL exceptedBuyBLL, IStoreEmployeesBLL storeEmployeesBLL, IStoreBLL storeBLL, IProductCodeBLL productCodeBLL)
        {
            this.customerInfoBLL = customerInfoBLL;
            this.exceptedBuyBLL = exceptedBuyBLL;
            this.storeEmployeesBLL = storeEmployeesBLL;
            this.storeBLL = storeBLL;
            this.productCodeBLL = productCodeBLL;
        }

       


        // GET: Customer
        /// <summary>
        /// 展示客户信息
        /// </summary>
        /// <returns></returns>
        public ViewResult CustomerIndex(string sortOrder, string searchString, string currentFilter, int? page)
        {
            SetEmployee();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Number = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.Name = sortOrder == "last" ? "last_desc" : "last";
            BuildCustomerInfo();//将顾客接待信息数据优化
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
                customerInfoModels = customerInfoModels.Where(w => w.客户姓名.Contains(searchString));//通过姓名查找
            }
            Session["Name"] = customerInfoModels.FirstOrDefault();
            #region 排序，默认按ID升序
            switch (sortOrder)
            {
                case "first_desc":
                    customerInfoModels = customerInfoModels.OrderByDescending(w => w.ID);
                    break;
                case "last_desc":
                    customerInfoModels = customerInfoModels.OrderByDescending(w => w.客户姓名);
                    break;
                case "last":
                    customerInfoModels = customerInfoModels.OrderBy(w => w.客户姓名);
                    break;
                default:
                    customerInfoModels = customerInfoModels.OrderBy(w => w.ID);
                    break;
            }

            #endregion
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(customerInfoModels.ToPagedList(pageNumber, pageSize));

        }

       
        /// <summary>
        /// 添加客户
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCustomerView()
        {
            return View();

        }

        /// <summary>
        /// 修改客户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult EditCustomerView(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            SetEmployee();
            BuildCustomerInfo();
            var customerInfo = customerInfoModels.First(p => p.ID == id);
            BuildExceptedBuy(id);
            customerInfo.customerExceptedBuyModels = exceptedBuyModels;



            return View(customerInfo);

        }

        /// <summary>
        /// 提交修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerEdit(CustomerInfoModel customerInfoModel) {
            return View(customerInfoModel);
        }
        public ActionResult ShowVisitInfo(int id)
        {
            if (id == 0) {
                return View("查询ID不存在！");
            }
            SetEmployee();
            BuildCustomerInfo();
           var customerInfo=customerInfoModels.First(p => p.ID == id);
            BuildExceptedBuy(id);
            customerInfo.customerExceptedBuyModels = exceptedBuyModels;
            return View(customerInfo);

        }

        /// <summary>
        /// 设置当前员工信息
        /// </summary>
        private void SetEmployee()
        {
           
            string userName = HttpContext.User.Identity.Name;
            if (true)
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
                var customer = customerInfoBLL.GetModels(p => p.店铺ID == storeID);//查询所有顾客接待信息
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
                            customerInfo.接待日期 = item.接待日期;
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

        /// <summary>
        /// 根据接待id查询产品信息
        /// </summary>
        private void BuildExceptedBuy(int id) {
            if (id==0)
            {
                return;
            }
            List<CustomerExceptedBuyModel> models = new List<CustomerExceptedBuyModel>();
            var exceptedBuy = exceptedBuyBLL.GetListBy(p=>p.接待ID==id);
            if (exceptedBuy!=null)
            {
                foreach (var item in exceptedBuy)
                {
                    CustomerExceptedBuyModel exceptedBuyModel = new CustomerExceptedBuyModel();
                    exceptedBuyModel.商品型号 = productCodeBLL.GetModel(p => p.ID == item.商品型号ID).型号;
                    exceptedBuyModel.备注 = item.备注;
                    models.Add(exceptedBuyModel);
                }
               exceptedBuyModels= models.AsEnumerable().AsQueryable();
            }
            

        }
    }
}