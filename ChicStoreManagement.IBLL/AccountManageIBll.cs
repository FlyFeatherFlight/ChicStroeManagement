using ChicStoreManagement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicStoreManagement.IBLL
{
    public interface AccountManageIBll
    {
       bool LoginCheck(AccountEntity account) ;
    }
}
