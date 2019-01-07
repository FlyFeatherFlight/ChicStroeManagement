using Spring.Context;
using Spring.Context.Support;

namespace ChicStoreManagement.IOC
{
    public  class SpringHelper
    {
        #region Spring容器上下文+IApplicationContext+SpringContext
        /// <summary>
        /// Spring容器上下文
        /// </summary>
        /// <returns></returns>
        private static IApplicationContext SpringContext {

            get
           {
                return ContextRegistry.GetContext();
            }
        }
        #endregion

        #region 获取 配置文件的对象
        /// <summary>
        /// 获取配置文件的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static T GetTObject<T>(string objName) where T : class {
            return (T)SpringContext.GetObject(objName);
        }
        #endregion
    }
}
