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
                .ForMember(x => x.CreateDate, y => y.MapFrom(z => z.CreateDate.HasValue ? z.CreateDate.Value.ToString("yyyy/MM/dd") : null))
                .ForMember(x => x.ModifyDate, y => y.MapFrom(z => z.ModifyDate.HasValue ? z.ModifyDate.Value.ToString("yyyy/MM/dd") : null));

            this.CreateMap<AccountInfoModel, AccountCondition>();
        }
    }
}