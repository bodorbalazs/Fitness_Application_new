﻿using AutoMapper;
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
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class FitnessPlanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFitnessPlanService _fitnessPlanService;
        private readonly IMapper _mapper;


        public FitnessPlanController(ApplicationDbContext context, IFitnessPlanService fitnessPlanService,
            IMapper mapper)
        {
            _context = context;
            _fitnessPlanService = fitnessPlanService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FitnessPlanDto>>> GetAsync()
        {
            return _mapper.Map<List<FitnessPlanDto>>(await _fitnessPlanService.GetFitnessPlansAsync());
        }

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

        [HttpPost]
        public async Task<IActionResult> AddFitnessPlan([FromBody] FitnessPlanDto NewFitnessPlan)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            NewFitnessPlan.ApplicationUserId = userId;
            var created = await _fitnessPlanService
                .InsertFitnessPlanAsync(_mapper.Map<FitnessApp.DAL.Models.FitnessPlan>(NewFitnessPlan));
            return Ok(created.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FitnessPlanDto fitnessPlan)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            fitnessPlan.ApplicationUserId = userId;
            await _fitnessPlanService.UpdateFitnessPlanAsync(id, _mapper.Map<FitnessApp.DAL.Models.FitnessPlan>(fitnessPlan));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _fitnessPlanService.DeleteFitnessPlanAsync(id);
            return NoContent();
        }
    }
}
