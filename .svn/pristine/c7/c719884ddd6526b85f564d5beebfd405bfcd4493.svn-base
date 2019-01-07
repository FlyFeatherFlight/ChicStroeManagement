using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using System.Web.Mvc;

namespace ChicStoreManagement
{
    public class AutoFacConfig
    {

        /// <summary>
        /// 负责调用autofac框架实现业务逻辑层和数据仓储层程序集中的类型对象的创建
        /// 负责创建MVC控制器类的对象(调用控制器中的有参构造函数),接管DefaultControllerFactory的工作
        /// </summary>
        public static void Register()
        {
            //实例化一个autofac的创建容器
            var builder = new ContainerBuilder();
            //告诉Autofac框架，将来要创建的控制器类存放在哪个程序集 (AutoFacMvcDemo)
            Assembly controllerAss = Assembly.Load("ChicStoreManagement.WEB");
            builder.RegisterControllers(controllerAss);
            //builder.RegisterModelBinders(controllerAss);

            //如果有Dal层的话，注册Dal层的组件
            //告诉autofac框架注册数据仓储层所在程序集中的所有类的对象实例
            Assembly dalAss = Assembly.Load("ChicStoreManagement.DAL");
            //创建respAss中的所有类的instance以此类的实现接口存储
            builder.RegisterTypes(dalAss.GetTypes()).AsImplementedInterfaces();

            //告诉autofac框架注册业务逻辑层所在程序集中的所有类的对象实例
            Assembly serviceAss = Assembly.Load("ChicStoreManagement.BLL");
            //创建serAss中的所有类的instance以此类的实现接口存储
            builder.RegisterTypes(serviceAss.GetTypes()).AsImplementedInterfaces();


            //创建一个Autofac的容器
            var container = builder.Build();
            //将MVC的控制器对象实例 交由autofac来创建
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}