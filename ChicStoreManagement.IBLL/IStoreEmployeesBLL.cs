using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.IBLL
{
    public interface IStoreEmployeesBLL
    {
        //获取所有的员工信息
        List<StoreEmploeeModel> GetAll(Expression<Func<StoreEmploeeModel, bool>> where);

        ///更新员工信息
        bool UpdateUser(StoreEmploeeModel storeEmploee);

        ///添加员工信息
        bool AddUser(StoreEmploeeModel storeEmploee);

        ///删除员工信息
        bool DelUser(StoreEmploeeModel storeEmploee);

        ///使用唯一的标识查询实体集
        ///</summary>
        ///<param name="id">标识</param>
        StoreEmploeeModel SelOne(string id);
    }
}
