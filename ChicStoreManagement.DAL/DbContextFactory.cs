using ChicStoreManagement.Model;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;

namespace ChicStoreManagement.DAL
{
    public partial class DbContextFactory 
    {
        /// <summary>
        /// 创建EF上下文对象,已存在就直接取,不存在就创建,保证线程内是唯一。
        /// </summary>
        public static DbContext Create()
        {
            DbContext dbContext = CallContext.GetData("DbContext") as DbContext;
            if (dbContext == null)
            {
                dbContext = new chicEntities();
                CallContext.SetData("DbContext", dbContext);
            }
            return dbContext;
        }
    }
}
