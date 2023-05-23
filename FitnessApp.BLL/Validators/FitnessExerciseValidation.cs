﻿using Fitness_Application_new.DTOs;
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
            RuleFor(x=> x.Id).NotNull().NotEmpty();
        }
    }
}