using System.Linq;
using Actias.SQLite.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Actias.SQLite.Tests
{
    [TestClass]
    public class SqliteInitializerTests
    {
        public TestContext TestContext { get; set; }

        public string TestDb
        {
            get { return TestContext.TestRunDirectory  + @"\testDb.sqlite"; }
        }

        [TestMethod]
        public void CanGetUsers()
        {
            using (var context = new MockContext(TestDb))
            {
                Assert.IsTrue(context.Users.Any());
            }
        }

        [TestMethod]
        public void CanGetGroups()
        {
            using (var context = new MockContext(TestDb))
            {
                Assert.IsTrue(context.Groups.Any());
            }
        }

        [TestMethod]
        public void CanGetUserGroups()
        {
            using (var context = new MockContext(TestDb))
            {
                Assert.IsTrue(context.UserGroups.Any());
            }
        }
    }
}
