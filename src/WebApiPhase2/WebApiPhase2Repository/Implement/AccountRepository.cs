using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WebApiPhase2Repository.Conditions;
using WebApiPhase2Repository.DataModels;
using WebApiPhase2Repository.Infrastructure;
using WebApiPhase2Repository.Interface;

namespace WebApiPhase2Repository.Implement
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDatabaseHelper _databaseHelper;

        public AccountRepository(IDatabaseHelper databaseHelper)
        {
            this._databaseHelper = databaseHelper;
        }

        /// <summary>
        /// 新增帳號
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool AddAccount(AccountCondition condition)
        {
            var sql = @"INSERT users
                               (account,
                                password,
                                phone,
                                email,
                                createdate,
                                modifydate,
                                modifyuser)
                        VALUES(@Account,
                               @Password,
                               @Phone,
                               @Email,
                               @CreateDate,
                               @ModifyDate,
                               @ModifyUser) ";

            var parameters = new DynamicParameters();
            parameters.Add("@Account", condition.Account, DbType.String);
            parameters.Add("@Password", condition.Password, DbType.String);
            parameters.Add("@Phone", condition.Phone, DbType.String);
            parameters.Add("@Email", condition.Email, DbType.String);
            parameters.Add("@CreateDate", condition.CreateDate, DbType.DateTime);
            parameters.Add("@ModifyDate", condition.ModifyDate, DbType.DateTime);
            parameters.Add("@ModifyUser", condition.ModifyUser, DbType.String);

            using (IDbConnection conn = this._databaseHelper.GetConnection())
            {
                var result = conn.Execute(
                    sql,
                    parameters);

                return result > 0;
            }
        }

        /// <summary>
        /// 取得單筆帳號資訊
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public AccountDataModel GetAccount(string account)
        {
            var sql = @"SELECT [Account]
                              ,[Password]
                              ,[Phone]
                              ,[Email]
                              ,[CreateDate]
                              ,[ModifyDate]
                              ,[ModifyUser]
                          FROM [Northwind].[dbo].[Users]
                          WHERE Account = @Account";

            var parameter = new DynamicParameters();
            parameter.Add("@Account", account, DbType.String);

            using (IDbConnection conn = this._databaseHelper.GetConnection())
            {
                var result = conn.QueryFirstOrDefault<AccountDataModel>(
                    sql,
                    parameter);

                return result;
            }
        }

        /// <summary>
        /// 取得帳號列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AccountDataModel> GetAccountList()
        {
            var sql = @"SELECT [Account]
                              ,[Password]
                              ,[Phone]
                              ,[Email]
                              ,[CreateDate]
                              ,[ModifyDate]
                              ,[ModifyUser]
                          FROM [Northwind].[dbo].[Users]";

            using (IDbConnection conn = this._databaseHelper.GetConnection())
            {
                var result = conn.Query<AccountDataModel>(
                    sql);

                return result;
            }
        }
    }
}