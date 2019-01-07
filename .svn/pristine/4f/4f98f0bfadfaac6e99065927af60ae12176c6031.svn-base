using ChicStoreManagement.CustomAttributes;
using log4net;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ChicStoreManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            MvcHandler.DisableMvcResponseHeader = true; //隐藏ASP.NET MVC版本
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("Log4Net.config")));;//读取Log4Net配置信息

            MvcHandler.DisableMvcResponseHeader = true; //隐藏ASP.NET MVC版本

            AreaRegistration.RegisterAllAreas();
           
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles); //启用文件合并和压缩
            //注册AutoFac
            AutoFacConfig.Register();

            RecordLog();
        }


        //采用分布式的方式记录日志
        private void RecordLog()
       {
            //读取日志  如果使用log4net,应用程序一开始的时候，都要进行初始化配置  
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
