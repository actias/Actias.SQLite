using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Actias.SQLite
{
    public class SqliteInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        public DbModelBuilder ModelBuilder { get; private set; }
        public string DbPath { get; private set; }

        public SqliteInitializer(string dbPath, DbModelBuilder modelBuilder)
        {
            DbPath = dbPath;
            ModelBuilder = modelBuilder;
        } 
        
        public virtual void InitializeDatabase(T context)
        {
            var model = ModelBuilder.Build(context.Database.Connection);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    SqliteHelpers.CreateDatabase(context.Database, model);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Seed(context);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        protected virtual void Seed(T context) { }
    }

    public class Index
    {
        public string Name { get; set; }
        public string Table { get; set; }
        public List<string> Columns { get; set; }
    }
}
