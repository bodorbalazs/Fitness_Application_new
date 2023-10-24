using AutoMapper;
using FitnessApp.DAL.Data;
using Fitness_Application_new.DTOs;
using Fitness_Application_new.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;

namespace Fitness_Application_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRatingService _ratingService;
        private readonly IMapper _mapper;


        public RatingController(ApplicationDbContext context, IRatingService ratingService, IMapper mapper)
        {
            _context = context;
            _ratingService = ratingService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetAsync()
        {
            return _mapper.Map<List<RatingDto>>(await _ratingService.GetRatingsAsync());
        }

        [HttpGet("{id}")]
        public async Task<RatingDto> Get(int id)
        {
            return _mapper.Map<RatingDto>(await _ratingService.GetRatingAsync(id));
        }

        [HttpGet("SpecificEventRating")]
        public async Task<RatingDto> GetSpecificEventRating(int planId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _mapper.Map<RatingDto>(await _ratingService.GetUserSpecificEventRating(planId, userId));
        }

        [HttpGet("SpecificEventAverageRating")]
        public async Task<int> GetSpecificEventAverageRating(int planId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _ratingService.GetSpecificEventAverageScore(planId);
        }

        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] RatingDto NewRating)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            NewRating.ApplicationUserId = userId;
            var created = await _ratingService
                .InsertRatingAsync(_mapper.Map<FitnessApp.DAL.Models.Rating>(NewRating));
            return Ok(created.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RatingDto rating)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            rating.ApplicationUserId = userId;
            await _ratingService.UpdateRatingAsync(id, _mapper.Map<FitnessApp.DAL.Models.Rating>(rating));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ratingService.DeleteRatingAsync(id);
            return NoContent();
        }

        [HttpDelete("DeletePlansratings")]
        public async Task<IActionResult> DeletePlansRating(int PlanId)
        {
            await _ratingService.DeletePlansRatingAsync(PlanId);
            return NoContent();
        }
    }
}
