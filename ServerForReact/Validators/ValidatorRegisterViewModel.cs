using FluentValidation;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        public ValidatorRegisterViewModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            RuleFor(x => x.Email)
              .NotEmpty().WithMessage("Поле пошта є обов'язковим!")
              .EmailAddress().WithMessage("Пошта є не коректною!")
              .DependentRules(() =>
              {
                  RuleFor(x => x.Email).Must(BeUniqueEmail).WithName("Email").WithMessage("Дана пошта уже зареєстрована!");
              });
            RuleFor(x => x.Password)
               .NotEmpty().WithName("Password").WithMessage("Поле пароль є обов'язковим!")
               .MinimumLength(5).WithName("Password").WithMessage("Поле пароль має містити міннімум 5 символів!");
            RuleFor(x => x.Age)
             .NotEmpty().WithMessage("Поле Age є обов'язковим!");
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Поле Name є обов'язковим!");
            RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Поле SurName є обов'язковим!");
            RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Поле Phone є обов'язковим!");
            RuleFor(x => x.Photo)
            .NotEmpty().WithMessage("Photo є обов'язковим!");
            RuleFor(x => x.ConfirmPassword)
              .NotEmpty().WithName("ConfirmPassword").WithMessage("Поле є обов'язковим!")
               .Equal(x => x.Password).WithMessage("Поролі не співпадають!");
        }


        //new Rule<string>(
        //       x =>
        //        Regex.IsMatch(x,
        //                      @"(?:(?:(\s*\(?([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*)|([2-9]1[02-9]|[2‌​-9][02-8]1|[2-9][02-8][02-9]))\)?\s*(?:[.-]\s*)?)([2-9]1[02-9]|[2-9][02-9]1|[2-9]‌​[02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})"),
        //        x => String.Format("{0} is not a valid phone number.", x));


        private bool BeUniqueEmail(string email)
        {
            return _userManager.FindByEmailAsync(email).Result == null;
        }
    }
}
