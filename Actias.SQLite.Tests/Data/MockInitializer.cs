using System.Data.Entity;
using System.Linq;

namespace Actias.SQLite.Tests.Data
{
    public class MockInitializer : CreateIfNotExistsInitializer<MockContext>
    {
        public MockInitializer(string dbPath, DbModelBuilder modelBuilder) : base(dbPath, modelBuilder){}

        protected override void Seed(MockContext context)
        {
            for (var i = 1; i <= 100; i++)
            {
                context.Users.Add(new User { UserId = i, Username = "Test User " + i});
            }

            context.SaveChanges();

            for (var i = 1; i <= 3; i++)
            {
                context.Groups.Add(new Group { GroupId = i, Name = "Test Group " + i });
            }

            context.SaveChanges();


            foreach (var user in context.Users)
            {
                var userGroup = new UserGroup
                {
                    UserId = user.UserId,
                    GroupId = user.UserId <= 33 ? 1 : user.UserId <= 66 ? 2 : 3
                };

                context.UserGroups.Add(userGroup);

                context.SaveChanges();
            }
        }
    }
}
