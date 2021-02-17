using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiPhase2Repository.Conditions;
using WebApiPhase2Repository.DataModels;
using WebApiPhase2Repository.Interface;
using WebApiPhase2Service.Dtos;
using WebApiPhase2Service.Implement;
using WebApiPhase2Service.InfoModels;
using WebApiPhase2Service.Mapping;

namespace WebApiPhase2Tests.Service
{
    [TestClass]
    public class AccountServiceTest
    {
        private IAccountRepository _accountRepository;
        private IMapper _mapper
        {
            get
            {
                var config = new MapperConfiguration(options => { options.AddProfile<ServiceProfile>(); });
                return config.CreateMapper();
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this._accountRepository = Substitute.For<IAccountRepository>();
        }

        private AccountService GetSystemUnderTest()
        {
            var sut = new AccountService(this._accountRepository,this._mapper);
            return sut;
        }

        #region AddAccount
        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "AddAccount")]
        public void AddAccount_Account為空_應回傳Exception()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var info = new AccountInfoModel
            {
                Account = string.Empty,
                Email = "test@gmail.com",
                Password = "123456",
                Phone = "0917888444"
            };
            //act
            Action actual = () => sut.AddAccount(info);

            //arrange
            actual.Should().Throw<Exception>()
                .Which.Message.Contains("請檢查輸入欄位，缺一不可！");
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "AddAccount")]
        public void AddAccount_Email為空_應回傳Exception()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var info = new AccountInfoModel
            {
                Account = "test",
                Email = string.Empty,
                Password = "123456",
                Phone = "0917888444"
            };
            //act
            Action actual = () => sut.AddAccount(info);

            //arrange
            actual.Should().Throw<Exception>()
                .Which.Message.Contains("請檢查輸入欄位，缺一不可！");
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "AddAccount")]
        public void AddAccount_Password為空_應回傳Exception()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var info = new AccountInfoModel
            {
                Account = "test",
                Email = "test@gmail.com",
                Password = string.Empty,
                Phone = "0917888444"
            };
            //act
            Action actual = () => sut.AddAccount(info);

            //arrange
            actual.Should().Throw<Exception>()
                .Which.Message.Contains("請檢查輸入欄位，缺一不可！");
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "AddAccount")]
        public void AddAccount_Phone為空_應回傳Exception()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var info = new AccountInfoModel
            {
                Account = "test",
                Email = "test@gmail.com",
                Password = "123456",
                Phone = string.Empty
            };
            //act
            Action actual = () => sut.AddAccount(info);

            //arrange
            actual.Should().Throw<Exception>()
                .Which.Message.Contains("請檢查輸入欄位，缺一不可！");
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "AddAccount")]
        public void AddAccount_使用者帳號重複_應回傳錯誤訊息()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var info = new AccountInfoModel
            {
                Account = "test",
                Email = "test@gmail.com",
                Password = "123456",
                Phone = "0917888111"
            };

            var data = new AccountDataModel
            {
                Account = "test",
                Email = "test@gmail.com",
                Phone = "0917888111"
            };

            var expect = new ResultDto
            {
                Success = false,
                Message = "該使用者帳號已存在，請確認！"
            };

            this._accountRepository.GetAccount(Arg.Any<string>()).Returns(data);

            //act
            var actual = sut.AddAccount(info);

            //arrange
            actual.Should().BeEquivalentTo(expect);
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "AddAccount")]
        public void AddAccount_密碼長度低於6碼_應回傳錯誤訊息()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var info = new AccountInfoModel
            {
                Account = "test",
                Email = "test@gmail.com",
                Password = "1234",
                Phone = "0917888111"
            };

            var expect = new ResultDto
            {
                Success = false,
                Message = "使用者密碼長度不可低於6碼！"
            };

            //act
            var actual = sut.AddAccount(info);

            //arrange
            actual.Should().BeEquivalentTo(expect);
        }

        #endregion
    }
}
