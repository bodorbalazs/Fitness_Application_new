using AutoMapper;
using Duende.IdentityServer.Services;
using FitnessApp.DAL.Data;
using Fitness_Application_new.DTOs;
using Fitness_Application_new.Interfaces;
using FitnessApp.DAL.Models;
using Fitness_Application_new.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddTransient<IFavouriteItemService, FavouriteItemService>();
builder.Services.AddTransient<IFitnessExerciseService, FitnessExerciseService>();
builder.Services.AddTransient<IFitnessPlanService, FitnessPlanService>();
builder.Services.AddTransient<IRatingService, RatingService>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddSwaggerDocument();

var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<FavouriteItem, FavouriteItemDto>();
    cfg.CreateMap<FitnessPlan, FitnessPlanDto>();
    cfg.CreateMap<FitnessExercise, FitnessExerciseDto>();
    cfg.CreateMap<Rating, RatingDto>();
});
// only during development, validate your mappings; remove it before release
#if DEBUG
configuration.AssertConfigurationIsValid();
#endif
// use DI (http://docs.automapper.org/en/latest/Dependency-injection.html) or create the mapper yourself
var mapper = configuration.CreateMapper();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseMigrationsEndPoint();
    app.UseOpenApi();
    app.UseSwaggerUi3(config =>
    {
        config.TransformToExternalPath = (s, r) =>
        {
            string path = s.EndsWith("swagger.json") && !string.IsNullOrEmpty(r.PathBase)
                ? $"{r.PathBase}{s}"
                : s;
            return path;
        };
    });
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapFallbackToFile("index.html"); ;

app.Run();
