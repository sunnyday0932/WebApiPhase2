using Dapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using WebApiPhase2Repository.Conditions;
using WebApiPhase2Repository.Implement;
using WebApiPhase2Repository.Infrastructure;
using WebApiPhase2RepositoryTests.TestUtilites;

namespace WebApiPhase2RepositoryTests
{
    [TestClass]
    public class AccountRepositoryTest
    {
        private static string DatabaseConnectionString => TestHook.DatabaseConnectionString;

        private IDatabaseHelper _DatabaseHelper { get; set; }

        private TestContext _TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DropTable();
            CreateTable();
            PrepareData();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DropTable();
        }

        private static void CreateTable()
        {
            using var conn = new SqlConnection(DatabaseConnectionString);
            conn.Open();
            using var trans = conn.BeginTransaction();
            var filePath = Path.Combine("DbScript", "Create.sql");
            var script = File.ReadAllText(filePath);
            conn.Execute(sql: script, transaction: trans);
            trans.Commit();
        }

        private static void PrepareData()
        {
            using var conn = new SqlConnection(DatabaseConnectionString);
            conn.Open();
            using var trans = conn.BeginTransaction();
            var filePath = Path.Combine("DbScript", "Insert.sql");
            var script = File.ReadAllText(filePath);
            conn.Execute(sql: script, transaction: trans);
            trans.Commit();
        }

        private static void DropTable()
        {
            using var conn = new SqlConnection(DatabaseConnectionString);
            conn.Open();
            var sqlCommand = DatabaseCommands.DropTable("Users");
            conn.Execute(sqlCommand);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this._DatabaseHelper = new DatabaseHelper(DatabaseConnectionString);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            using var conn = new SqlConnection(DatabaseConnectionString);
            conn.Open();
            var sqlCommand = DatabaseCommands.TruncateTable("Users");
            conn.Execute(sqlCommand);
        }

        private AccountRepository GetSystemUnderTest()
        {
            var sut = new AccountRepository(this._DatabaseHelper);
            return sut;
        }

        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "AddAccount")]
        public void AddAccount_傳入model為null_應拋出ArgumentNullException()
        {
            //arrange
            AccountCondition condition = null;
            var sut = this.GetSystemUnderTest();
            //act
            Action actual = () => sut.AddAccount(condition);
            //assert
            actual.Should().Throw<ArgumentNullException>()
                .Which.Message.Contains("model");
        }
    }
}
