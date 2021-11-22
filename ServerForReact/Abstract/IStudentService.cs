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
    }
}
