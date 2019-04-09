using ChicStoreManagement.Controllers;
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
            filters.Add(new WarningController());
        }
    }
}
