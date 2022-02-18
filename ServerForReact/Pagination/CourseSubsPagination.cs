using AutoMapper;
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
    public class CourseSubsPagination
    {
        private readonly AppEFContext context;
        private readonly IMapper mapper;
        public CourseSubsPagination(AppEFContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<CourseStudentViewModel> GetUserSubs(string UserName)
        {
            if (UserName == null)
            {
                return null;
            }
            var student = context.Users.SingleOrDefault(x => x.UserName == UserName);
            var list = context.StudentCourses.Select(x => mapper.Map<CourseStudentViewModel>(x)).ToList();
            var subs = list.Where(x => x.StudentId == student.Id);

            return subs;
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

        public CourseSubsPaginationResultViewModel PaginationCourses(IQueryable<Courses> query, int page, int pageSize, string UserName)
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

            var subs = GetUserSubs(UserName); /*Get all subscriptions of student*/

            CourseSubsPaginationResultViewModel res = new CourseSubsPaginationResultViewModel
            {
                Courses = model,
                Total = total,
                CurrentPage = page,
                Pages = pages,
                subscriptions = subs
            };
            return res;
        }

        public Task<CourseSubsPaginationResultViewModel> CoursesSubsSorting(CourseSubsPaginationViewModel search)
        {
            int page = search.Page;
            int pageSize = search.PageSize;
            string UserName = search.Name;
            var query = context.Courses
                .AsQueryable();

            query = SearchCourses(query, search.SearchWord);
            query = SortingCourses(query, search.TypeOfSort, search.Sort);
            CourseSubsPaginationResultViewModel res = PaginationCourses(query, page, pageSize, UserName);

            return Task.FromResult(res);

        }
    }
}
