using ChicStoreManagement.IOC;
using ChicStoreManagement.Model;

using System.Web.Mvc;

namespace ChicStoreManagement.Controllers
{
    public class BaseController : Controller
    {
        #region 注入属性
       
        public 销售_店铺员工档案 StoreEmployeeService
        {
            get
            {
                return SpringHelper.GetTObject<销售_店铺员工档案>("StoreEmployeeBLL");
            }
        }
        #endregion
        ///// <summary>
        ///// 获取当前登陆人的账户信息
        ///// </summary>
        ///// <returns>账户信息</returns>
        //protected AccountEntity GetCurrentAccount()
        //{
        //    if (Request.Cookies["sessionId"] != null)
        //    {
        //        string sessionId = Request.Cookies["sessionId"].Value;//接收从Cookie中传递过来的Memcache的key
        //        object obj = Common.MemcacheHelper.Get(sessionId);//根据key从Memcache中获取用户的信息
        //        return obj == null ? null : //Common.SerializerHelper.DeserializeToObject<Account>(obj.ToString()); ;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
