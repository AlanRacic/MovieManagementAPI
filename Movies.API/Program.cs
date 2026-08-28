using Microsoft.EntityFrameworkCore;
using Movies.Data.Interfaces;
using Movies.Data.Models;
using Movies.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<MovieManagementContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MoviesDB")));

builder.Services.AddScoped<IMovieRepository, MovieRepository>();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
