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
            CreateMap<User, UserEditViewModel>().ReverseMap();
            //CreateMap<User, EditUserViewModel>()
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(x => $"{x.Name} 123")).ReverseMap();

            CreateMap<User, UserCreateViewModel>().ReverseMap();

            CreateMap<User, UserDetailsViewModel>().ReverseMap();

            CreateMap<Lesson, LessonViewModel>().ReverseMap();

            CreateMap<Test, TestViewModel>().ReverseMap();

            CreateMap<Question, QuestionViewModel>().ReverseMap();
        }
    }
}
