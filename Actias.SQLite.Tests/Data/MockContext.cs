using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Actias.SQLite.Tests.Data
{
    public class MockContext : SqliteContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<User> Users { get; set; }

        public MockContext(string path) : base(path){}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            Database.SetInitializer(new MockInitializer(Path, modelBuilder));
        }
    }
}
