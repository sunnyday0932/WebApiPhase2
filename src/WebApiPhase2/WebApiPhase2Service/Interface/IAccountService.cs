using System;
using System.Collections.Generic;
using System.Text;
using WebApiPhase2Service.Dtos;
using WebApiPhase2Service.InfoModels;

namespace WebApiPhase2Service.Interface
{
    public interface IAccountService
    {
        /// <summary>
        /// 取得單筆帳號資訊
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public AccountDto GetAccount(string account);

        /// <summary>
        /// 取得帳號列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AccountDto> GetAccountList();

        /// <summary>
        /// 新增帳號
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ResultDto AddAccount(AccountInfoModel info);
    }
}