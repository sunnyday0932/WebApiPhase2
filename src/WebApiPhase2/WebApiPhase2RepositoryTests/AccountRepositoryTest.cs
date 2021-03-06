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
using System.Linq;
using System.Data;
using WebApiPhase2Repository.DataModels;

namespace WebApiPhase2RepositoryTests
{
    [TestClass]
    public class AccountRepositoryTest
    {
        private static readonly string ConnectionString = TestHook.SampleDbConnection;
        private IDatabaseHelper _DatabaseHelper { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TableCommands.DropTable(ConnectionString, "Users");

            var createScript = File.ReadAllText(@"DbScripts\Create.sql");
            TableCommands.CreateTable(ConnectionString, createScript);

            var insertScript = File.ReadAllText(@"DbScripts\Insert.sql");
            TableCommands.Execute(ConnectionString,insertScript);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TableCommands.DropTable(ConnectionString,"Users");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this._DatabaseHelper = new DatabaseHelper(ConnectionString);
        }

        private AccountRepository GetSystemUnderTest()
        {
            var sut = new AccountRepository(this._DatabaseHelper);
            return sut;
        }

        #region AddAccout
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

        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "AddAccount")]
        public void AddAccount_傳入model_建立已存在的資料_應回傳SqlException()
        {
            //arrange
            var fixture = new Fixture();
            var condition = fixture.Build<AccountCondition>()
                .With(x => x.Phone, "09111111")
                .With(x => x.Email, "test@yahoo.com")
                .With(x => x.Account, "test2")
                .With(x => x.Password, "231543214")
                .With(x => x.ModifyUser, "123423")
                .Create();
            var sut = this.GetSystemUnderTest();

            //act
            Action actual = () => sut.AddAccount(condition);

            //assert
            actual.Should().Throw<SqlException>();
        }

        #endregion
        #region ForgetPassword
        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "ForgetPassword")]
        public void ForgetPassword_傳入model_更新成功_應回傳True()
        {
            //arrange
            var fixture = new Fixture();
            var condition = fixture.Build<AccountCondition>()
                .With(x => x.Account, "test2")
                .With(x => x.Password, "231543214")
                .Create();

            var sut = this.GetSystemUnderTest();

            //act
            var actual = sut.ForgetPassword(condition);

            //assert
            actual.Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "ForgetPassword")]
        public void ForgetPassword_傳入model_更新不存在的帳號_應回傳False()
        {
            //arrange
            var fixture = new Fixture();
            var condition = fixture.Build<AccountCondition>()
                .With(x => x.Account, "test5")
                .With(x => x.Password, "231543214")
                .Create();

            var sut = this.GetSystemUnderTest();

            //act
            var actual = sut.ForgetPassword(condition);

            //assert
            actual.Should().BeFalse();
        }

        #endregion

        #region GetAccount
        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "GetAccount")]
        public void GetAccount_輸入存在的帳號_回傳的model不為空()
        {
            //arrange
            var sut = this.GetSystemUnderTest();

            //act
            var actual = sut.GetAccount("test2");

            //assert
            actual.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "GetAccount")]
        public void GetAccount_輸入存在的帳號_應回傳null()
        {
            //arrange
            var sut = this.GetSystemUnderTest();

            //act
            var actual = sut.GetAccount("test5");

            //assert
            actual.Should().BeNull();
        }
        #endregion

        #region GetAccountList
        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "GetAccountList")]
        public void GetAccountList_回傳的list應為2()
        {
            //arrange
            var sut = this.GetSystemUnderTest();

            //act
            var actual = sut.GetAccountList();

            //assert
            actual.Should().NotBeNull();
            actual.Count().Should().Be(2);
        }
        #endregion

        #region GetAccountPassword
        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "GetAccountPassword")]
        public void GetAccountPassword_輸入存在的帳號_應回傳password()
        {
            //arrange
            var sut = this.GetSystemUnderTest();

            //act
            var actual = sut.GetAccountPassword("test2");

            //assert
            actual.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "GetAccountPassword")]
        public void GetAccountPassword_輸入不存在的帳號_應回傳null()
        {
            //arrange
            var sut = this.GetSystemUnderTest();

            //act
            var actual = sut.GetAccountPassword("test6666");

            //assert
            actual.Should().BeNull();
        }
        #endregion

        #region RemoveAccount
        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "RemoveAccount")]
        public void RemoveAccount_輸入存在的帳號_刪除成功_應回傳True()
        {
            //arrange
            var sut = this.GetSystemUnderTest();

            //act
            var actual = sut.RemoveAccount("test2");

            //assert
            actual.Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "RemoveAccount")]
        public void RemoveAccount_輸入不存在的帳號_異動0筆_應回傳False()
        {
            //arrange
            var sut = this.GetSystemUnderTest();

            //act
            var actual = sut.RemoveAccount("test25555");

            //assert
            actual.Should().BeFalse();
        }
        #endregion

        #region UpdateAccount
        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "UpdateAccount")]
        public void UpdateAccount_更新成功_應回傳True()
        {
            //arrange
            var fixture = new Fixture();
            var condiont = fixture.Build<AccountCondition>()
                .With(x => x.Account, "test2")
                .With(x => x.Phone, "09111111")
                .With(x => x.Email, "test@yahoo.com")
                .With(x => x.Password, "231543214")
                .With(x => x.ModifyUser, "123423")
                .Create();

            var sut = this.GetSystemUnderTest();

            //act
            var actual = sut.UpdateAccount(condiont);

            //assert
            actual.Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("AccountRepository")]
        [TestProperty("AccountRepository", "UpdateAccount")]
        public void UpdateAccount_輸入不存在的帳號_異動0筆_應回傳False()
        {
            //arrange
            var fixture = new Fixture();
            var condiont = fixture.Build<AccountCondition>()
                .With(x => x.Account, "test253")
                .With(x => x.Phone, "09111111")
                .With(x => x.Email, "test@yahoo.com")
                .With(x => x.Password, "231543214")
                .With(x => x.ModifyUser, "123423")
                .Create();

            var sut = this.GetSystemUnderTest();

            //act
            var actual = sut.UpdateAccount(condiont);

            //assert
            actual.Should().BeFalse();
        }
        #endregion
    }
}
