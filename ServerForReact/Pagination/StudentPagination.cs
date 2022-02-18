using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerForReact.Data;
using ServerForReact.Data.Identity;
using ServerForReact.Models;
using ServerForReact.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServerForReact.Pagination
{
    public class StudentPagination
    {
        private readonly AppEFContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        public StudentPagination(AppEFContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public IQueryable<AppUser> SearchStudents(IQueryable<AppUser> query, string word)
        {
            if (word == null)
            {
                return query;
            }

            query = query.Where(
                  x => x.UserName.ToLower().Contains(word.ToLower())
                  || x.Surname.ToLower().Contains(word.ToLower())
                  || x.Email.ToLower().Contains(word.ToLower())
                  || x.PhoneNumber.Contains(word.ToString()));

            return query;
        }

        public IQueryable<AppUser> SortingStudents(IQueryable<AppUser> query, string typeOfSort, string sort)
        {
            Dictionary<string, dynamic> OrderFunctions =
                 new Dictionary<string, dynamic>
                 {
                    { "id", (Expression<Func<AppUser, long>>)(x => x.Id) },
                    { "name",  (Expression<Func<AppUser, string>>)(x => x.UserName) },
                    { "age",   (Expression<Func<AppUser, int>>)(x => x.Age) }
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

        public StudentPaginationResultViewModel PaginationStudents(IQueryable<AppUser> query, int page, int pageSize)
        {
            if (page <= 0)
            {
                page = 1;
            }
            var model = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => mapper.Map<StudentViewModel>(x))
            .ToList();
            int total = query.Count();
            int pages = (int)Math.Ceiling(total / (double)pageSize);

            return new StudentPaginationResultViewModel
            {
                Students = model,
                Total = total,
                CurrentPage = page,
                Pages = pages
            };
        }

        public Task<StudentPaginationResultViewModel> UsersSorting(StudentPaginationViewModel search)
        {
            int page = search.Page;
            int pageSize = search.PageSize;
            var query = context.Users
                .AsQueryable();

            query = SearchStudents(query, search.SearchWord);
            query = SortingStudents(query, search.TypeOfSort, search.Sort);
            StudentPaginationResultViewModel res = PaginationStudents(query, page, pageSize);

            return Task.FromResult(res);
        }
    }
}
