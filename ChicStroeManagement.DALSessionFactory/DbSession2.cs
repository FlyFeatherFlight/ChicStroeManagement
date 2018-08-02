

using ChicStoreManagement.DAL;
using ChicStoreManagement.IDAL;

namespace ChicStoreManagement.DALSessionFactory
{
	public partial class DBSession : IDBSession
    {
	
		private I销售_店铺档案Repository _销售_店铺档案Repository;
        public I销售_店铺档案Repository I销售_店铺档案Repository
        {
            get
            {
                if(_销售_店铺档案Repository == null)
                {
                   // _销售_店铺档案Repository = new 销售_店铺档案Repository();
				    _销售_店铺档案Repository = DalFactory.Get销售_店铺档案Repository;
                }
                return _销售_店铺档案Repository;
            }
            set { _销售_店铺档案Repository = value; }
        }
	
		private I销售_店铺员工档案Repository _销售_店铺员工档案Repository;
        public I销售_店铺员工档案Repository I销售_店铺员工档案Repository
        {
            get
            {
                if(_销售_店铺员工档案Repository == null)
                {
                   // _销售_店铺员工档案Repository = new 销售_店铺员工档案Repository();
				    _销售_店铺员工档案Repository =DalFactory.Get销售_店铺员工档案Repository;
                }
                return _销售_店铺员工档案Repository;
            }
            set { _销售_店铺员工档案Repository = value; }
        }
	
		private I销售_职务Repository _销售_职务Repository;
        public I销售_职务Repository I销售_职务Repository
        {
            get
            {
                if(_销售_职务Repository == null)
                {
                   // _销售_职务Repository = new 销售_职务Repository();
				    _销售_职务Repository =DalFactory.Get销售_职务Repository;
                }
                return _销售_职务Repository;
            }
            set { _销售_职务Repository = value; }
        }
	}	
}