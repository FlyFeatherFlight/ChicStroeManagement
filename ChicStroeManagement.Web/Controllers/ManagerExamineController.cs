using ChicStoreManagement.IBLL;
using ChicStoreManagement.WEB.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private int employeeID;//员工id
        private string employeeName;//员工姓名
        private string store;//当前店铺名称
        private int storeID;//当前店铺id
        // private IQueryable<Employees> workers;//所有员工信息
        private IQueryable<CustomerInfoModel> customerInfoModels;//所有接待信息
        private IQueryable<CustomerExceptedBuyModel> exceptedBuyModels;//预计购买
        public ManagerExamineController(ICustomerInfoBLL customerInfoBLL, IExceptedBuyBLL exceptedBuyBLL, IStoreEmployeesBLL storeEmployeesBLL, IStoreBLL storeBLL, IProductCodeBLL productCodeBLL, ICustomerTrackingBLL customerTrackingBLL, IPositionBLL positionBLL)
        {
            this.customerInfoBLL = customerInfoBLL;
            this.exceptedBuyBLL = exceptedBuyBLL;
            this.storeEmployeesBLL = storeEmployeesBLL;
            this.storeBLL = storeBLL;
            this.productCodeBLL = productCodeBLL;
            this.customerTrackingBLL = customerTrackingBLL; 
            this.positionBLL = positionBLL;
        }


        // GET: ManagerExamine
        /// <summary>
        /// 显示意向客户名录
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 客户追踪日志审查
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerTrackLogExamineView() {

            return View();
        }

        /// <summary>
        /// 设计跟踪日志审查
        /// </summary>
        /// <returns></returns>
        public ActionResult DesignTrackLogExamineView()
        {
            return View();
        }
        /// <summary>
        /// 诚意/意向客户确认
        /// </summary>
        /// <returns></returns>
        public ActionResult SincerityCustomerConfirmView() {

            return View();

        }



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