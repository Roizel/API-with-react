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
    public class ValidatorRegisterViewModel : AbstractValidator<RegisterViewModel>
    {
        IStudentService studentService;
        public ValidatorRegisterViewModel(IStudentService _studentService)
        {
            studentService = _studentService;
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The Email field is required!")
                .EmailAddress().WithMessage("Email is incorrect!")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Email).Must(studentService.IsEmailExist).WithName("Email").WithMessage("This mail is already registered!");
                });

            RuleFor(x => x.Password)
                .NotEmpty().WithName("Password").WithMessage("The Password field is required!")
                .MinimumLength(5).WithName("Password").WithMessage("The Password field must be at least 5 characters long!");

            RuleFor(x => x.Age)
                .NotEmpty().WithMessage("The Age field is required!")
                .InclusiveBetween(14, 80).WithMessage("Registration is possible only from 14 to 80 years");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The Name field is required!")
                .MaximumLength(30).WithMessage("Maximum number of characters - 30")
                .MinimumLength(2).WithMessage("Minimum number of characters - 2");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("The Surname field is required!")
                .MaximumLength(30).WithMessage("Maximum number of characters - 30")
                .MinimumLength(2).WithMessage("Minimum number of characters - 2");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("The Phone field is required!");

            RuleFor(x => x.Photo)
                .NotEmpty().WithMessage("The Photo field is required!");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithName("ConfirmPassword").WithMessage("The Password field is required!")
                .Equal(x => x.Password).WithMessage("Passwords do not match!");
        }
    }
}
