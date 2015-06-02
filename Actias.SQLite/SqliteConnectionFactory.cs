using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;

namespace Actias.SQLite
{
    public class SqliteConnectionFactory : IDbConnectionFactory
    {
        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            return new SQLiteConnection(nameOrConnectionString);
        }
    }
}
