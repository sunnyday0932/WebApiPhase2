using System.Collections.Generic;
using WebApiPhase2Repository.Conditions;
using WebApiPhase2Repository.DataModels;

namespace WebApiPhase2Repository.Interface
{
    public interface IAccountRepository
    {
        /// <summary>
        /// 取得單筆帳號資訊
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public AccountDataModel GetAccount(string account);

        /// <summary>
        /// 取得帳號列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AccountDataModel> GetAccountList();

        /// <summary>
        /// 新增帳號
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool AddAccount(AccountCondition condition);

        /// <summary>
        /// 刪除帳號
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool RemoveAccount(string account);

        /// <summary>
        /// 更新帳號資訊
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool UpdateAccount(AccountCondition condition);

        /// <summary>
        /// 取得密碼
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetAccountPassword(string account);

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool ForgetPassword(AccountCondition condition);
    }
}