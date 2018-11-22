using CWMIzhanaka.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CWMIzhanaka.Areas.Admin.Models
{
    public class DataBase : DbContext 
    {
        public DbSet<PageDataHandler> Pages { get; set; }
        public DbSet<SideBarDataHandler> Sidebar { get; set; }
        public DbSet<CategoryDataHandler> Category { get; set; }
        public DbSet<ProductDataHandler> Products { get; set; }
        public DbSet<AccountsDataHandler> Accounts { get; set; }
        public DbSet<RolesDataHandler> Roles { get; set; }
        public DbSet <UserRolesDataHandler> UserRoles { get; set; }
    }
}