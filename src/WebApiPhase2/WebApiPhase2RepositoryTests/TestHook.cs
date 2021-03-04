using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiPhase2RepositoryTests.TestUtilites;

namespace WebApiPhase2RepositoryTests
{
    [TestClass]
    public class TestHook
    {
        internal static string SampleDbConnection =>
            string.Format(TestDbConnenction.LocalDb.LocalDbConnectionString, DatabaseName.SampleDB);

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            var sampleDbDatabase = new TestDbUtilities(DatabaseName.SampleDB);
            if (sampleDbDatabase.IsLocalDbExists())
            {
                sampleDbDatabase.DeleteLocalDb();
            }
            sampleDbDatabase.CreateDatabase();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            var defaultDatabase = new TestDbUtilities(DatabaseName.Default);
            defaultDatabase.DeleteLocalDb(SampleDbConnection);
        }
    }
}
