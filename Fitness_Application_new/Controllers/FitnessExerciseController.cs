using AutoMapper;
using Fitness_Application_new.DTOs;
using Fitness_Application_new.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace Fitness_Application_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class FitnessExerciseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFitnessExerciseService _fitnessExerciseService;
        private readonly IMapper _mapper;


        public FitnessExerciseController(ApplicationDbContext context, IFitnessExerciseService fitnessExerciseService, IMapper mapper)
        {
            _context = context;
            _fitnessExerciseService = fitnessExerciseService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FitnessExerciseDto>>> GetAsync()
        {
            return _mapper.Map<List<FitnessExerciseDto>>(await _fitnessExerciseService.GetFitnessExercisesAsync());
        }

        [HttpGet("{id}")]
        public async Task<FitnessExerciseDto> Get(int id)
        {
            return _mapper.Map<FitnessExerciseDto>(await _fitnessExerciseService.GetFitnessExerciseAsync(id));
        }

        [HttpPost("SavePicture")]
        public async Task<IActionResult> AddExercisePicture([FromForm] IFormFile image, [FromForm] string id)
        {
            try
            {
                await _fitnessExerciseService.InsertExercisePictureAsync(image, id);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> AddFitnessExercise([FromBody] FitnessExerciseDto NewFitnessExercise)
        {
            var created = await _fitnessExerciseService
                .InsertFitnessExerciseAsync(_mapper.Map<FitnessApp.DAL.Models.FitnessExercise>(NewFitnessExercise));
            return Ok(created.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FitnessExerciseDto fitnessExercise)
        {
            await _fitnessExerciseService.UpdateFitnessExerciseAsync(id, _mapper.Map<FitnessApp.DAL.Models.FitnessExercise>(fitnessExercise));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _fitnessExerciseService.DeleteFitnessExerciseAsync(id);
            return NoContent();
        }
    }
}
