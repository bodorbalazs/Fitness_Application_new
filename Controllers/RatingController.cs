using AutoMapper;
using FitnessApp.DAL.Data;
using Fitness_Application_new.DTOs;
using Fitness_Application_new.Interfaces;
//using Fitness_Application_new.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Application_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // GET: Favourites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetAsync()
        {
            return _mapper.Map<List<RatingDto>>(await _ratingService.GetRatingsAsync());
        }

        // GET: Favourites/Details/5
        [HttpGet("{id}")]
        public async Task<RatingDto> Get(int id)
        {
            return _mapper.Map<RatingDto>(await _ratingService.GetRatingAsync(id));
        }

        // POST: Favourites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] RatingDto NewRating)
        {
            var created = await _ratingService
                .InsertRatingAsync(_mapper.Map<FitnessApp.DAL.Models.Rating>(NewRating));
            return CreatedAtAction(
                        nameof(Get),
                        new { id = created.Id },
                        _mapper.Map<RatingDto>(created)
            );
        }

        // GET: Favourites/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RatingDto rating)
        {
            await _ratingService.UpdateRatingAsync(id, _mapper.Map<FitnessApp.DAL.Models.Rating>(rating));
            return NoContent();
        }

        // GET: Favourites/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ratingService.DeleteRatingAsync(id);
            return NoContent();
        }

        private bool RatingExists(int id)
        {
            return _context.rating.Any(e => e.Id == id);
        }
    }
}
