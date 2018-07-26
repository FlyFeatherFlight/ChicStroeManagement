using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ChicStroeManagement.ViewModel
{
    /// <summary>
    /// 访问实体类
    /// </summary>
    public class VisitInfoModel
    {
        DataTable dt = new DataTable("test");
        public VisitInfoModel()
        {

        }

        public VisitInfoModel(string accountName, string customerName, DateTime startTime, string visitWay, string visitResult, string managerTips)
        {
            AccountName = accountName;
            CustomerName = customerName;
            StartTime = startTime;
            VisitWay = visitWay;
            VisitResult = visitResult;
            ManagerTips = managerTips;
        }

        public string AccountName { get; set; }//雇员姓名
        public string  CustomerName { get; set; }//客户名字
        public DateTime StartTime  { get; set; }//开始时间

        public string VisitWay { get; set; }//沟通方式
        public string VisitResult { get; set; }//结果
        public string ManagerTips { get; set; }//店长批注
        public DataTable MakeData()
        {

            dt.Columns.Add("1", typeof(String));
            dt.Columns.Add("2", typeof(String));
            dt.Columns.Add("3", typeof(String));
            dt.Columns.Add("4", typeof(String));
            dt.Columns.Add("5", typeof(String));
            dt.Columns.Add("6", typeof(String));
            dt.Rows.Add("1", "2", "3", "4", "5", "6");
            dt.Rows.Add("1", "2", "3", "4", "5", "6");
            dt.Rows.Add("1", "2", "3", "4", "5", "6");
            dt.Rows.Add("1", "2", "3", "4", "5", "6");
            dt.Rows.Add("1", "2", "3", "4", "5", "6");
            return dt;

        }
    }
}