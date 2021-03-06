using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            TestLocalDbProcess.CreateDatabase(TestDbConnenction.LocalDb.Default, DatabaseName.SampleDB);

            AssertionOptions.AssertEquivalencyUsing(options =>
            {
                options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation))
                       .WhenTypeIs<DateTime>();

                return options;
            });
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            TestLocalDbProcess.DestroyDatabase(TestDbConnenction.LocalDb.Default, DatabaseName.SampleDB);
        }
    }
}
