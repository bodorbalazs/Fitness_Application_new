using Fitness_Application_new.DTOs;
using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.BLL.Validators
{
    public class RatingValidation : AbstractValidator<RatingDto>
    {
        public RatingValidation()
        {
            RuleFor(x=> x.Id).NotEmpty().NotNull().WithMessage("no Id given for rating");
            RuleFor(x=> x.value).NotEmpty().InclusiveBetween(0,10).WithMessage("Rating is not between 0 and 10");
        }
    }
}
