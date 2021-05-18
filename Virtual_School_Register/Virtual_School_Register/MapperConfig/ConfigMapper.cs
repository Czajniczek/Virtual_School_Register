using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virtual_School_Register.Models;
using Virtual_School_Register.ViewModels;

namespace Virtual_School_Register.MapperConfig
{
    public class ConfigMapper : Profile
    {
        public ConfigMapper()
        {
            CreateMap<User, EditUserViewModel>().ReverseMap();
            //CreateMap<User, EditUserViewModel>()
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(x => $"{x.Name} 123")).ReverseMap();

            CreateMap<User, CreateUserViewModel>().ReverseMap();
        }
    }
}
