using AutoMapper;
using Fitness_Application_new.DTOs;
using FitnessApp.DAL.Models;

namespace FitnessApp.API.Automapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<FavouriteItem, FavouriteItemDto>().ReverseMap();
            CreateMap<FitnessPlan, FitnessPlanDto>().ReverseMap();
            CreateMap<FitnessExercise, FitnessExerciseDto>().ReverseMap();
            CreateMap<Rating, RatingDto>().ReverseMap();
        }
    }
}
