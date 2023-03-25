using AutoMapper;
using Fitness_Application_new.DTOs;
using Fitness_Application_new.Interfaces;
//using Fitness_Application_new.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FitnessApp.DAL.Data;
using Microsoft.AspNetCore.Authorization;

namespace Fitness_Application_new.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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

        // GET: Favourites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavouriteItemDto>>> GetAsync()
        {
             return _mapper.Map<List<FavouriteItemDto>>(await _favouriteItemService.GetFavouriteItemsAsync());
        }

        // GET: Favourites/Details/5
        [HttpGet("{id}")]
        public async Task<FavouriteItemDto> Get(int id)
        {
            return _mapper.Map<FavouriteItemDto>(await _favouriteItemService.GetFavouriteItemAsync(id));
        }

        // POST: Favourites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> AddFavouriteItem([FromBody] FavouriteItemDto Newfavourite)
        {
            var created = await _favouriteItemService
                .InsertFavouriteItemAsync(_mapper.Map<FitnessApp.DAL.Models.FavouriteItem>(Newfavourite));
            return CreatedAtAction(
                        nameof(Get),
                        new { id = created.Id },
                        _mapper.Map<FavouriteItemDto>(created)
            );
        }

        // GET: Favourites/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FavouriteItemDto favourite)
        {
            await _favouriteItemService.UpdateFavouriteItemAsync(id, _mapper.Map<FitnessApp.DAL.Models.FavouriteItem>(favourite));
            return NoContent();
        }

        // GET: Favourites/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _favouriteItemService.DeleteFavouriteItemAsync(id);
            return NoContent();
        }

        private bool FavouriteExists(int id)
        {
            return _context.favouriteItems.Any(e => e.Id == id);
        }
    }
}
