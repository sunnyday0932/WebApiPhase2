using AutoFixture;
using Dapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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
    [DeploymentItem(@"DbScripts\Create.sql")]
    [DeploymentItem(@"DbScripts\Insert.sql")]
    public class AccountRepositoryTest
    {
        private IDatabaseHelper _DatabaseHelper { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            CreateTable();
            PrepareData();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            using var conn = new SqlConnection(TestHook.SampleDbConnection);
            conn.Open();
            var sqlCommand = TableCommands.DropTable("Users");
            conn.Execute(sqlCommand);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this._DatabaseHelper = Substitute.For<IDatabaseHelper>();

            this._DatabaseHelper
                .GetConnection()
                .Returns(new SqlConnection(TestHook.SampleDbConnection));
        }

        private static void CreateTable()
        {
            using var conn = new SqlConnection(TestHook.SampleDbConnection);
            conn.Open();
            using var trans = conn.BeginTransaction();
            var script = File.ReadAllText(@"Create.sql");
            conn.Execute(sql: script, transaction: trans);
            trans.Commit();
        }

        private static void PrepareData()
        {
            using var conn = new SqlConnection(TestHook.SampleDbConnection);
            conn.Open();
            using var trans = conn.BeginTransaction();
            var script = File.ReadAllText(@"Insert.sql");
            conn.Execute(sql: script, transaction: trans);
            trans.Commit();
        }


        private AccountRepository GetSystemUnderTest()
        {
            var sut = new AccountRepository(this._DatabaseHelper);
            return sut;
        }

        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "AddAccount")]
        public void AddAccount_傳入model_建立成功_應回傳True()
        {
            //arrange
            var fixture = new Fixture();
            var condition = fixture.Build<AccountCondition>()
                .With(x => x.Phone, "09111111")
                .With(x => x.Email, "test@yahoo.com")
                .With(x => x.Account, "test2444")
                .With(x => x.Password, "231543214")
                .With(x => x.ModifyUser, "123423")
                .Create();
            var sut = this.GetSystemUnderTest();

            //act
            var actual = sut.AddAccount(condition);
            //assert
            actual.Should().BeTrue();
        }
    }
}
