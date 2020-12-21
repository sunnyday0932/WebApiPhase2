using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPhase2.Parameters;
using WebApiPhase2.ViewModles;
using WebApiPhase2Service.Dtos;
using WebApiPhase2Service.InfoModels;

namespace WebApiPhase2.Mapping
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            this.CreateMap<AccountDto, AccountViewModel>();
            this.CreateMap<AccountParameter, AccountInfoModel>();
            this.CreateMap<ResultDto, ResultViewModel>();
        }
    }
}