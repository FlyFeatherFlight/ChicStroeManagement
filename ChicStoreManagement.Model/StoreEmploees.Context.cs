﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChicStoreManagement.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class chicEntities : DbContext
    {
        public chicEntities()
            : base("name=chicEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<销售_店铺档案> 销售_店铺档案 { get; set; }
        public DbSet<销售_店铺员工档案> 销售_店铺员工档案 { get; set; }
        public DbSet<销售_职务> 销售_职务 { get; set; }
    }
}
