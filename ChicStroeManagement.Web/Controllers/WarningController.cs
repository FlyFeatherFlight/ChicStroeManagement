using log4net;
using System;
using System.Web.Mvc;

namespace ChicStoreManagement.Controllers
{
    /// <summary>
    /// 异常处理过滤器，使用log4net记录日志，并跳转至错误页面
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class WarningController : HandleErrorAttribute
    {
        ILog log = LogManager.GetLogger(typeof(WarningController));
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string message = string.Format("消息类型：{0}\r\n消息内容：{1}\r\n引发异常的方法：{2}\r\n引发异常源：{3}"
                , filterContext.Exception.GetType().Name
                , filterContext.Exception.Message
                , filterContext.Exception.TargetSite
                , filterContext.Exception.Source + filterContext.Exception.StackTrace
                );
                //记录日志
                log.Error(message);
                //转向
               

                //跳转到自定义的错误页,增强用户体验。
                ActionResult result = new ViewResult() { ViewName = "Error" };
                filterContext.Result = result;
                //异常处理结束后,一定要将ExceptionHandled设置为true,否则仍然会继续抛出错误。
                filterContext.ExceptionHandled = true;

            }
            base.OnException(filterContext);
        }
    }
}


