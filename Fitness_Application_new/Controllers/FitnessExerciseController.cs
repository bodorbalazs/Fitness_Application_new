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
    //[Authorize]
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
        [HttpPost("SavePicture")]
        public async Task<IActionResult> AddExercisePicture([FromForm] IFormFile image,[FromForm] string id)
        {
            try
            {
                //Path.Combine(_environment.WebRootPath, "FolderNameOfYourWWWRoot", Image.FileName);
                string path = Path.Combine(@"C:\Users\bodor\OneDrive\Desktop\egyetemshit\MSC\Dipterv1\Fitness_Application_new\ClientApp\src\assets\images", id);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
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

        // GET: Favourites/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FitnessExerciseDto fitnessExercise)
        {
            await _fitnessExerciseService.UpdateFitnessExerciseAsync(id, _mapper.Map<FitnessApp.DAL.Models.FitnessExercise>(fitnessExercise));
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
