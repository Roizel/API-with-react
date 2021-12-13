using FluentValidation;
using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Validators
{
    public class ValidatorSaveEditStudentViewModel : AbstractValidator<SaveEditStudentViewModel>
    {
        public ValidatorSaveEditStudentViewModel()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The Email field is required!")
                .EmailAddress().WithMessage("Email is incorrect!");

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
        }
    }
}
