﻿using System;
using System.Collections.Generic;
using System.Text;
using WebApiPhase2Service.Dtos;

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
    }
}