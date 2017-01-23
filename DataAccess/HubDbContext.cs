using System.Data.Entity;
using Hub.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Hub.DataAccess
{
    public class HubDbContext : IdentityDbContext<HubUser>
    {
        public HubDbContext() : base("name=hubDB")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            
        }

        public static HubDbContext Create()
        {
            return new HubDbContext();
        }

        public DbSet<Section> Sections { get; set; }
        public DbSet<ContentSection> ContentSections { get; set; }
        public DbSet<Depot> Depots { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}