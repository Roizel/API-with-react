using FluentValidation;
using Microsoft.AspNetCore.Identity;
using ServerForReact.Abstract;
using ServerForReact.Data.Identity;
using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Validators
{
    public class ValidatorLoginViewModel : AbstractValidator<LoginViewModel>
    {
        private readonly IStudentService studentService;
        public ValidatorLoginViewModel(IStudentService _studentService)
        {
            studentService = _studentService;
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The Email field is required!")
                .EmailAddress().WithMessage("Email is incorrect!")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Email).Must(studentService.IsEmailExist).WithName("Email").WithMessage("This Email does not exist!");
                });

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The password field is required!")
                .DependentRules(() =>
                {
                    RuleFor(x => x)
                    .Must(x => studentService.IsPasswordCorrect(x).Result == true).WithMessage("The password is wrong!").WithName("Password");
                });
        }
    }
}
