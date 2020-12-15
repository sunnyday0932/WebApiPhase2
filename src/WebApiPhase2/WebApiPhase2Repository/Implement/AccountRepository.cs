using System;
using System.Collections.Generic;
using System.Text;
using WebApiPhase2Repository.DataModels;
using WebApiPhase2Repository.Interface;

namespace WebApiPhase2Repository.Implement
{
    public class AccountRepository : IAccountRepository
    {
        /// <summary>
        /// 取得單筆帳號資訊
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public AccountDataModel GetAccount(string account)
        {
            throw new NotImplementedException();
        }
    }
}