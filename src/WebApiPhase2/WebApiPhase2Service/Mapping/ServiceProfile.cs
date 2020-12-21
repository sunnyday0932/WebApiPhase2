using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiPhase2Repository.Conditions;
using WebApiPhase2Repository.DataModels;
using WebApiPhase2Service.Dtos;
using WebApiPhase2Service.InfoModels;

namespace WebApiPhase2Service.Mapping
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            this.CreateMap<AccountDataModel, AccountDto>()
                .ForMember(x => x.CreateDate, y => y.MapFrom(z => z.CreateDate.ToString("yyyy/MM/dd")))
                .ForMember(x => x.ModifyDate, y => y.MapFrom(z => z.ModifyDate.ToString("yyyy/MM/dd")));

            this.CreateMap<AccountInfoModel, AccountCondition>();
        }
    }
}