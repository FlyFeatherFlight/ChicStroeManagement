using ChicStoreManagement.WEB.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ChicStoreManagement.CustomAttributes
{

    ///　<summary>
    ///　权限拦截
    ///　</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeFilter : ActionFilterAttribute
    {
        FilterContextInfo fcinfo;
        private bool isstate = true;
        private bool islogin = false;
        private  string userName;
        private Employees Employees;

        // OnActionExecuted 在执行操作方法后由 ASP.NET MVC 框架调用。
        // OnActionExecuting 在执行操作方法之前由 ASP.NET MVC 框架调用。
        // OnResultExecuted 在执行操作结果后由 ASP.NET MVC 框架调用。
        // OnResultExecuting 在执行操作结果之前由 ASP.NET MVC 框架调用。

        /// <summary>
        /// 在执行操作方法之前由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            //检查是否免检页面
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                     || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (skipAuthorization)
            {
                return;
            }

            #region 是否已经登陆
            var user = filterContext.HttpContext.User;
            var name = filterContext.HttpContext.User.Identity.Name;
            if (user == null || !user.Identity.IsAuthenticated)

            {
                filterContext.Result = new ContentResult { Content = @"抱歉,您还未登录！" };
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }
            
            #endregion

          

            #region
            #endregion
            #region 权限验证
            
            Employees = new Employees();
            filterContext.HttpContext.Session["Employee"] = "1";
            Employees = filterContext.HttpContext.Session["Employee"] as Employees;
            fcinfo = new FilterContextInfo(filterContext);
            var actionName = fcinfo.ActionName;//获取域名
            var contollerName = fcinfo.ControllerName;//获取 controllerName 名称


            Checkstate(Employees.职务, actionName, contollerName);
           
            if (isstate)//如果满足
            {
                return;
               // filterContext.Result = new HttpUnauthorizedResult();//直接URL输入的页面地址跳转到登陆页  
                // filterContext.Result = new RedirectResult("http://www.baidu.com");//也可以跳到别的站点

            }
            else
            {
                filterContext.Result = new ContentResult { Content = @"抱歉,你不具有当前操作的权限！" };// 直接返回 return Content("抱歉,你不具有当前操作的权限！")
            }
            #endregion
        }

        private void Checkstate(string positionName, string actionName, string contollerName)
        {
            islogin = true;
            if (actionName == "Manager" && positionName == "店长" || actionName == "" && positionName == "销售顾问" || positionName == "" && positionName == "驻店设计师")
            {
                isstate = true;

            }
        }



        /// <summary>
        /// 在执行操作方法后由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        ///  OnResultExecuted 在执行操作结果后由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
        /// <summary>
        /// OnResultExecuting 在执行操作结果之前由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }


        public class FilterContextInfo
        {
            public FilterContextInfo(ActionExecutingContext filterContext)
            {
                #region 获取链接中的字符
                // 获取域名
                DomainName = filterContext.HttpContext.Request.Url.Authority;

                //获取模块名称
                //  module = filterContext.HttpContext.Request.Url.Segments[1].Replace('/', ' ').Trim();

                //获取 controllerName 名称
                ControllerName = filterContext.RouteData.Values["controller"].ToString();

                //获取ACTION 名称
                ActionName = filterContext.RouteData.Values["action"].ToString();

                #endregion
            }
            /// <summary>
            /// 获取域名
            /// </summary>
            public string DomainName { get; set; }
            /// <summary>
            /// 获取模块名称
            /// </summary>
            public string Module { get; set; }
            /// <summary>
            /// 获取 controllerName 名称
            /// </summary>
            public string ControllerName { get; set; }
            /// <summary>
            /// 获取ACTION 名称
            /// </summary>
            public string ActionName { get; set; }

        }

    }


}