using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiPhase2Repository.Interface;
using WebApiPhase2Service.Dtos;
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
    }
}