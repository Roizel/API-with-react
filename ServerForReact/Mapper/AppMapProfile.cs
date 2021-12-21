using AutoMapper;
using ServerForReact.Data.Entities;
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
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(x => x.Phone));

            CreateMap<AppUser, StudentViewModel>()
                .ForMember(x => x.Photo, opt => opt.MapFrom(x => x.Photo))
                .ForMember(x => x.Phone, opt => opt.MapFrom(x => x.PhoneNumber))
                .ForMember(x=>x.Name, opt => opt.MapFrom(x=>x.UserName));

            CreateMap<CreateCourseViewModel, Courses>()
                .ForMember(x => x.PathImg, opt => opt.MapFrom(x => "/images/" + x.Photo));

            CreateMap<Courses, CourseItemViewModel>()
                .ForMember(x => x.Photo, opt => opt.MapFrom(x => "/images/" + x.PathImg));

            CreateMap<AppUser, EditStudentViewModel>()
                .ForMember(x => x.Photo, opt => opt.MapFrom(x => "/images/" + x.Photo))
                .ForMember(x => x.Phone, opt => opt.MapFrom(x => x.PhoneNumber))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.UserName));

            CreateMap<SaveEditStudentViewModel, AppUser>()
               .ForMember(x => x.Photo, opt => opt.Ignore())
               .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Name))
               .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(x => x.Phone));
            CreateMap<Courses, EditCourseViewModel>()
              .ForMember(x => x.Photo, opt => opt.MapFrom(x => "/images/" + x.PathImg));

            CreateMap<StudentCourses, CourseStudentViewModel>()
              .ForMember(x => x.CourseId, opt => opt.MapFrom(x => x.CourseId))
              .ForMember(x => x.StudentId, opt => opt.MapFrom(x => x.StudentId))
              .ForMember(x => x.JoinCourse, opt => opt.MapFrom(x => x.JoinCourse));

        }
    }
}
