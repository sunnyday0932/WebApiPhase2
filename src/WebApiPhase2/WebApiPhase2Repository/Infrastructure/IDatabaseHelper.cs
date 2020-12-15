using System.Data;

namespace WebApiPhase2Repository.Infrastructure
{
    public interface IDatabaseHelper
    {
        /// <summary>
        /// 建立連線
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection();
    }
}