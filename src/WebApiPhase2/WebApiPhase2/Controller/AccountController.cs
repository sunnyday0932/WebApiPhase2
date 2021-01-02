using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPhase2.Parameters;
using WebApiPhase2.ViewModles;
using WebApiPhase2Service.InfoModels;
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

        /// <summary>
        /// 新增帳號
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public ResultViewModel AddAccount([FromBody]AccountParameter parameter)
        {
            var info = this._mapper.Map<AccountInfoModel>(parameter);
            var data = this._accountService.AddAccount(info);
            var result = this._mapper.Map<ResultViewModel>(data);

            return result;
        }

        /// <summary>
        /// 刪除帳號
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Route("")]
        [HttpDelete]
        public ResultViewModel RemoveAccount([FromBody]AccountParameter parameter)
        {
            var info = this._mapper.Map<AccountInfoModel>(parameter);
            var data = this._accountService.RemoveAccount(info);
            var result = this._mapper.Map<ResultViewModel>(data);

            return result;
        }

        /// <summary>
        /// 更新帳號資訊
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPatch]
        public ResultViewModel UpdateAccount([FromBody]AccountParameter parameter)
        {
            var info = this._mapper.Map<AccountInfoModel>(parameter);
            var data = this._accountService.UpdateAccount(info);
            var result = this._mapper.Map<ResultViewModel>(data);

            return result;
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Route("forget")]
        [HttpPatch]
        public ResultViewModel ForgetPassword([FromBody]AccountParameter parameter)
        {
            var info = this._mapper.Map<AccountInfoModel>(parameter);
            var data = this._accountService.ForgetPassword(info);
            var result = this._mapper.Map<ResultViewModel>(data);

            return result;
        }
    }
}