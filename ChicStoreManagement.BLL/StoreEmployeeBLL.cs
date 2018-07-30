using ChicStoreManagement.DAL;
using ChicStoreManagement.IBLL;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChicStoreManagement.BLL
{
   
       public class StoreEmployeeBLL : IStoreEmployeesBLL
        {
            private IStoreEmployeesDAL storeEmployeesDAL = new StoreEmployeesDAL();
            public bool AddUser(StoreEmploeeModel storeEmploee)
            {
                throw new NotImplementedException();
            }

            public bool DelUser(StoreEmploeeModel storeEmploee)
            {
                throw new NotImplementedException();
            }

            //根据条件获取的员工信息
            public List<StoreEmploeeModel> GetAll(Expression<Func<StoreEmploeeModel, bool>> where)
            {
                return storeEmployeesDAL.GetAll(where);

            }

            public StoreEmploeeModel SelOne(string id)
            {
                throw new NotImplementedException();
            }

            public bool UpdateUser(StoreEmploeeModel storeEmploee)
            {
                throw new NotImplementedException();
            }
        }
    }


