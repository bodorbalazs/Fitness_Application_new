using Fitness_Application_new.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.BLL.Validators
{
    public class FitnessExerciseValidation : AbstractValidator<FitnessExerciseDto>
    {
        public FitnessExerciseValidation()
        {
            RuleFor(x=> x.Name).Length(0,30).WithMessage("Fitness Exercise name must be less than 30 characters");
            RuleFor(x => x.Difficulty).Length(0, 20).WithMessage("Difficulty must be less than 20 characters");
        }
    }
}
