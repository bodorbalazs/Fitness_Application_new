using Fitness_Application_new.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.BLL.Validators
{
    public class FitnessPlanValidation : AbstractValidator<FitnessPlanDto>
    {
        public FitnessPlanValidation()
        {
            RuleFor(x => x.Name).Length(0, 20).WithMessage("Fitness plan name must be less than 20 characters");
        }
    }
}
