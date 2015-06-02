using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;

namespace Actias.SQLite
{
    public class SqliteContext : DbContext
    {
        public string Path { get; private set; }

        public SqliteContext(string path) : base(new SQLiteConnection
			{
				ConnectionString = new SQLiteConnectionStringBuilder
				{
					DataSource = path,
					ForeignKeys = true,
					BinaryGUID = false,
				}.ConnectionString
			}, true)
		{
			Path = path;
		}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            Database.SetInitializer(new SqliteInitializer<SqliteContext>(Path, modelBuilder));
        }
    }
}
