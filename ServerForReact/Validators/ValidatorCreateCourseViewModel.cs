using FluentValidation;
using Microsoft.AspNetCore.Identity;
using ServerForReact.Data;
using ServerForReact.Data.Identity;
using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Validators
{
    public class ValidatorCreateCourseViewModel : AbstractValidator<CreateCourseViewModel>
    {
        public ValidatorCreateCourseViewModel()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The Name field is required!")
                .MaximumLength(30).WithMessage("Maximum number of characters - 30")
                .MinimumLength(2).WithMessage("Minimum number of characters - 2");

            RuleFor(x => x.Photo)
                .NotEmpty().WithMessage("The Photo field is required!");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("The Description field is required!")
                .MaximumLength(1000).WithMessage("Maximum number of characters - 1000")
                .MinimumLength(20).WithMessage("Minimum number of characters - 20");

            RuleFor(x => x.Duration)
                .NotEmpty().WithMessage("The Duration field is required!")
                .MaximumLength(30).WithMessage("Maximum number of characters - 30")
                .MinimumLength(4).WithMessage("Minimum number of characters - 4");

            RuleFor(x => x.StartCourse)
                .NotEmpty().WithMessage("The StartCourse field is required!")
                .Must(BeAValidDate).WithMessage("Wrong Data");
        }
        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
