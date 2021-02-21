using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Reflection;
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

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "AddAccount")]
        public void AddAccount_輸入信箱格式部正確_應回傳錯誤訊息()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var info = new AccountInfoModel
            {
                Account = "test",
                Email = "test#gmail.com",
                Password = "1234567",
                Phone = "0917888111"
            };

            var expect = new ResultDto
            {
                Success = false,
                Message = "請確認信箱格式！"
            };

            //act
            var actual = sut.AddAccount(info);

            //arrange
            actual.Should().BeEquivalentTo(expect);
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "AddAccount")]
        public void AddAccount_新增成功_應回傳正確訊息()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var info = new AccountInfoModel
            {
                Account = "test",
                Email = "test@gmail.com",
                Password = "1234567",
                Phone = "0917888111"
            };

            this._accountRepository.AddAccount(Arg.Any<AccountCondition>()).Returns(true);

            var expect = new ResultDto
            {
                Success = true,
                Message = "新增成功"
            };

            //act
            var actual = sut.AddAccount(info);

            //arrange
            actual.Should().BeEquivalentTo(expect);
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "AddAccount")]
        public void AddAccount_新增失敗_應回傳錯誤訊息()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var info = new AccountInfoModel
            {
                Account = "test",
                Email = "test@gmail.com",
                Password = "1234567",
                Phone = "0917888111"
            };

            this._accountRepository.AddAccount(Arg.Any<AccountCondition>()).Returns(false);

            var expect = new ResultDto
            {
                Success = false,
                Message = "新增失敗"
            };

            //act
            var actual = sut.AddAccount(info);

            //arrange
            actual.Should().BeEquivalentTo(expect);
        }

        #endregion

        #region Private and Internal Function
        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "ConverPassword")]
        public void ConverPassword_輸入密碼_應回傳加密結果()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var expect = "9GYVaHLoOg+y+V/HHwKtkzBH3y8XWn14h8ifWPYViLc=";

            //act
            var actual = sut.ConverPassword("test2", "12371324" );

            //arrange
            actual.Should().Be(expect);
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "CheckMailFormate")]
        public void CheckMailFormate_輸入錯誤格式Mail_應回傳False()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var method = sut.GetType().GetMethod("CheckMailFormate", BindingFlags.Instance | BindingFlags.NonPublic);

            //act
            var actual = method.Invoke(sut, new[] { "fff#gmail.com"});

            //arrange
            actual.Should().Be(false);
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "CheckMailFormate")]
        public void CheckMailFormate_輸入正確格式Mail_應回傳True()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var method = sut.GetType().GetMethod("CheckMailFormate", BindingFlags.Instance | BindingFlags.NonPublic);

            //act
            var actual = method.Invoke(sut, new[] { "fff@gmail.com" });

            //arrange
            actual.Should().Be(true);
        }
        #endregion
    }
}
