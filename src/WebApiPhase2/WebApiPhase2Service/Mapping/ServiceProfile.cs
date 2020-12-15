using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiPhase2Repository.DataModels;
using WebApiPhase2Service.Dtos;

namespace WebApiPhase2Service.Mapping
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            this.CreateMap<AccountDataModel, AccountDto>()
                .ForMember(x => x.CreateDate, y => y.MapFrom(z => z.CreateDate.ToString("yyyy/MM/dd")))
                .ForMember(x => x.ModifyDate, y => y.MapFrom(z => z.ModifyDate.ToString("yyyy/MM/dd")));
        }
    }
}