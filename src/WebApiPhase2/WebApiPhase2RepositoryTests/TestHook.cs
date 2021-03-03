using FluentAssertions;
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
        internal static TestSettings CurrentTestSetting { get; set; }

        private static string DatabaseType { get; set; }

        internal static string DatabaseIp { get; set; }

        private static string DatabaseName => "TestDB";

        /// <summary>
        /// Connection string
        /// </summary>
        internal static string DatabaseConnectionString => string.Format(TestDbConnections.LocalDB.Database, DatabaseName);

        [AssemblyInitialize]
        [Timeout(300)]
        public static void AssemblyInitialize(TestContext context)
        {
            TestLocalDbProcess.CreateDatabase(TestDbConnections.LocalDB.Master, DatabaseName);

            AssertionOptions.AssertEquivalencyUsing(options =>
            {
                options.Using<DateTime>(x => x.Subject.Should().BeCloseTo(x.Expectation))
                .WhenTypeIs<DateTime>();

                return options;
            });
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            TestLocalDbProcess.DestroyDatabase(TestDbConnections.LocalDB.Master, DatabaseName);
        }

    }
}
