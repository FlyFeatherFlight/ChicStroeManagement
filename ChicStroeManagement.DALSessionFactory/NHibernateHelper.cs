using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicStoreManagement.DALSessionFactory
{
   public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        /// <summary>
        /// 创建ISessionFactory
        /// </summary>
        public static ISessionFactory SessionFactory
        {
            get
            {
                //配置ISessionFactory
                return _sessionFactory == null ? (new Configuration()).Configure().BuildSessionFactory() : _sessionFactory;
            }
        }
    }
}