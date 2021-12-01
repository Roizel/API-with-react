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
        //public void UpdateStudent(RegisterViewModel model);
        public Task<string> DeleteStudent(int id);
        public bool IsEmailExist(string email); /*Check email in Validator*/
        public Task<bool> IsPasswordCorrect(LoginViewModel model); /*Check Password in Validator*/
    }
}
