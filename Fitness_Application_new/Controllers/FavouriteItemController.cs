using AutoMapper;
using Fitness_Application_new.DTOs;
using Fitness_Application_new.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FitnessApp.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Fitness_Application_new.Services;
using System.Security.Claims;

namespace Fitness_Application_new.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class FavouriteItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFavouriteItemService _favouriteItemService;
        private readonly IMapper _mapper;


        public FavouriteItemController(ApplicationDbContext context, IFavouriteItemService favouriteItemService, IMapper mapper)
        {
            _context = context;
            _favouriteItemService = favouriteItemService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavouriteItemDto>>> GetAsync()
        {
            return _mapper.Map<List<FavouriteItemDto>>(await _favouriteItemService.GetFavouriteItemsAsync());
        }

        [HttpGet("GetUsersFavourites")]
        public async Task<ActionResult<IEnumerable<FavouriteItemDto>>> GetUsersPlansAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _mapper.Map<List<FavouriteItemDto>>(await _favouriteItemService.GetUsersFavouriteItemsAsync(userId));
        }

        [HttpGet("GetPlanUsersFavourite")]
        public async Task<ActionResult<FavouriteItemDto>> GetPlanUsersFavouriteAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _mapper.Map<FavouriteItemDto>(await _favouriteItemService.GetPlanUsersFavouriteItemAsync(userId, id));
        }

        [HttpGet("{id}")]
        public async Task<FavouriteItemDto> Get(int id)
        {
            return _mapper.Map<FavouriteItemDto>(await _favouriteItemService.GetFavouriteItemAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddFavouriteItem([FromBody] FavouriteItemDto Newfavourite)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Newfavourite.ApplicationUserId = userId;
            var created = await _favouriteItemService
                .InsertFavouriteItemAsync(_mapper.Map<FitnessApp.DAL.Models.FavouriteItem>(Newfavourite));
            return Ok(created.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FavouriteItemDto favourite)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            favourite.ApplicationUserId = userId;
            await _favouriteItemService.UpdateFavouriteItemAsync(id, _mapper.Map<FitnessApp.DAL.Models.FavouriteItem>(favourite));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _favouriteItemService.DeleteFavouriteItemAsync(id);
            return NoContent();
        }
    }
}
