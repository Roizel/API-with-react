using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServerForReact.Data;
using ServerForReact.Data.Entities;
using ServerForReact.Models;
using ServerForReact.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Pagination
{
    public class CoursePagination
    {
        private readonly AppEFContext context;
        private readonly IMapper mapper;
        public CoursePagination(AppEFContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public DbSet<Courses> All()
        {
            var list = context.Courses;
            return list;
        }
        public Task<CoursePaginationResultViewModel> Query(CoursePaginationViewModel search)
        {
            int page = search.Page;
            int pageSize = 8;
            var query = context.Courses
                .AsQueryable();

            if (!string.IsNullOrEmpty(search.Id))
            {
                int id = int.Parse(search.Id);
                query = query.Where(x => x.Id == id);
            }

            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(search.Description))
            {
                query = query.Where(x => x.Description.ToLower().Contains(search.Description.ToLower()));
            }

            if (!string.IsNullOrEmpty(search.Duration))
            {
                query = query.Where(x => x.Duration.ToLower().Contains(search.Duration.ToLower()));
            }

            //if (!string.IsNullOrEmpty(search.StartCourse))
            //{
            //    query = query.Where(x => x.StartCourse.ToString().Contains(search.StartCourse));
            //}

            var model = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => mapper.Map<CourseItemViewModel>(x))
                .ToList();
            int total = query.Count();
            int pages = (int)Math.Ceiling(total / (double)pageSize);

            CoursePaginationResultViewModel res = new CoursePaginationResultViewModel
            {
                Courses = model,
                Total = total,
                CurrentPage = page,
                Pages = pages
            };

            return Task.FromResult(res);
        }

    }
}
