using ChicStoreManagement.Common;
using log4net;
using ServiceStack.Redis;
using System.Web.Mvc;

namespace ChicStoreManagement.CustomAttributes
{

    public class SystemIExceptionFilter : HandleErrorAttribute

    {
        //private static readonly ILog logs = LogHelper.GetInstance(); //LogManager.GetLogger(typeof(TEST));
        //public static RedisClient client = new RedisClient("127.0.0.1", 6379);//发布到正式环境时，记得更改IP地址和默认端口，并且设置密码
        #region 如果设置密码
        //static string host = "127.0.0.1";/*访问host地址*/
        //static string password = "2016@Test.88210_yujie";/*实例id:密码*/
        //static readonly RedisClient client = new RedisClient(host, 6379, password); 
        #endregion
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            //client.EnqueueItemOnList("errorMsg", filterContext.Exception.ToString());
            LogHelper.WriteLog(filterContext.Exception.ToString());
            ActionResult result = new ViewResult() { ViewName = "~/Views/Warning/RepeatSubmit.cshtml" };
            filterContext.Result = result;
            //异常处理结束后,一定要将ExceptionHandled设置为true,否则仍然会继续抛出错误。
                filterContext.ExceptionHandled = true;
            //filterContext.HttpContext.Response.Redirect("/Error.html");
            //Response.Redirect("/CustomErrorHandle/CustomErrorPage");
        }
    }
    class LogHelper
    {
        public static void WriteLog(string txt)
        {
            ILog log = LogManager.GetLogger("log4netlogger");
            log.Error(txt);
        }
    }
}