using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;

namespace Actias.SQLite
{
    public static class SqliteHelpers
    {
        public static void CreateDatabase(Database database, DbModel model)
        {
            foreach (var sql in model.StoreModel.EntityTypes.Select(type => type.CreateTableStatement(model)))
            {
                database.ExecuteSqlCommand(sql);
            }

            foreach (var sql in model.CreateIndexStatements())
            {
                database.ExecuteSqlCommand(sql);
            }
        }

        public static string CreateTableStatement(this EntityType entity, DbModel model)
        {
            var definitions = entity
                .Properties
                .Select(property => property.CreateColumnStatement())
                .ToList();

            var tableName = entity.TableName();

            if (entity.KeyProperties.Any())
            {
                var keys = entity.KeyProperties.Select(x => x.Name);
                definitions.Add(string.Format("primary key ({0})", string.Join(", ", keys)));
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var association in from a in model.StoreModel.AssociationTypes where a.Constraint.ToRole.Name == entity.Name select a)
            {
                var to = association.Constraint.ToProperties.Select(x => x.ColumnName()).ToList();
                var from  = association.Constraint.ToProperties.Select(x => x.ColumnName()).ToList();
                var table = association.Constraint.FromRole.TableName(model);

                definitions.Add(string.Format("foreign key ({0}) references {1} ({2})", string.Join(", ", to), table, string.Join(", ", from)));
            }

            return string.Format("create table if not exists [{0}] (\n{1}\n);", tableName, string.Join(",\n", definitions));
        }

        public static string CreateColumnStatement(this EdmProperty property)
        {
            var properties = new HashSet<string>();

            if (!property.Nullable)
            {
                properties.Add("not null");
            }

            var annotations = property.MetadataProperties
                .Select(x => x.Value)
                .OfType<IndexAnnotation>();

            if (annotations.SelectMany(annotation => annotation.Indexes).Any(attr => attr.IsUnique))
            {
                properties.Add("UNIQUE");
            }

            return string.Format("[{0}] {1} {2}", property.ColumnName(), property.TypeName, string.Join(" ", properties));
        }

        public static IEnumerable<string> CreateIndexStatements(this DbModel model)
        {
            var indicies = new Dictionary<string, Index>();

            foreach (var type in model.StoreModel.EntityTypes)
            {
                foreach (var property in type.Properties)
                {
                    var annotations = property.MetadataProperties
                        .Select(x => x.Value)
                        .OfType<IndexAnnotation>();

                    foreach (var attribute in annotations.SelectMany(annotation => annotation.Indexes).Where(attribute => !string.IsNullOrEmpty(attribute.Name)))
                    {
                        Index index;

                        if (!indicies.TryGetValue(attribute.Name, out index))
                        {
                            index = new Index
                            {
                                Name = attribute.Name,
                                Table = type.TableName(),
                                Columns = new List<string>(),
                            };

                            indicies.Add(index.Name, index);
                        }

                        index.Columns.Add(property.ColumnName());
                    }
                }
            }

            return indicies
                .Values
                .Select(index => string.Format("create index {0} on {1} ({2});", index.Name, index.Table, string.Join(", ", index.Columns)))
                .ToList();
        }

        public static string TableName(this RelationshipEndMember member, DbModel model)
        {
            var entity = model.StoreModel.EntityTypes.FirstOrDefault(x => x.Name == member.Name);
            return entity == null ? member.Name : entity.TableName();
        }


        public static string TableName(this EntityType entity)
        {
            MetadataProperty metadataProperty;

            return entity.MetadataProperties.TryGetValue("TableName", false, out metadataProperty) ? metadataProperty.Value.ToString() : entity.Name;
        }

        public static string ColumnName(this EdmProperty property)
        {
            var column = property.MetadataProperties.FirstOrDefault(x => x.Name == "ColumnName");
            return column != null ? (string)column.Value : property.Name;
        }
    }
}
