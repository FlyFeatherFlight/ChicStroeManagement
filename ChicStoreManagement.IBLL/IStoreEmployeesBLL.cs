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
        IQueryable<销售_店铺员工档案> GetAll(string entity);

        ///更新员工信息
        bool UpdateUser(销售_店铺员工档案 storeEmploee);

        ///添加员工信息
        bool AddUser(销售_店铺员工档案 storeEmploee);

        ///删除员工信息
        bool DelUser(销售_店铺员工档案 storeEmploee);

        ///使用唯一的标识查询实体集
        ///</summary>
        ///<param name="id">标识</param>
        销售_店铺员工档案 GetById(int id);
    }
}
