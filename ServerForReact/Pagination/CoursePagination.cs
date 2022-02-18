using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServerForReact.Data;
using ServerForReact.Data.Entities;
using ServerForReact.Models;
using ServerForReact.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public IQueryable<Courses> SearchCourses(IQueryable<Courses> query, string word)
        {
            if (word == null)
            {
                return query;
            }

            query = query.Where(
                      x => x.Name.ToLower().Contains(word.ToLower())
                      || x.Description.ToLower().Contains(word.ToLower())
                      || x.Duration.ToLower().Contains(word.ToLower()));

            return query;
        }

        public IQueryable<Courses> SortingCourses(IQueryable<Courses> query, string typeOfSort, string sort)
        {
            Dictionary<string, dynamic> OrderFunctions =
                 new Dictionary<string, dynamic>
                 {
                    { "id", (Expression<Func<Courses, int>>)(x => x.Id) },
                    { "name",  (Expression<Func<Courses, string>>)(x => x.Name) }
                 };

            if (typeOfSort == "ascend")
            {
                query = Queryable.OrderBy(query, OrderFunctions[sort]);
            }
            else if (typeOfSort == "descend")
            {
                query = Queryable.OrderByDescending(query, OrderFunctions[sort]);
            }

            return query;
        }

        public CoursePaginationResultViewModel PaginationCourses(IQueryable<Courses> query, int page, int pageSize)
        {
            if (page <= 0)
            {
                page = 1;
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
                CurrentPage = page,
                Pages = pages
            };
            return res;
        }

        public Task<CoursePaginationResultViewModel> CoursesSorting(CoursePaginationViewModel search)
        {
            int page = search.Page;
            int pageSize = search.PageSize;
            var query = context.Courses
                .AsQueryable();

            query = SearchCourses(query, search.SearchWord);
            query = SortingCourses(query, search.TypeOfSort, search.Sort);
            CoursePaginationResultViewModel res = PaginationCourses(query, page, pageSize);

            return Task.FromResult(res);

        }
    }
}
