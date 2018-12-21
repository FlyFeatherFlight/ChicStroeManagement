using ChicStoreManagement.BLL;
using ChicStoreManagement.Model;
using System.Web.Mvc;
using System.Web.Security;
using ChicStoreManagement.CustomAttributes;
using ChicStoreManagement.WEB.ViewModel;
using DotNet.Highcharts.Options;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using System.Collections.Generic;
using System;
using ChicStoreManagement.IBLL;
using System.Linq;

namespace ChicStoreManagement.Controllers
{
    
    public class HomeController: Controller
    {

        private IStoreEmployeesBLL storeEmployeesBLL;
        private ICustomerInfoBLL customerInfoBLL;
        private ICustomerTrackingBLL CustomerTrackingBLL;
        private IDesignSubmitBLL DesignSubmitBLL;
        private IDesignTrackingLogBLL DesignTrackingLogBLL;
        private IDesignResultBLL DesignResultBLL;
        private IStoreBLL storeBLL;

        private int employeeID;//员工id
        private string employeeName;//员工姓名
        private string store;//当前店铺名称
        private int storeID;//当前店铺id
        public HomeController(IStoreEmployeesBLL storeEmployeesBLL, ICustomerInfoBLL customerInfoBLL, ICustomerTrackingBLL customerTrackingBLL, IDesignSubmitBLL designSubmitBLL, IDesignTrackingLogBLL designTrackingLogBLL, IDesignResultBLL designResultBLL, IStoreBLL storeBLL)
        {
            this.storeEmployeesBLL = storeEmployeesBLL;
            this.customerInfoBLL = customerInfoBLL;
            CustomerTrackingBLL = customerTrackingBLL;
            DesignSubmitBLL = designSubmitBLL;
            DesignTrackingLogBLL = designTrackingLogBLL;
            DesignResultBLL = designResultBLL;
            this.storeBLL = storeBLL;
        }


        /// <summary>
        /// 门店管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string userName = HttpContext.User.Identity.Name;
            if (userName != null)
            {
                var employees = HttpContext.Session["Employee"] as Employees;

                ViewBag.Store = employees.店铺;
                ViewBag.IsManager = employees.是否店长;
                ViewBag.Employee = employees.姓名;
                ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employees.ID).是否店长;
                ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employees.ID).是否设计师;
                ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employees.ID).是否销售;
            }
            SetEmployee();
            ViewBag.CustomerCount = ""+customerInfoBLL.GetModels(p=>p.店铺ID==storeID).Count();
           var n= customerInfoBLL.GetModels(p => p.店铺ID == storeID).Count(); 
            ViewBag.DesignApplyCount = ""+DesignSubmitBLL.GetModels(p=>p.店铺ID==storeID).Count();

            ViewBag.DesignResultCount = "" + DesignResultBLL.GetModels(p => p.店铺ID == storeID).Count() ;
            //创建区域1
            var series1 = new Series();
            series1.Name = "区域一";

            //Poin数组
            Point[] series1Points =
            {
                new Point(){X = 0.0, Y = 0.0},
                new Point() {X = 10.0, Y = 0.0},
                new Point() {X = 10.0, Y = 10.0},
                new Point() {X = 0.0, Y = 10.0}
            };
            series1.Data = new Data(series1Points);

            //创建区域2
            var series2 = new Series();
            series2.Name = "区域二";

            //Point数组
            Point[] series2Points =
                {
                    new Point() {X = 5.0, Y = 5.0},
                    new Point() {X = 15.0, Y =5.0},
                    new Point() {X = 15.0, Y = 15.0},
                    new Point() {X = 5.0, Y = 15.0}
                };
            series2.Data = new Data(series2Points);

            //把2个区域加入到Series集合中
            var chartSeries = new List<Series>();
            chartSeries.Add(series1);
            chartSeries.Add(series2);

            //创建chart model
            var chart1 = new Highcharts("我的第一个区域图表");
            chart1.InitChart(new Chart() { DefaultSeriesType = ChartTypes.Area })
                .SetTitle(new Title() { Text = "我的第一个区域图表" })
                .SetSeries(chartSeries.ToArray());

            ViewBag.ChartModel = chart1;
            return View();

        }

        /// <summary>
        /// 公司简介
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        /// <summary>
        /// 公司联系方式
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// 获取数据接口
        /// </summary>
        /// <param name="BeformDays">前多少天</param>
        /// <param name="Type">请求类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDataList(int BeformDays, int Type,string DataType)
        {
            //时间当然大家可以根据自己需要统计的数据进行整合我这里是用来演示就没有用数据库了
              var Time = new List<String>();
            //数量
            var Count = new List<int>();
            var PieData = new List<ReportDatas>();
            //Type为1表示曲线和柱状数据
            if (Type == 1)
            {
                for (int i = 0; i < BeformDays; i++)
                {
                    Time.Add(DateTime.Now.AddDays(-
                    BeformDays).ToShortDateString());
                    Count.Add(i + 1);
                }
            }
            else//饼状图
            {
                for (int i = 0; i < BeformDays; i++)
                {
                    var my = new ReportDatas();
                    my.Count = i + 1;
                    my.Time = DateTime.Now.AddDays(-
                    BeformDays).ToShortDateString();
                    PieData.Add(my);
                }
            }


            var Obj = new
            {
                Times = Time,
                Counts = Count,
                PieDatas = PieData
            };

            return Json(Obj, JsonRequestBehavior.AllowGet);
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
    }
}
