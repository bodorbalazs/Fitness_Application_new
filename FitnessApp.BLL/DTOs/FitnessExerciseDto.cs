﻿using FitnessApp.DAL.Models;

namespace Fitness_Application_new.DTOs
{
    public class FitnessExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public string Difficulty { get; set; }
        public int FitnessPlanId { get; set; }
        public FitnessPlanDto FitnessPlan { get; set; }
    }
}
