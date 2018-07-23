using ChicStoreManagement.Entity;
using ChicStoreManagement.IBLL;
using System.Collections.Generic;

namespace ChicStoreManagement.BLL
{
   public   class AccountManageBll: IBLL.AccountManageIBll
    {
        public AccountManageBll()
        {
        }

        

        public bool LoginCheck(AccountEntity account)

        {

            bool flag = false;

            // List<AccountEntity> accountList = new AccountServiceDal().GetAccountInfo();   //校验数据库中用户数据，需使用此代码



           // < span style = "color:#ff0000;" >//Test Account Data</span>

             List < AccountEntity > accountList = new List<AccountEntity>()  //增加两条用户数据

             {

                new AccountEntity("Jarvis","ABC123"),

                new AccountEntity("admin","admin123")

             };



            foreach (AccountEntity accountInfo in accountList)

            {

                if (accountInfo.Name == account.Name && accountInfo.Password == account.Password)

                {

                    flag = true;

                }

            }

            return flag;

        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
