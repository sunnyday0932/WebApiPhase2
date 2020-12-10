using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPhase2.ViewModles;

namespace WebApiPhase2.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// 取得單筆帳號資訊
        /// </summary>
        /// <returns></returns>
        [Route("{account}")]
        [HttpGet]
        public AccountViewModel GetAccount(string account)
        {
        }
    }
}