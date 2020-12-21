using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using WebApiPhase2Repository.Conditions;
using WebApiPhase2Repository.Interface;
using WebApiPhase2Service.Dtos;
using WebApiPhase2Service.InfoModels;
using WebApiPhase2Service.Interface;

namespace WebApiPhase2Service.Implement
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;
        private IMapper _mapper;

        public AccountService(
            IAccountRepository accountRepository,
            IMapper mapper)
        {
            this._accountRepository = accountRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// 新增帳號
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ResultDto AddAccount(AccountInfoModel info)
        {
            //a、所有欄位皆須輸入。
            if (string.IsNullOrWhiteSpace(info.Account) ||
                string.IsNullOrWhiteSpace(info.Email) ||
                string.IsNullOrWhiteSpace(info.Password) ||
                string.IsNullOrWhiteSpace(info.Phone))
            {
                throw new Exception("請檢查輸入欄位，缺一不可！");
            }

            var condition = this._mapper.Map<AccountCondition>(info);

            //b、使用者帳號不可重複。
            var checkAccount = this._accountRepository.GetAccount(condition.Account);
            if (checkAccount != null)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "該使用者帳號已存在，請確認！"
                };
            }

            //c、密碼長度不可低於6碼。
            if (condition.Password.Length < 6)
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "使用者密碼長度不可低於6碼！"
                };
            }

            //d、密碼需透過加密處理存入資料庫。
            condition.Password = ConverPassword(condition.Account, condition.Password);

            //e、檢查信箱是否合法。
            var checkMail = CheckMailFormate(condition.Email);
            if (checkMail.Equals(false))
            {
                return new ResultDto
                {
                    Success = false,
                    Message = "請確認信箱格式！"
                };
            }

            //補上其他欄位
            condition.CreateDate = DateTime.Now;
            condition.ModifyDate = DateTime.Now;
            condition.ModifyUser = "System";

            var result = this._accountRepository.AddAccount(condition);

            return new ResultDto
            {
                Success = result,
                Message = result ? "新增成功" : "新增失敗"
            };
        }

        /// <summary>
        /// 取得單筆帳號資訊
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public AccountDto GetAccount(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
            {
                throw new Exception("Account 不可為空 !");
            }

            var data = this._accountRepository.GetAccount(account);
            var result = this._mapper.Map<AccountDto>(data);
            result.Phone = ConvertPhoneNumber(result.Phone);
            return result;
        }

        /// <summary>
        /// 取得帳號列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AccountDto> GetAccountList()
        {
            var data = this._accountRepository.GetAccountList();
            var result = data.Select(x => new AccountDto
            {
                Phone = ConvertPhoneNumber(x.Phone),
                Account = x.Account,
                CreateDate = x.CreateDate.ToString("yyyy/MM/dd"),
                Email = x.Email,
                ModifyDate = x.ModifyDate.ToString("yyyy/MM/dd"),
                ModifyUser = x.ModifyUser
            });

            return result;
        }

        /// <summary>
        /// 轉換電話號碼
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        private string ConvertPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return phoneNumber;
            }
            else
            {
                return (phoneNumber.Substring(0, 6) + new string('*', phoneNumber.Length - 6));
            }
        }

        /// <summary>
        /// 密碼加密
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string ConverPassword(string account, string password)
        {
            var sha256 = new SHA256CryptoServiceProvider();
            var source = Encoding.Default.GetBytes(account + password);
            var crypto = sha256.ComputeHash(source);
            var result = Convert.ToBase64String(crypto);

            return result;
        }

        /// <summary>
        /// 確認信箱格式
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool CheckMailFormate(string email)
        {
            try
            {
                var mail = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}