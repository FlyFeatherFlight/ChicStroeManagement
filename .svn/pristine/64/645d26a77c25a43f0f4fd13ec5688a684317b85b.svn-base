using ChicStoreManagement.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicStoreManagement.DAL
{
    class AccountManage
    {
        public AccountManage()
        {
        }

        /// <summary>

        /// 获取店员信息

        /// </summary>

        /// <returns></returns>

        public List<AccountEntity> GetAccountInfo()

        {

            string sqlStr = @"ProcSelAccount";  //存储过程名

            List<AccountEntity> accountList = new List<AccountEntity>();



            using (SqlDataReader reader = SqlHelper.ExecReader(sqlStr))   //SqlHelper： SQL帮助类

            {

                while (reader.Read())

                {

                    AccountEntity account = BuildSubject(reader);

                    accountList.Add(account);

                }

            }

            return accountList;

        }



        public AccountEntity BuildSubject(SqlDataReader reader)

        {

            AccountEntity account = new AccountEntity();

            account.Name = reader.GetString(0);

            account.Password = reader.GetString(1);
            return account;
        }
    }

    internal class SqlHelper
    {
        /// <summary>
        /// 数据库查询
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        internal static SqlDataReader ExecReader(string sqlStr)
        {
            throw new NotImplementedException();
        }
    }
}