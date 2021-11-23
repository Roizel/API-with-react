using AutoMapper;
using ServerForReact.Data.Identity;
using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Mapper
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<RegisterViewModel, AppUser>()
              .ForMember(x => x.Photo, opt => opt.Ignore())
              .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Name));
            CreateMap<AppUser, StudentViewModel>()
           .ForMember(x => x.Photo, opt => opt.MapFrom(x => "/images/" + x.Photo));
        }
    }
}
