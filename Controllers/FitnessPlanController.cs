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
    public class FitnessPlanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFitnessPlanService _fitnessPlanService;
        private readonly IMapper _mapper;


        public FitnessPlanController(ApplicationDbContext context, IFitnessPlanService fitnessPlanService, IMapper mapper)
        {
            _context = context;
            _fitnessPlanService = fitnessPlanService;
            _mapper = mapper;
        }

        // GET: Favourites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FitnessPlanDto>>> GetAsync()
        {
            return _mapper.Map<List<FitnessPlanDto>>(await _fitnessPlanService.GetFitnessPlansAsync());
        }

        // GET: Favourites/Details/5
        [HttpGet("{id}")]
        public async Task<FitnessPlanDto> Get(int id)
        {
            return _mapper.Map<FitnessPlanDto>(await _fitnessPlanService.GetFitnessPlanAsync(id));
        }

        // POST: Favourites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> AddFitnessPlan([FromBody] FitnessPlanDto NewFitnessPlan)
        {
            var created = await _fitnessPlanService
                .InsertFitnessPlanAsync(_mapper.Map<Models.FitnessPlan>(NewFitnessPlan));
            return CreatedAtAction(
                        nameof(Get),
                        new { id = created.Id },
                        _mapper.Map<FitnessPlanDto>(created)
            );
        }

        // GET: Favourites/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FitnessPlanDto fitnessPlan)
        {
            await _fitnessPlanService.UpdateFitnessPlanAsync(id, _mapper.Map<Models.FitnessPlan>(fitnessPlan));
            return NoContent();
        }

        // GET: Favourites/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _fitnessPlanService.DeleteFitnessPlanAsync(id);
            return NoContent();
        }

        private bool FitnessPlanExists(int id)
        {
            return _context.fitnessPlan.Any(e => e.Id == id);
        }
    }
}
