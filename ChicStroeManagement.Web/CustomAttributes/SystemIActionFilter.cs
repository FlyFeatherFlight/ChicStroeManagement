using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChicStoreManagement.CustomAttributes
{

    public class SystemIActionFilter : IActionFilter

    {

        //

        // Summary:

        //     Called after the action method is invoked.

        //      在Action返回之后

        // Parameters:

        //   filterContext:

        //     Information about the current request and action.

        public void OnActionExecuted(ActionExecutedContext filterContext)

        {

        }

        //

        // Summary:

        //     Called before the action method is invoked.

        //      在进入Action之前

        //      说明:使用RedirectToRouteResult进行路由值进行重定向时

        //      RouteName 路由名称 

        //      RouteValues 路由值  特别注意第三个值 Permanent 获取一个值

        //      该值指示重定向是否应为永久重定向 如果为true 在本程序会出现问题

        // Parameters:

        //   filterContext:

        //     Information about the current request and action.

        public void OnActionExecuting(ActionExecutingContext filterContext)

        {

            //验证 控制器 视图 

            string tempAction = filterContext.RouteData.Values["action"].ToString();

            string tempController = filterContext.RouteData.Values["controller"].ToString();

            string tempLoginAction = filterContext.RouteData.Values["action"].ToString();



            if (tempAction == "HomeLogin" && tempController == "Home" || tempLoginAction == "UserLogin" ? false : true)

            {

                //请求登录时

                if (tempAction == "UserLogin" && tempController == "Home" ? false : true)

                {

                    //Cookie

                    HttpCookie tempToken = filterContext.HttpContext.Request.Cookies["exclusiveuser_token"];

                    if (tempToken == null)

                    {

                        filterContext.Result = new RedirectToRouteResult("HomeLogin", new RouteValueDictionary(new { controller = "Home", action = "HomeLogin" }), false);

                    }

                    //登录token不为null时  进行合法性验证token 头部,载荷,签名,cookie过期时间

                    if (tempToken == null ? false : true)

                    {

                        //UserToken 方法 将验证 token 合法性 包括token 签名 ,token载荷,cookie 过期时间等

                        //string SystemToken = new SecondTrackToken().UserToken();

                        //if (SystemToken == null)

                        //{

                           filterContext.Result = new RedirectToRouteResult("HomeLogin", new RouteValueDictionary(new { controller = "Home", action = "HomeLogin" }), false);

                        //};

                    }

                }

            }

        }

    }

}