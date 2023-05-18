using AutoMapper;
using FitnessApp.DAL.Data;
using Fitness_Application_new.DTOs;
using Fitness_Application_new.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Cors;

namespace Fitness_Application_new.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class FitnessPlanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFitnessPlanService _fitnessPlanService;
        private readonly IMapper _mapper;
        private IValidator<FitnessPlanDto> _validator;
        private readonly string AppDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");


        public FitnessPlanController(ApplicationDbContext context, IFitnessPlanService fitnessPlanService, IMapper mapper,IValidator<FitnessPlanDto> validator)
        {
            _validator = validator;
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

        [HttpGet("GetUsersPlans")]
        public async Task<ActionResult<IEnumerable<FitnessPlanDto>>> GetUsersPlansAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _mapper.Map<List<FitnessPlanDto>>(await _fitnessPlanService.GetUserFitnessPlansAsync(userId));
        }

        // POST: Favourites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> AddFitnessPlan([FromBody] FitnessPlanDto NewFitnessPlan)
        {
            ValidationResult result = await _validator.ValidateAsync(NewFitnessPlan);
            if (!result.IsValid)
            {
                return BadRequest();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            NewFitnessPlan.ApplicationUserId = userId;
            var created = await _fitnessPlanService
                .InsertFitnessPlanAsync(_mapper.Map<FitnessApp.DAL.Models.FitnessPlan>(NewFitnessPlan));
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
            ValidationResult result = await _validator.ValidateAsync(fitnessPlan);
            if (!result.IsValid)
            {
                return BadRequest();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            fitnessPlan.ApplicationUserId = userId;
            await _fitnessPlanService.UpdateFitnessPlanAsync(id, _mapper.Map<FitnessApp.DAL.Models.FitnessPlan>(fitnessPlan));
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
