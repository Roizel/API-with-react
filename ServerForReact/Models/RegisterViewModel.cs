using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Models
{
    public class RegisterViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public IFormFile Photo { get; set; }
        public int Age { get; set; }
        public int Phone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class AccountError /*Error*/
    {
        public AccountError() { }
        public AccountError(string message)
        {
            Errors.Invalid.Add(message);
        }
        public AccountErrorItem Errors { get; set; } = new AccountErrorItem();
    }

    public class AccountErrorItem
    {
        public List<string> Invalid { get; set; } = new List<string>();
    }
}
