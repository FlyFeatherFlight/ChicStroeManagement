using ChicStoreManagement.IBLL;
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
        /// <summary>
        /// 身份检测判断
        /// </summary>
        private bool isState;

        /// <summary>
        /// 当前操作人员名字
        /// </summary>
        private string userName;
        /// <summary>
        /// 当前操作人员信息
        /// </summary>
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
            userName = filterContext.HttpContext.User.Identity.Name;//取得当前用户名

            if (filterContext.HttpContext.Session["Employee"] == null || user == null || !user.Identity.IsAuthenticated)

            {
                filterContext.HttpContext.Session.RemoveAll();

                isState = false;
                filterContext.Result = new ContentResult { Content = @"抱歉,您还未登录！" };
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }
            else
            {

                isState = true;
            }
            #endregion



            #region
            #endregion
            #region 权限验证


            GetEmployee(filterContext);//得到当前员工信息


            fcinfo = new FilterContextInfo(filterContext);
            string actionName = fcinfo.ActionName;//获取域名
            string contollerName = fcinfo.ControllerName;//获取 controllerName 名称

            ///检查操作权限
            CheckAuth(Employees.职务,Employees.停用标志, actionName, contollerName);

            if (isState)//如果满足
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
        /// <summary>
        /// 得到当前员工信息
        /// </summary>
        /// <param name="filterContext"></param>
        private void GetEmployee(ActionExecutingContext filterContext)
        {
            if (userName != null)
            {
                Employees = filterContext.HttpContext.Session["Employee"] as Employees;
                //HttpCookie httpCookie = new HttpCookie("user")
                //{
                //    Value = userName
                //};
                //Response.Cookie.Add(httpCookie);
            }
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="positionName">职位</param>
        /// <param name="actionName">action名字</param>
        /// <param name="contollerName">control名</param>
        private void CheckAuth(string positionName,bool stopFlag, string actionName, string contollerName)
        {

            if (stopFlag == true)
            {  
                //停用之后，不可登陆
                isState = false;
                return;
            }
            if (contollerName != "ManagerExamine" && contollerName != "Manager"&& contollerName!= "ManagerGoal")
            {
                isState = true;
                return;//如果是非管理者页面 任何人都可以访问
            }
            if (contollerName == "Manager" || contollerName == "ManagerExamine"|| contollerName== "ManagerGoal")
            {
                if (positionName=="店长"||positionName=="老板")
                {
                isState = true;//店长或者老板操作
                return;
                }
               
            }
            else
            {
                isState = false;
                return;
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