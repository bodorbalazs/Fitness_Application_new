using FitnessApp.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace Fitness_Application_new.DTOs
{
    public class FitnessExerciseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? PictureUrl { get; set; }
        public string? Difficulty { get; set; }
        public int? FitnessPlanId { get; set; }
        public FitnessPlanDto? FitnessPlan { get; set; }
        //public IFormFile? File { get; set; }
        public string? FileName { get; set; }

    }
}
