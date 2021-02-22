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

        #region GetAccount
        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "GetAccount")]
        public void GetAccount_Account為空_應回傳Exception()
        {
            //arrange
            var sut = this.GetSystemUnderTest();

            //act
            Action actual = () => sut.GetAccount("");

            //assert
            actual.Should().Throw<Exception>()
                .Which.Message.Contains("Account 不可為空 !");
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "GetAccount")]
        public void GetAccount_Account有資料_應回傳正確資訊()
        {
            //arrange
            var data = new AccountDataModel()
            { 
                Account = "test123",
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                Email = "test123@gmail.com",
                ModifyUser = "sys",
                Phone = "0918777888"
            };
            var sut = this.GetSystemUnderTest();
            this._accountRepository.GetAccount("test123").Returns(data);

            var expect = new AccountDto()
            {
                Account = "test123",
                CreateDate = DateTime.Now.ToString("yyyy/MM/dd"),
                ModifyDate = DateTime.Now.ToString("yyyy/MM/dd"),
                Email = "test123@gmail.com",
                ModifyUser = "sys",
                Phone = "091877****"
            };
            //act
            var actual = sut.GetAccount("test123");

            //assert
            actual.Should().BeEquivalentTo(expect);
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "GetAccount")]
        public void GetAccount_Account無資料_應回傳空值()
        {
            //arrange
            var sut = this.GetSystemUnderTest();
            this._accountRepository.GetAccount("test123").ReturnsForAnyArgs(x => null);

            //act
            var actual = sut.GetAccount("test123");

            //assert
            actual.Should().BeNull();
        }

        #endregion GetAccountList

        #region GetAccountList
        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "GetAccountList")]
        public void GetAccountList_無資料_應回傳空值()
        {
            //arrange
            var sut = this.GetSystemUnderTest();
            this._accountRepository.GetAccountList().ReturnsForAnyArgs(x => null);

            //act
            var actual = sut.GetAccountList();

            //assert
            actual.Should().BeNull();
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "GetAccountList")]
        public void GetAccountList_有資料_應回傳正確結果()
        {
            //arrange
            var data = new List<AccountDataModel>
            {
                new AccountDataModel
                {
                    Account = "test123",
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    Email = "test123@gmail.com",
                    ModifyUser = "sys",
                    Phone = "0918777888"
                }
            };

            var sut = this.GetSystemUnderTest();
            this._accountRepository.GetAccountList().Returns(data);

            var expect = new List<AccountDto>
            {
                new AccountDto
                {
                    Account = "test123",
                    CreateDate = DateTime.Now.ToString("yyyy/MM/dd"),
                    ModifyDate = DateTime.Now.ToString("yyyy/MM/dd"),
                    Email = "test123@gmail.com",
                    ModifyUser = "sys",
                    Phone = "091877****"
                }
            };
            //act
            var actual = sut.GetAccountList();

            //assert
            actual.Should().BeEquivalentTo(expect);
        }
        #endregion

        #region RemoveAccount
        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "RemoveAccount")]
        public void RemoveAccount_傳入的Account為空_應回傳Exception()
        {
            //arrange
            var data = new AccountInfoModel()
            { 
                Account = "",
                Email = "test22@gmail.com",
                Password = "9GYVaHLoOg+y+V/HHwKtkzBH3y8XWn14h8ifWPYViLc=",
                Phone = "0988123456"
            };

            var sut = this.GetSystemUnderTest();

            //act
            Action actual = () => sut.RemoveAccount(data);

            //assert
            actual.Should().Throw<Exception>()
                .Which.Message.Contains("請檢查輸入欄位，缺一不可！");
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "RemoveAccount")]
        public void RemoveAccount_傳入的Email為空_應回傳Exception()
        {
            //arrange
            var data = new AccountInfoModel()
            {
                Account = "test2",
                Email = "",
                Password = "9GYVaHLoOg+y+V/HHwKtkzBH3y8XWn14h8ifWPYViLc=",
                Phone = "0988123456"
            };

            var sut = this.GetSystemUnderTest();

            //act
            Action actual = () => sut.RemoveAccount(data);

            //assert
            actual.Should().Throw<Exception>()
                .Which.Message.Contains("請檢查輸入欄位，缺一不可！");
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "RemoveAccount")]
        public void RemoveAccount_傳入的Phone為空_應回傳Exception()
        {
            //arrange
            var data = new AccountInfoModel()
            {
                Account = "test2",
                Email = "test22@gmail.com",
                Password = "9GYVaHLoOg+y+V/HHwKtkzBH3y8XWn14h8ifWPYViLc=",
                Phone = ""
            };

            var sut = this.GetSystemUnderTest();

            //act
            Action actual = () => sut.RemoveAccount(data);

            //assert
            actual.Should().Throw<Exception>()
                .Which.Message.Contains("請檢查輸入欄位，缺一不可！");
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "RemoveAccount")]
        public void RemoveAccount_查無帳號_應回傳錯誤訊息()
        {
            //arrange
            var data = new AccountInfoModel()
            {
                Account = "test111122",
                Email = "test22@gmail.com",
                Password = "9GYVaHLoOg+y+V/HHwKtkzBH3y8XWn14h8ifWPYViLc=",
                Phone = "0988123456"
            };

            var expect = new ResultDto
            {
                Success = false,
                Message = "請確認要刪除的帳號！"
            };

            var sut = this.GetSystemUnderTest();
            this._accountRepository.GetAccount("test111122").ReturnsForAnyArgs(x => null);

            //act
            var actual = sut.RemoveAccount(data);

            //assert
            actual.Should().BeEquivalentTo(expect);
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "RemoveAccount")]
        public void RemoveAccount_輸入Email與資料不一致_應回傳錯誤訊息()
        {
            //arrange
            var data = new AccountInfoModel()
            {
                Account = "test111122",
                Email = "test22@gmail.com",
                Password = "9GYVaHLoOg+y+V/HHwKtkzBH3y8XWn14h8ifWPYViLc=",
                Phone = "0988123456"
            };

            var checkInfo = new AccountDataModel
            {
                Account = "test111122",
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                Email = "test123@gmail.com",
                ModifyUser = "sys",
                Phone = "0988123456"
            };

            var expect = new ResultDto
            {
                Success = false,
                Message = "請確認輸入的EMail是否與註冊時一致！"
            };

            var sut = this.GetSystemUnderTest();
            this._accountRepository.GetAccount("test111122").Returns(checkInfo);

            //act
            var actual = sut.RemoveAccount(data);

            //assert
            actual.Should().BeEquivalentTo(expect);
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "RemoveAccount")]
        public void RemoveAccount_輸入電話與資料不一致_應回傳錯誤訊息()
        {
            //arrange
            var data = new AccountInfoModel()
            {
                Account = "test111122",
                Email = "test123@gmail.com",
                Password = "9GYVaHLoOg+y+V/HHwKtkzBH3y8XWn14h8ifWPYViLc=",
                Phone = "0988123412"
            };

            var checkInfo = new AccountDataModel
            {
                Account = "test111122",
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                Email = "test123@gmail.com",
                ModifyUser = "sys",
                Phone = "0988123456"
            };

            var expect = new ResultDto
            {
                Success = false,
                Message = "請確認輸入的電話是否與註冊時一致！"
            };

            var sut = this.GetSystemUnderTest();
            this._accountRepository.GetAccount("test111122").Returns(checkInfo);

            //act
            var actual = sut.RemoveAccount(data);

            //assert
            actual.Should().BeEquivalentTo(expect);
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "RemoveAccount")]
        public void RemoveAccount_刪除失敗_應回傳錯誤訊息()
        {
            //arrange
            var data = new AccountInfoModel()
            {
                Account = "test111122",
                Email = "test123@gmail.com",
                Password = "9GYVaHLoOg+y+V/HHwKtkzBH3y8XWn14h8ifWPYViLc=",
                Phone = "0988123456"
            };

            var checkInfo = new AccountDataModel
            {
                Account = "test111122",
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                Email = "test123@gmail.com",
                ModifyUser = "sys",
                Phone = "0988123456"
            };

            var expect = new ResultDto
            {
                Success = false,
                Message = "刪除失敗"
            };

            var sut = this.GetSystemUnderTest();
            this._accountRepository.GetAccount("test111122").Returns(checkInfo);
            this._accountRepository.RemoveAccount("test111122").Returns(false);

            //act
            var actual = sut.RemoveAccount(data);

            //assert
            actual.Should().BeEquivalentTo(expect);
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "RemoveAccount")]
        public void RemoveAccount_刪除成功_應回傳正確訊息()
        {
            //arrange
            var data = new AccountInfoModel()
            {
                Account = "test111122",
                Email = "test123@gmail.com",
                Password = "9GYVaHLoOg+y+V/HHwKtkzBH3y8XWn14h8ifWPYViLc=",
                Phone = "0988123456"
            };

            var checkInfo = new AccountDataModel
            {
                Account = "test111122",
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                Email = "test123@gmail.com",
                ModifyUser = "sys",
                Phone = "0988123456"
            };

            var expect = new ResultDto
            {
                Success = true,
                Message = "刪除成功"
            };

            var sut = this.GetSystemUnderTest();
            this._accountRepository.GetAccount("test111122").Returns(checkInfo);
            this._accountRepository.RemoveAccount("test111122").Returns(true);

            //act
            var actual = sut.RemoveAccount(data);

            //assert
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

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "ConvertPhoneNumber")]
        public void ConvertPhoneNumber_輸入空字串_應直接回傳空字串()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var method = sut.GetType().GetMethod("ConvertPhoneNumber", BindingFlags.Instance | BindingFlags.NonPublic);

            //act
            var actual = method.Invoke(sut, new[] { string.Empty });

            //arrange
            actual.Should().Be(string.Empty);
        }

        [TestMethod]
        [Owner("Sian")]
        [TestCategory("AccountServiceTest")]
        [TestProperty("AccountServiceTest", "ConvertPhoneNumber")]
        public void ConvertPhoneNumber_輸入電話號碼_應回傳轉換後結果()
        {
            //assert
            var sut = this.GetSystemUnderTest();
            var method = sut.GetType().GetMethod("ConvertPhoneNumber", BindingFlags.Instance | BindingFlags.NonPublic);
            var expect = "091877****";

            //act
            var actual = method.Invoke(sut, new[] { "0918777888" });

            //arrange
            actual.Should().Be(expect);
        }
        #endregion
    }
}
