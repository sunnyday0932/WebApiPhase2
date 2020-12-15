using System;
using System.Collections.Generic;
using System.Text;
using WebApiPhase2Service.Dtos;
using WebApiPhase2Service.Interface;

namespace WebApiPhase2Service.Implement
{
    public class AccountService : IAccountService
    {
        /// <summary>
        /// 取得單筆帳號資訊
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public AccountDto GetAccount(string account)
        {
            throw new NotImplementedException();
        }
    }
}