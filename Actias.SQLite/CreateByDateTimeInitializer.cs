using System.Data.Entity;
using System.IO;

namespace Actias.SQLite
{
    public class CreateAlwaysInitializer<T> : SqliteInitializer<T> where T : DbContext
    {
        public CreateAlwaysInitializer(string dbPath, DbModelBuilder modelBuilder) : base(dbPath, modelBuilder) { }

        public override void InitializeDatabase(T context)
        {
            if (File.Exists(DbPath))
            {
                File.Delete(DbPath);
            }

            base.InitializeDatabase(context);
        }
    }
}
