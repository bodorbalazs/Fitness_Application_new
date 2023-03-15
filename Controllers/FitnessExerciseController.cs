using AutoMapper;
using Fitness_Application_new.Data;
using Fitness_Application_new.DTOs;
using Fitness_Application_new.Interfaces;
//using Fitness_Application_new.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Application_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // GET: Favourites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FitnessExerciseDto>>> GetAsync()
        {
            return _mapper.Map<List<FitnessExerciseDto>>(await _fitnessExerciseService.GetFitnessExercisesAsync());
        }

        // GET: Favourites/Details/5
        [HttpGet("{id}")]
        public async Task<FitnessExerciseDto> Get(int id)
        {
            return _mapper.Map<FitnessExerciseDto>(await _fitnessExerciseService.GetFitnessExerciseAsync(id));
        }

        // POST: Favourites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> AddFitnessExercise([FromBody] FitnessExerciseDto NewFitnessExercise)
        {
            var created = await _fitnessExerciseService
                .InsertFitnessExerciseAsync(_mapper.Map<Models.FitnessExercise>(NewFitnessExercise));
            return CreatedAtAction(
                        nameof(Get),
                        new { id = created.Id },
                        _mapper.Map<FitnessExerciseDto>(created)
            );
        }

        // GET: Favourites/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FitnessExerciseDto fitnessExercise)
        {
            await _fitnessExerciseService.UpdateFitnessExerciseAsync(id, _mapper.Map<Models.FitnessExercise>(fitnessExercise));
            return NoContent();
        }

        // GET: Favourites/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _fitnessExerciseService.DeleteFitnessExerciseAsync(id);
            return NoContent();
        }

        private bool FitnessExerciseExists(int id)
        {
            return _context.fitnessExercise.Any(e => e.Id == id);
        }
    }
}
