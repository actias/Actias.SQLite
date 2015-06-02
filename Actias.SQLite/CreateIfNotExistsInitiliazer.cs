using System.Data.Entity;
using System.IO;

namespace Actias.SQLite
{
    public class CreateIfNotExistsInitializer<T> : SqliteInitializer<T> where T : DbContext
    {
        public CreateIfNotExistsInitializer(string dbPath, DbModelBuilder modelBuilder) : base(dbPath, modelBuilder) { }

        public override void InitializeDatabase(T context)
        {
            if (File.Exists(DbPath))
            {
                return;
            }

            base.InitializeDatabase(context);
        }
    }
}
