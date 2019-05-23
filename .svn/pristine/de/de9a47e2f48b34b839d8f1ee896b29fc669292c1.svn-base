using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicStoreManagement.BLL
{
    /// <summary>
    /// 登录记录
    /// </summary>
    public partial class LogInRecordBLL:BaseService<销售_登录记录表>, ILogInRecordBLL
    {
        private ILogInRecordDAL logInRecordDAL;

        public LogInRecordBLL(ILogInRecordDAL logInRecordDAL)
        {
            this.logInRecordDAL = logInRecordDAL ?? throw new ArgumentNullException(nameof(logInRecordDAL));
            Dal = logInRecordDAL;
        }
    }
}
