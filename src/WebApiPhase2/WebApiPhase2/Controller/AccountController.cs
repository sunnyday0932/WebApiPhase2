using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPhase2.ViewModles;
using WebApiPhase2Service.Interface;

namespace WebApiPhase2.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        private IMapper _mapper;

        public AccountController(
            IAccountService accountService,
            IMapper mapper)
        {
            this._accountService = accountService;
            this._mapper = mapper;
        }

        /// <summary>
        /// 取得單筆帳號資訊
        /// </summary>
        /// <returns></returns>
        [Route("{account}")]
        [HttpGet]
        public AccountViewModel GetAccount(string account)
        {
            var data = this._accountService.GetAccount(account);
            var result = this._mapper.Map<AccountViewModel>(data);

            return result;
        }

        /// <summary>
        /// 取得帳號列表
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IEnumerable<AccountViewModel> GetAccountList()
        {
            var data = this._accountService.GetAccountList();
            var result = this._mapper.Map<IEnumerable<AccountViewModel>>(data);

            return result;
        }
    }
}