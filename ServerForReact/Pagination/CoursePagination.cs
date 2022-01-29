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
        public Task<CoursePaginationResultViewModel> All()
        {
            int page = 1;
            int pageSize = 10;
            var query = context.Courses
                .AsQueryable();

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
                CurrentPage = 1,
                Pages = pages
            };
            return Task.FromResult(res);
        }

        public Task<CoursePaginationResultViewModel> CoursesSorting(CoursePaginationViewModel search)
        {
            int page = search.Page;
            int pageSize = 10;
            var query = context.Courses
                .AsQueryable();

            if (search.SearchWord != null)
            {
                query = query.Where(
                    x => x.Name.ToLower().Contains(search.SearchWord.ToLower())
                    || x.Description.ToLower().Contains(search.SearchWord.ToLower())
                    || x.Duration.ToLower().Contains(search.SearchWord.ToLower()));
            }

            if (search.TypeOfSort == "ascend")
            {
                if (search.Sort == "name")
                {
                    query = query.OrderBy(x => x.Name);
                }
                if (search.Sort == "id")
                {
                    query = query.OrderBy(x => x.Id);
                }
                if (search.Sort == "duration")
                {
                    query = query.OrderBy(x => x.Duration);
                }
            }
            if (search.TypeOfSort == "descend")
            {
                if (search.Sort == "name")
                {
                    query = query.OrderByDescending(x => x.Name);
                }
                if (search.Sort == "id")
                {
                    query = query.OrderByDescending(x => x.Id);
                }
                if (search.Sort == "duration")
                {
                    query = query.OrderByDescending(x => x.Duration);
                }
            }

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
                CurrentPage = 1,
                Pages = pages
            };

            return Task.FromResult(res);

        }
    }
}
