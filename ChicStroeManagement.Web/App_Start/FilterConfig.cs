using ChicStoreManagement.CustomAttributes;
using System.Web;
using System.Web.Mvc;

namespace ChicStoreManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAttributes.AuthorizeFilter());
            //注册异常处理过滤器。  
            filters.Add(new SystemIExceptionFilter());
        }
    }
}
