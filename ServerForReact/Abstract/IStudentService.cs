using ServerForReact.Data.Identity;
using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Abstract
{
    public interface IStudentService
    {
        public Task<string> CreateStudent(RegisterViewModel model);
        public Task<string> LoginStudent(LoginViewModel model);
        //public Task<AppUser> UpdateStudent(SaveEditStudentViewModel model);
        public Task<AppUser> UpdateStudent(SaveEditStudentViewModel model);
        public Task<string> DeleteStudent(int id);
        public bool IsEmailExistRegister(string email);
        public bool IsEmailExistLogin(string email);
        public Task<bool> IsPasswordCorrect(LoginViewModel model); /*Check Password in Validator*/
    }
}
