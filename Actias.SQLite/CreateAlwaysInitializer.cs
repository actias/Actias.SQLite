using System;
using System.Data.Entity;
using System.IO;

namespace Actias.SQLite
{
    public class CreateByDateTimeInitializer<T> : SqliteInitializer<T> where T : DbContext
    {
        public long TimeInSeconds { get; private set; }

        public CreateByDateTimeInitializer(string dbPath, DbModelBuilder modelBuilder, long timeInSeconds) : base(dbPath, modelBuilder)
        {
            TimeInSeconds = timeInSeconds;
        }

        public override void InitializeDatabase(T context)
        {

            if (File.Exists(DbPath) && (DateTime.UtcNow - File.GetCreationTimeUtc(DbPath)).TotalSeconds > TimeInSeconds)
            {
                File.Delete(DbPath);
            }

            base.InitializeDatabase(context);
        }
    }
}
