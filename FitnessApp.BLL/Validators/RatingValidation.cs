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
            RuleFor(x=> x.Id).NotEmpty();
            RuleFor(x=> x.value).NotEmpty();
        }
    }
}
