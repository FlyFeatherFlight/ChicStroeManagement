using ChicStoreManagement.IBLL;
using ChicStoreManagement.Model;

namespace ChicStoreManagement.BLL
{
	public partial class 销售_店铺档案Service :BaseService<销售_店铺档案>,I销售_店铺档案Service
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.I销售_店铺档案Repository;
        }
    }   
	public partial class StoreEmployeeBLL :BaseService<销售_店铺员工档案>,IStoreEmployeeBLL
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.I销售_店铺员工档案Repository;
        }
    }   
	public partial class 销售_职务Service :BaseService<销售_职务>,I销售_职务Service
    {
        public override void SetCurretnRepository()
        {
            CurrentRepository = this.GetCurrentDbSession.I销售_职务Repository;
        }
    }   
	
}