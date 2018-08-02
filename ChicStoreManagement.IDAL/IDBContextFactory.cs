using System.Data.Entity;

namespace ChicStoreManagement.IDAL
{
    public  interface IDBContextFactory
    {
        DbContext CreateDbContext();
    }
}
