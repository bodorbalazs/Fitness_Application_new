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
using System.Text.Json.Serialization;
using FluentValidation;
using FitnessApp.BLL.Validators;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;
using Azure.Storage.Blobs;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
         
        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddScoped<IFavouriteItemService, FavouriteItemService>();
        builder.Services.AddScoped<IFitnessExerciseService, FitnessExerciseService>();
        builder.Services.AddScoped<IFitnessPlanService, FitnessPlanService>();
        builder.Services.AddScoped<IRatingService, RatingService>();

        builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobConnectionString")));


        builder.Services.AddScoped<IValidator<FitnessPlanDto>, FitnessPlanValidation>();
        builder.Services.AddScoped<IValidator<FitnessExerciseDto>, FitnessExerciseValidation>();
        builder.Services.AddScoped<IValidator<RatingDto>, RatingValidation>();
        builder.Services.AddScoped<IValidator<FavouriteItemDto>, FavouriteItemValidation>();
        builder.Services.AddFluentValidationAutoValidation();

        builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
 //           .AddDeveloperSigningCredential();

        builder.Services.AddAuthentication()
            .AddIdentityServerJwt();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddControllersWithViews();
        builder.Services.AddControllers();
        builder.Services.AddRazorPages();
        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddSwaggerDocument();
        builder.Services.AddControllers().AddJsonOptions(x =>
                        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        builder.Services.AddCors(feature =>
                        feature.AddPolicy(
                            "CorsPolicy",
                            apiPolicy => apiPolicy
                                            //.AllowAnyOrigin()
                                            //.WithOrigins("http://localhost:4200")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .SetIsOriginAllowed(host => true)
                                            .AllowCredentials()
                                        ));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseOpenApi();
            app.UseSwaggerUi3();
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
        app.UseCors("CorsPolicy");
        app.MapFallbackToFile("index.html"); ;

        app.Run();
    }
}