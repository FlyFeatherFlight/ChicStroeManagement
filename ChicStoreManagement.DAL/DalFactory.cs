using ChicStoreManagement.IDAL;
using ChicStoreManagement.IOC;

namespace ChicStoreManagement.DAL
{
  public partial class DalFactory
  {
	 public static I销售_店铺档案Repository Get销售_店铺档案Repository
	 {
	   get
	    {
	     return SpringHelper.GetTObject<I销售_店铺档案Repository>("销售_店铺档案Repository");
	    }
	 }
  	 public static I销售_店铺员工档案Repository Get销售_店铺员工档案Repository
	 {
	   get
	    {
	     return SpringHelper.GetTObject<I销售_店铺员工档案Repository>("StoreEmployeesDAL");
	    }
	 }
  	 public static I销售_职务Repository Get销售_职务Repository
	 {
	   get
	    {
	     return SpringHelper.GetTObject<I销售_职务Repository>("销售_职务Repository");
	    }
	 }
   }
}