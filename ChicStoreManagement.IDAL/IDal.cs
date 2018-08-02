using ChicStoreManagement.Model;
using System.Collections.Generic;
using System.Linq;

namespace ChicStoreManagement.IDAL
{   
	public partial interface I销售_店铺档案Repository :IBaseRepositoryDAL<销售_店铺档案>
    {      
    }
	public partial interface I销售_店铺员工档案Repository :IBaseRepositoryDAL<销售_店铺员工档案>
    {
        销售_店铺员工档案 GetById(int id);
        IQueryable<销售_店铺员工档案> GetAll(string entity);
    }
	public partial interface I销售_职务Repository :IBaseRepositoryDAL<销售_职务>
    {      
    }
	
}