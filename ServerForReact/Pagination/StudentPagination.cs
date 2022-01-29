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
        public Task <StudentPaginationResultViewModel> All()
        {
            int page = 1;
            int pageSize = 10;
            var query = context.Users
                .AsQueryable();

            var model = query
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .Select(x => mapper.Map<StudentViewModel>(x))
               .ToList();

            int total = query.Count();
            int pages = (int)Math.Ceiling(total / (double)pageSize);
            StudentPaginationResultViewModel res = new StudentPaginationResultViewModel
            {
                Students = model,
                Total = total,
                CurrentPage = 1,
                Pages = pages
            };

            return Task.FromResult(res);
        }
        public Task<StudentPaginationResultViewModel> UsersSorting(StudentPaginationViewModel search)
        {
            int page = search.Page;
            int pageSize = 10;
            var query = context.Users
                .AsQueryable();

            if (search.SearchWord != null)
            {
                query = query.Where(
                    x => x.UserName.ToLower().Contains(search.SearchWord.ToLower())
                    || x.Surname.ToLower().Contains(search.SearchWord.ToLower())
                    || x.Email.ToLower().Contains(search.SearchWord.ToLower())
                    || x.PhoneNumber.Contains(search.SearchWord.ToString()));
            }
            if (search.TypeOfSort == "ascend")
            {
                if (search.Sort == "name")
                {
                    query = query.OrderBy(x => x.UserName);
                }
                if (search.Sort == "id")
                {
                    query = query.OrderBy(x => x.Id);
                }
                if (search.Sort == "age")
                {
                    query = query.OrderBy(x => x.Age);
                }
            }
            if (search.TypeOfSort == "descend")
            {
                if (search.Sort == "name")
                {
                    query = query.OrderByDescending(x => x.UserName);
                }
                if (search.Sort == "id")
                {
                    query = query.OrderByDescending(x => x.Id);
                }
                if (search.Sort == "age")
                {
                    query = query.OrderByDescending(x => x.Age);
                }
            }

            var model = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => mapper.Map<StudentViewModel>(x))
                .ToList();
            int total = query.Count();
            int pages = (int)Math.Ceiling(total / (double)pageSize);

            StudentPaginationResultViewModel res = new StudentPaginationResultViewModel
            {
                Students = model,
                Total = total,
                CurrentPage = page,
                Pages = pages
            };

            return Task.FromResult(res);
        }
    }
}
