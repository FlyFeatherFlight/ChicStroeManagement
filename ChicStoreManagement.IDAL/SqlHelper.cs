using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicStoreManagement.IDAL
{
    interface SqlHelper
    {
        SqlDataReader ExecReader(string sqlStr);
        SqlDataAdapter SqlDataAdapter();
    }
}
