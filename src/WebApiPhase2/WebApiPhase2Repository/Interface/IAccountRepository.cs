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
    }
}